#nullable enable

using System.Net.Http;

namespace CalculoIRRF.Infrastructure.Tributacao;

/// <summary>
/// Consulta as fontes de uma tabela ao mesmo tempo e escolhe a competência mais recente em que se pode confiar:
/// a publicada pela fonte oficial ou, enquanto ela não a publica, a que duas fontes alternativas trazem com os mesmos valores.
/// Uma fonte sozinha, ou fontes que divergem, não alteram os dados: um erro de digitação em um site não chega ao banco.
/// </summary>
internal static class ConsultaDeFontes
{
    public static async Task<TabelaEscolhida<T>> EscolherAsync<T>(IReadOnlyList<IFonteTabela<T>> fontes, Func<HttpClient> criarHttpClient, string nomeTabela, CancellationToken cancellationToken)
        where T : class, ITabelaPublicada<T>
    {
        Consulta<T>[] consultas;
        using (var httpClient = criarHttpClient())
            consultas = await Task.WhenAll(fontes.Select(fonte => ConsultarAsync(fonte, httpClient, cancellationToken)));

        return Escolher(consultas, nomeTabela);
    }

    private static async Task<Consulta<T>> ConsultarAsync<T>(IFonteTabela<T> fonte, HttpClient httpClient, CancellationToken cancellationToken)
        where T : class, ITabelaPublicada<T>
    {
        try
        {
            var tabela = await fonte.ObterAsync(httpClient, cancellationToken);
            tabela.Validar();
            return new Consulta<T>(fonte, tabela, null);
        }
        // Uma fonte fora do ar ou com a página alterada não impede as outras; só o cancelamento pedido interrompe a consulta.
        catch (Exception ex) when (!cancellationToken.IsCancellationRequested)
        {
            return new Consulta<T>(fonte, null, DescreverFalha(ex));
        }
    }

    private static TabelaEscolhida<T> Escolher<T>(Consulta<T>[] consultas, string nomeTabela)
        where T : class, ITabelaPublicada<T>
    {
        var observacoes = new List<string>();
        var publicadas = consultas.Where(consulta => consulta.Tabela is not null).Select(consulta => (consulta.Fonte, Tabela: consulta.Tabela!));

        foreach (var grupo in publicadas.GroupBy(item => item.Tabela.Competencia).OrderByDescending(grupo => grupo.Key))
        {
            var oficial = grupo.FirstOrDefault(item => item.Fonte.Oficial);
            if (oficial.Tabela is not null)
                return new TabelaEscolhida<T>(oficial.Tabela, [oficial.Fonte.Nome], true, observacoes);

            var confirmadas = grupo
                .Select(item => grupo.Where(outra => outra.Tabela.TemMesmosValores(item.Tabela)).ToArray())
                .FirstOrDefault(iguais => iguais.Length >= 2);
            if (confirmadas is not null)
            {
                observacoes.Add(DescreverFonteOficial(consultas));
                return new TabelaEscolhida<T>(confirmadas[0].Tabela, confirmadas.Select(item => item.Fonte.Nome).ToArray(), false, observacoes);
            }

            observacoes.Add(grupo.Count() == 1
                ? $"{grupo.First().Fonte.Nome} já mostra a tabela de {grupo.Key:MM/yyyy}, ainda sem a confirmação de outra fonte."
                : $"As fontes alternativas divergem nos valores de {grupo.Key:MM/yyyy}, e essa tabela não foi importada.");
        }

        var detalhes = consultas.Select(consulta => consulta.Tabela is not { } tabela
            ? $"• {consulta.Fonte.Nome}: {consulta.Falha}"
            : consultas.Count(outra => outra.Tabela?.Competencia == tabela.Competencia) > 1
                ? $"• {consulta.Fonte.Nome}: tabela de {tabela.Competencia:MM/yyyy} com valores diferentes dos da outra fonte."
                : $"• {consulta.Fonte.Nome}: tabela de {tabela.Competencia:MM/yyyy} sem a confirmação de outra fonte.");
        throw new InvalidOperationException($"Nenhuma fonte confirmou a tabela de {nomeTabela}. Os dados locais não foram alterados.\n\n{string.Join("\n", detalhes)}");
    }

    private static string DescreverFonteOficial<T>(Consulta<T>[] consultas) where T : class, ITabelaPublicada<T>
    {
        var oficial = consultas.FirstOrDefault(consulta => consulta.Fonte.Oficial);
        return oficial?.Tabela is { } tabela
            ? $"A página oficial ainda mostra a tabela de {tabela.Competencia:MM/yyyy}."
            : "A página oficial não pôde ser consultada.";
    }

    private static string DescreverFalha(Exception ex) => ex switch
    {
        // O HttpClient sinaliza o tempo esgotado como cancelamento.
        OperationCanceledException => "A página não respondeu a tempo.",
        HttpRequestException { StatusCode: { } status } => $"A página respondeu com o erro {(int)status}.",
        HttpRequestException => "Não foi possível acessar a página. Verifique a conexão com a internet.",
        _ => ex.Message
    };

    private sealed record Consulta<T>(IFonteTabela<T> Fonte, T? Tabela, string? Falha) where T : class;
}

/// <param name="Fontes">Nomes das fontes que publicaram os valores escolhidos.</param>
internal sealed record TabelaEscolhida<T>(T Tabela, IReadOnlyList<string> Fontes, bool Oficial, IReadOnlyList<string> Observacoes);
