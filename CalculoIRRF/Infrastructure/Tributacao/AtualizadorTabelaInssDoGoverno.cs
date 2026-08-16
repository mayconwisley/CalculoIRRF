#nullable enable

using CalculoIRRF.Application.Management;
using CalculoIRRF.Infrastructure.Persistence;
using CalculoIRRF.Infrastructure.Persistence.Entities;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace CalculoIRRF.Infrastructure.Tributacao;

/// <summary>Importa a tabela progressiva dos segurados empregado, doméstico e trabalhador avulso.</summary>
public sealed class AtualizadorTabelaInssDoGoverno(
    HttpClient httpClient,
    CalculoIrrfDbContext context) : IAtualizadorTabelaInss
{
    private const string UrlTabela = "https://www.gov.br/inss/pt-br/direitos-e-deveres/inscricao-e-contribuicao/tabela-de-contribuicao-mensal";
    private static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("pt-BR");
    private static readonly Regex ValorMonetario = new(@"\d{1,3}(?:\.\d{3})*,\d{2}", RegexOptions.Compiled);
    private static readonly Regex Percentual = new(@"\d{1,2}(?:,\d+)?", RegexOptions.Compiled);

    public Uri FonteOficial => new(UrlTabela);

    public async Task<AtualizacaoTabelaInssResultado> AtualizarAsync(CancellationToken cancellationToken)
    {
        using var resposta = await httpClient.GetAsync(FonteOficial, cancellationToken);
        resposta.EnsureSuccessStatusCode();

        var documento = new HtmlDocument();
        documento.LoadHtml(await resposta.Content.ReadAsStringAsync(cancellationToken));
        var tabela = ExtrairTabela(documento);
        await PersistirAsync(tabela, cancellationToken);

        return new AtualizacaoTabelaInssResultado(tabela.Competencia, tabela.Faixas.Count, FonteOficial);
    }

    private static TabelaInssOnline ExtrairTabela(HtmlDocument documento)
    {
        var tabelas = documento.DocumentNode.SelectNodes("//table") ?? Enumerable.Empty<HtmlNode>();
        var tabela = tabelas.FirstOrDefault(item => Normalizar(item.InnerText).Contains("aliquota progressiva para fins de recolhimento ao inss", StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException("A tabela de INSS para empregados não foi encontrada na página oficial.");

        var faixas = (tabela.SelectNodes(".//tr[td]") ?? Enumerable.Empty<HtmlNode>())
            .Select(linha => linha.SelectNodes("./td")?.Select(coluna => WebUtility.HtmlDecode(coluna.InnerText).Trim()).ToArray() ?? Array.Empty<string>())
            .Where(colunas => colunas.Length >= 2 && ValorMonetario.IsMatch(colunas[0]))
            .Select((colunas, indice) => new FaixaInssOnline(indice + 1, ExtrairUltimoValor(colunas[0]), ExtrairPercentual(colunas[1])))
            .ToArray();

        if (faixas.Length != 4 || faixas.Any(faixa => faixa.Limite <= 0m) || faixas.Select(faixa => faixa.Aliquota).Distinct().Count() != 4)
            throw new InvalidOperationException("A página oficial retornou uma estrutura de faixas INSS inválida. Nenhum dado foi alterado.");

        var competencia = ExtrairCompetencia(documento.DocumentNode.InnerText);
        return new TabelaInssOnline(competencia, faixas);
    }

    private async Task PersistirAsync(TabelaInssOnline tabela, CancellationToken cancellationToken)
    {
        var competencia = tabela.Competencia.ToDateTime(TimeOnly.MinValue);
        await using var transacao = await context.Database.BeginTransactionAsync(cancellationToken);
        var existentes = await context.FaixasInss.Where(item => item.Competencia == competencia).ToListAsync(cancellationToken);
        context.FaixasInss.RemoveRange(existentes);
        context.FaixasInss.AddRange(tabela.Faixas.Select(faixa => new FaixaInssEntity
        {
            Competencia = competencia,
            Faixa = faixa.Faixa,
            Valor = (double)faixa.Limite,
            Porcentagem = (double)faixa.Aliquota
        }));
        await context.SaveChangesAsync(cancellationToken);
        await transacao.CommitAsync(cancellationToken);
    }

    private static DateOnly ExtrairCompetencia(string conteudo)
    {
        var normalizado = Normalizar(conteudo);
        var match = Regex.Match(normalizado, @"(?:competencia\s+)?janeiro de (?<ano>20\d{2})", RegexOptions.IgnoreCase);
        if (!match.Success || !int.TryParse(match.Groups["ano"].Value, out var ano))
            throw new InvalidOperationException("Não foi possível identificar a competência da tabela oficial de INSS.");

        return new DateOnly(ano, 1, 1);
    }

    private static decimal ExtrairUltimoValor(string texto)
    {
        var valores = ValorMonetario.Matches(texto).Select(match => decimal.Parse(match.Value, Cultura)).ToArray();
        return valores.LastOrDefault();
    }

    private static decimal ExtrairPercentual(string texto)
    {
        var match = Percentual.Match(texto);
        return match.Success ? decimal.Parse(match.Value, Cultura) : 0m;
    }

    private static string Normalizar(string valor)
    {
        var semAcentos = string.Concat(WebUtility.HtmlDecode(valor)
            .Normalize(NormalizationForm.FormD)
            .Where(caractere => CharUnicodeInfo.GetUnicodeCategory(caractere) != UnicodeCategory.NonSpacingMark));
        return semAcentos.Replace('\u00A0', ' ').Trim();
    }

    private sealed record TabelaInssOnline(DateOnly Competencia, IReadOnlyList<FaixaInssOnline> Faixas);
    private sealed record FaixaInssOnline(int Faixa, decimal Limite, decimal Aliquota);
}
