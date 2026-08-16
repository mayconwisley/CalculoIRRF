#nullable enable

using CalculoIRRF.Application.Abstractions;
using CalculoIRRF.Application.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;
using System.IO;

namespace CalculoIRRF.Infrastructure.Reporting;

public sealed class QuestPdfRelatorioPdfService : IRelatorioPdfService
{
    private static readonly CultureInfo CulturaPtBr = CultureInfo.GetCultureInfo("pt-BR");
    private const string AzulPrimario = "1D4ED8";
    private const string AzulClaro = "EFF6FF";
    private const string CinzaBorda = "D1D5DB";
    private const string CinzaTexto = "4B5563";
    private const string VerdeVantagem = "027A48";
    private const string VerdeClaroVantagem = "ECFDF3";
    private const string VerdeBordaVantagem = "86EFAC";

    public Task GerarRelatorioImpostoAsync(SimulacaoImpostoDto simulacao, string caminhoArquivo, CancellationToken cancellationToken) =>
        GerarAsync(caminhoArquivo, cancellationToken, documento => documento.Page(pagina =>
        {
            ConfigurarPagina(pagina, "Relatório de simulação tributária", simulacao.Entrada.Competencia);
            pagina.Content().Column(coluna =>
            {
                coluna.Spacing(16);
                coluna.Item().Element(container => CriarResumoImposto(container, simulacao));
                coluna.Item().Element(container => CriarComparativoIrrf(container, simulacao));
                coluna.Item().Element(container => CriarMemoriaIrrf(container, simulacao, simulacao.Normal));
                coluna.Item().Element(container => CriarMemoriaIrrf(container, simulacao, simulacao.Simplificada));
                coluna.Item().Element(container => CriarDetalhesFaixas(container, "Detalhamento do INSS", simulacao.DetalhesInss));
                coluna.Item().Element(container => CriarDetalhesFaixas(container, "IRRF normal - cálculo progressivo", simulacao.Normal.DetalhesProgressivos));
                coluna.Item().Element(container => CriarDetalhesFaixas(container, "IRRF simplificado - cálculo progressivo", simulacao.Simplificada.DetalhesProgressivos));
            });
        }));

    public Task GerarRelatorioPensaoAsync(SimulacaoPensaoDto simulacao, EntradaPensaoDto entrada, bool incluirDetalhes, string caminhoArquivo, CancellationToken cancellationToken) =>
        GerarAsync(caminhoArquivo, cancellationToken, documento => documento.Page(pagina =>
        {
            ConfigurarPagina(pagina, "Relatório de cálculo de pensão alimentícia", entrada.Competencia);
            pagina.Content().Column(coluna =>
            {
                coluna.Spacing(16);
                coluna.Item().Element(container => CriarResumoPensao(container, simulacao, entrada));
                coluna.Item().Element(container => CriarComparativoPensao(container, simulacao));

                if (incluirDetalhes)
                {
                    coluna.Item().Element(container => CriarDetalhesPensao(container, simulacao.Normal, entrada, simulacao.ValorInss));
                    coluna.Item().PageBreak();
                    coluna.Item().Element(container => CriarDetalhesPensao(container, simulacao.Simplificada, entrada, simulacao.ValorInss));
                }
            });
        }));

    public Task GerarRelatorioEstabilidadeAsync(SimulacaoEstabilidadeDto simulacao, EntradaEstabilidadeDto entrada, string caminhoArquivo, CancellationToken cancellationToken) =>
        GerarAsync(caminhoArquivo, cancellationToken, documento => documento.Page(pagina =>
        {
            ConfigurarPagina(pagina, "Demonstrativo de cálculo de estabilidade", $"Emissão: {DateTime.Now:dd/MM/yyyy HH:mm}");
            pagina.Content().Column(coluna =>
            {
                coluna.Spacing(16);
                coluna.Item().Element(container => CriarVerbasEstabilidade(container, simulacao));
                coluna.Item().Element(container => CriarDadosEstabilidade(container, simulacao, entrada));
                coluna.Item().Background(AzulPrimario).Padding(14).Row(total =>
                {
                    total.RelativeItem().Text("Total a receber").FontSize(14).SemiBold().FontColor(Colors.White);
                    total.AutoItem().Text(Moeda(simulacao.Total)).FontSize(16).SemiBold().FontColor(Colors.White);
                });
            });
        }));

    private static Task GerarAsync(string caminhoArquivo, CancellationToken cancellationToken, Action<IDocumentContainer> criarDocumento)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var diretorio = Path.GetDirectoryName(caminhoArquivo);
        if (string.IsNullOrWhiteSpace(diretorio))
            throw new ArgumentException("Informe um caminho válido para o arquivo PDF.", nameof(caminhoArquivo));

        Directory.CreateDirectory(diretorio);
        return Task.Run(() =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            Document.Create(criarDocumento).GeneratePdf(caminhoArquivo);
        }, cancellationToken);
    }

    private static void ConfigurarPagina(PageDescriptor pagina, string titulo, DateOnly competencia)
        => ConfigurarPagina(pagina, titulo, $"Competência: {competencia:MM/yyyy}  |  Emissão: {DateTime.Now:dd/MM/yyyy HH:mm}");

    private static void ConfigurarPagina(PageDescriptor pagina, string titulo, string subtitulo)
    {
        pagina.Size(PageSizes.A4);
        pagina.Margin(36);
        pagina.DefaultTextStyle(estilo => estilo.FontFamily(Fonts.Arial).FontSize(9).FontColor(CinzaTexto));
        pagina.Header().Column(cabecalho =>
        {
            cabecalho.Spacing(4);
            cabecalho.Item().Text(titulo).FontSize(20).SemiBold().FontColor(AzulPrimario);
            cabecalho.Item().Text(subtitulo).FontSize(9).FontColor(CinzaTexto);
            cabecalho.Item().PaddingTop(8).LineHorizontal(1).LineColor(AzulPrimario);
        });
        pagina.Footer().PaddingTop(12).Row(rodape =>
        {
            rodape.RelativeItem().Text("Calculadora de Imposto - dados processados localmente").FontSize(8).FontColor(CinzaTexto);
            rodape.AutoItem().Text(texto =>
            {
                texto.Span("Página ").FontSize(8).FontColor(CinzaTexto);
                texto.CurrentPageNumber().FontSize(8).FontColor(CinzaTexto);
                texto.Span(" de ").FontSize(8).FontColor(CinzaTexto);
                texto.TotalPages().FontSize(8).FontColor(CinzaTexto);
            });
        });
    }

    private static void CriarVerbasEstabilidade(IContainer container, SimulacaoEstabilidadeDto simulacao)
    {
        CriarSecao(container, "Valores da indenização", secao => secao.Table(tabela =>
        {
            tabela.ColumnsDefinition(colunas => { colunas.RelativeColumn(2); colunas.RelativeColumn(); });
            CabecalhoTabela(tabela.Cell(), "Estabilidade"); CabecalhoTabela(tabela.Cell(), "Valor");
            LinhaEstabilidade(tabela, "Indenização", simulacao.Indenizacao);
            LinhaEstabilidade(tabela, "13º salário", simulacao.DecimoTerceiro);
            LinhaEstabilidade(tabela, "Férias", simulacao.Ferias);
            LinhaEstabilidade(tabela, "1/3 de férias", simulacao.TercoFerias);
            LinhaEstabilidade(tabela, "FGTS 8%", simulacao.FgtsOitoPorCento);
            LinhaEstabilidade(tabela, "FGTS 40%", simulacao.MultaFgtsQuarentaPorCento);
            LinhaEstabilidade(tabela, "Complementos", simulacao.Complementos);
            LinhaEstabilidade(tabela, "Subtotal", simulacao.Total, true);
        }));
    }

    private static void CriarDadosEstabilidade(IContainer container, SimulacaoEstabilidadeDto simulacao, EntradaEstabilidadeDto entrada)
    {
        CriarSecao(container, "Dados considerados no cálculo", secao => secao.Table(tabela =>
        {
            tabela.ColumnsDefinition(colunas =>
            {
                colunas.RelativeColumn(14); colunas.RelativeColumn(10);
                colunas.RelativeColumn(14); colunas.RelativeColumn(10);
            });
            CelulaRotulo(tabela.Cell(), "Demissão"); CelulaValor(tabela.Cell(), entrada.Demissao.ToString("dd/MM/yyyy", CulturaPtBr));
            CelulaRotulo(tabela.Cell(), "Fim da estabilidade"); CelulaValor(tabela.Cell(), entrada.FimEstabilidade.ToString("dd/MM/yyyy", CulturaPtBr));
            CelulaRotulo(tabela.Cell(), "Média para cálculo"); CelulaValor(tabela.Cell(), Moeda(entrada.MediaRemuneratoria));
            CelulaRotulo(tabela.Cell(), "Dias-base"); CelulaValor(tabela.Cell(), entrada.DiasBase.ToString(CulturaPtBr));
            CelulaRotulo(tabela.Cell(), "Dias de estabilidade"); CelulaValor(tabela.Cell(), simulacao.DiasEstabilidade.ToString(CulturaPtBr));
            CelulaRotulo(tabela.Cell(), "Avos de 13º e férias"); CelulaValor(tabela.Cell(), simulacao.Avos.ToString(CulturaPtBr));
        }));
    }

    private static void LinhaEstabilidade(TableDescriptor tabela, string descricao, decimal valor, bool destaque = false)
    {
        var corFundo = destaque ? AzulClaro : null;
        var celulaDescricao = tabela.Cell().BorderBottom(1).BorderColor(CinzaBorda).Padding(6);
        var celulaValor = tabela.Cell().BorderBottom(1).BorderColor(CinzaBorda).Padding(6);
        if (corFundo is not null)
        {
            celulaDescricao = celulaDescricao.Background(corFundo);
            celulaValor = celulaValor.Background(corFundo);
        }

        var textoDescricao = celulaDescricao.Text(descricao).FontColor(CinzaTexto);
        var textoValor = celulaValor.AlignRight().Text(Moeda(valor)).FontColor(CinzaTexto);
        if (destaque)
        {
            textoDescricao.SemiBold();
            textoValor.SemiBold();
        }
    }

    private static void CriarResumoImposto(IContainer container, SimulacaoImpostoDto simulacao)
    {
        CriarSecao(container, "Dados considerados", secao => secao.Table(tabela =>
        {
            tabela.ColumnsDefinition(colunas => { colunas.RelativeColumn(); colunas.RelativeColumn(); colunas.RelativeColumn(); colunas.RelativeColumn(); });
            CelulaRotulo(tabela.Cell(), "Valor bruto"); CelulaValor(tabela.Cell(), Moeda(simulacao.Entrada.ValorBruto));
            CelulaRotulo(tabela.Cell(), "Base de INSS"); CelulaValor(tabela.Cell(), Moeda(simulacao.BaseInssConsiderada));
            CelulaRotulo(tabela.Cell(), "Dependentes"); CelulaValor(tabela.Cell(), simulacao.Entrada.QuantidadeDependentes.ToString(CulturaPtBr));
            CelulaRotulo(tabela.Cell(), "INSS calculado"); CelulaValor(tabela.Cell(), Moeda(simulacao.ValorInss));
            CelulaRotulo(tabela.Cell(), "FGTS padrão (8%)"); CelulaValor(tabela.Cell(), Moeda(simulacao.FgtsOitoPorCento));
            CelulaRotulo(tabela.Cell(), "FGTS Jovem Aprendiz (2%)"); CelulaValor(tabela.Cell(), Moeda(simulacao.FgtsDoisPorCento));
        }));
    }

    private static void CriarComparativoIrrf(IContainer container, SimulacaoImpostoDto simulacao)
    {
        CriarSecao(container, "Comparativo de IRRF", secao => secao.Column(coluna =>
        {
            coluna.Spacing(8);
            var modalidadeMaisVantajosa = simulacao.ModalidadeMaisVantajosa;
            if (modalidadeMaisVantajosa is not null)
                coluna.Item().Background(VerdeClaroVantagem).Border(1).BorderColor(VerdeBordaVantagem).Padding(8).Text(simulacao.MensagemVantagem).SemiBold().FontColor(VerdeVantagem);
            else
                coluna.Item().Text(simulacao.MensagemVantagem).SemiBold().FontColor(AzulPrimario);
            coluna.Item().Table(tabela =>
            {
                tabela.ColumnsDefinition(colunas => { colunas.RelativeColumn(2); colunas.RelativeColumn(); colunas.RelativeColumn(); colunas.RelativeColumn(); });
                CabecalhoTabela(tabela.Cell(), "Modalidade"); CabecalhoTabela(tabela.Cell(), "Base de cálculo"); CabecalhoTabela(tabela.Cell(), "Redução mensal"); CabecalhoTabela(tabela.Cell(), "IRRF final");
                LinhaIrrf(tabela, simulacao.Normal, EhMaisVantajosa(simulacao.Normal, modalidadeMaisVantajosa));
                LinhaIrrf(tabela, simulacao.Simplificada, EhMaisVantajosa(simulacao.Simplificada, modalidadeMaisVantajosa));
            });
        }));
    }

    private static void LinhaIrrf(TableDescriptor tabela, ModalidadeIrrfDto modalidade, bool ehMaisVantajosa)
    {
        var nomeModalidade = ehMaisVantajosa ? $"{modalidade.Nome} - MAIS VANTAJOSO" : modalidade.Nome;
        CelulaTabela(tabela.Cell(), nomeModalidade, ehMaisVantajosa);
        CelulaTabela(tabela.Cell(), Moeda(modalidade.BaseCalculo), ehMaisVantajosa);
        CelulaTabela(tabela.Cell(), Moeda(modalidade.ReducaoMensal), ehMaisVantajosa);
        CelulaTabela(tabela.Cell(), Moeda(modalidade.Imposto), ehMaisVantajosa);
    }

    private static void CriarMemoriaIrrf(IContainer container, SimulacaoImpostoDto simulacao, ModalidadeIrrfDto modalidade)
    {
        var ehNormal = modalidade.Nome.Equals("Normal", StringComparison.OrdinalIgnoreCase);
        var deducaoDaBase = ehNormal
            ? simulacao.Entrada.ValorBruto - simulacao.ValorInss - modalidade.BaseCalculo
            : simulacao.Entrada.ValorBruto - modalidade.BaseCalculo;
        var formulaBase = ehNormal
            ? $"{Moeda(simulacao.Entrada.ValorBruto)} - {Moeda(simulacao.ValorInss)} - {Moeda(deducaoDaBase)} = {Moeda(modalidade.BaseCalculo)}"
            : $"{Moeda(simulacao.Entrada.ValorBruto)} - {Moeda(deducaoDaBase)} = {Moeda(modalidade.BaseCalculo)}";
        var tituloBase = ehNormal ? "Valor bruto - INSS - dedução por dependentes" : "Valor bruto - desconto simplificado";

        CriarSecao(container, $"Memória de cálculo do IRRF - {modalidade.Nome}", secao => secao.Border(1).BorderColor(CinzaBorda).Column(cartao =>
        {
            cartao.Item().Background(AzulPrimario).Padding(6).Text("Cálculo consolidado").FontColor(Colors.White).SemiBold();
            cartao.Item().Padding(8).Column(memoria =>
            {
                memoria.Spacing(3);
                AdicionarFormulaIrrf(memoria, tituloBase, formulaBase);
                AdicionarFormulaIrrf(memoria, "IR progressivo", $"{Moeda(modalidade.BaseCalculo)} x {Percentual(modalidade.Aliquota)} - {Moeda(modalidade.Deducao)} = {Moeda(modalidade.ImpostoAntesReducao)}");
                AdicionarFormulaIrrf(memoria, "IRRF após redução mensal", $"{Moeda(modalidade.ImpostoAntesReducao)} - {Moeda(modalidade.ReducaoMensal)} = {Moeda(modalidade.Imposto)}");
            });
        }), $"IRRF final: {Moeda(modalidade.Imposto)}");
    }

    private static void AdicionarFormulaIrrf(ColumnDescriptor coluna, string titulo, string formula)
    {
        coluna.Item().Text(titulo).FontSize(8).SemiBold().FontColor(AzulPrimario);
        coluna.Item().Text(formula).FontFamily("Courier New").FontSize(8);
    }

    private static void CriarDetalhesFaixas(IContainer container, string titulo, IReadOnlyList<DetalheFaixaDto> detalhes)
    {
        CriarSecao(container, titulo, secao => secao.Table(tabela =>
        {
            tabela.ColumnsDefinition(colunas => { colunas.ConstantColumn(52); colunas.RelativeColumn(); colunas.RelativeColumn(); colunas.RelativeColumn(); });
            CabecalhoTabela(tabela.Cell(), "Faixa"); CabecalhoTabela(tabela.Cell(), "Base calculada"); CabecalhoTabela(tabela.Cell(), "Alíquota"); CabecalhoTabela(tabela.Cell(), "Imposto");
            foreach (var detalhe in detalhes)
            {
                CelulaTabela(tabela.Cell(), detalhe.Faixa.ToString(CulturaPtBr));
                CelulaTabela(tabela.Cell(), Moeda(detalhe.BaseCalculada));
                CelulaTabela(tabela.Cell(), Percentual(detalhe.Aliquota));
                CelulaTabela(tabela.Cell(), Moeda(detalhe.Imposto));
            }
        }), $"Total de impostos: {Moeda(detalhes.Sum(detalhe => detalhe.Imposto))}");
    }

    private static void CriarResumoPensao(IContainer container, SimulacaoPensaoDto simulacao, EntradaPensaoDto entrada)
    {
        CriarSecao(container, "Dados considerados", secao => secao.Table(tabela =>
        {
            tabela.ColumnsDefinition(colunas =>
            {
                colunas.RelativeColumn(14); colunas.RelativeColumn(8);
                colunas.RelativeColumn(14); colunas.RelativeColumn(8);
                colunas.RelativeColumn(14); colunas.RelativeColumn(8);
            });
            CelulaRotulo(tabela.Cell(), "Rendimentos"); CelulaValor(tabela.Cell(), Moeda(entrada.ValorBruto));
            CelulaRotulo(tabela.Cell(), "Base de INSS"); CelulaValor(tabela.Cell(), Moeda(entrada.BaseInss));
            CelulaRotulo(tabela.Cell(), "INSS calculado"); CelulaValor(tabela.Cell(), Moeda(simulacao.ValorInss));
            CelulaRotulo(tabela.Cell(), "Percentual de pensão"); CelulaValor(tabela.Cell(), Percentual(entrada.Percentual));
            CelulaRotulo(tabela.Cell(), "Outros descontos"); CelulaValor(tabela.Cell(), Moeda(entrada.OutrosDescontos));
            CelulaRotulo(tabela.Cell(), "Dependentes"); CelulaValor(tabela.Cell(), entrada.Dependentes.ToString(CulturaPtBr));
        }));
    }

    private static void CriarComparativoPensao(IContainer container, SimulacaoPensaoDto simulacao)
    {
        CriarSecao(container, "Comparativo dos modelos", secao => secao.Column(coluna =>
        {
            coluna.Spacing(8);
            coluna.Item().Text(simulacao.MensagemVantagem).SemiBold().FontColor(AzulPrimario);
            coluna.Item().Table(tabela =>
            {
                tabela.ColumnsDefinition(colunas => { colunas.RelativeColumn(2); colunas.RelativeColumn(); colunas.RelativeColumn(); colunas.RelativeColumn(); });
                CabecalhoTabela(tabela.Cell(), "Modelo"); CabecalhoTabela(tabela.Cell(), "IRRF final"); CabecalhoTabela(tabela.Cell(), "Pensão"); CabecalhoTabela(tabela.Cell(), "Total");
                LinhaPensao(tabela, simulacao.Normal);
                LinhaPensao(tabela, simulacao.Simplificada);
            });
        }));
    }

    private static void LinhaPensao(TableDescriptor tabela, ModalidadePensaoDto modalidade)
    {
        CelulaTabela(tabela.Cell(), modalidade.Nome);
        CelulaTabela(tabela.Cell(), Moeda(modalidade.Imposto));
        CelulaTabela(tabela.Cell(), Moeda(modalidade.Pensao));
        CelulaTabela(tabela.Cell(), Moeda(modalidade.Total));
    }

    private static void CriarDetalhesPensao(IContainer container, ModalidadePensaoDto modalidade, EntradaPensaoDto entrada, decimal valorInss)
    {
        var rendimentosTributaveis = entrada.ValorBruto - entrada.OutrosDescontos;
        var baseIrrfInicial = modalidade.Detalhes.Count == 0 ? 0m : modalidade.Detalhes[0].BaseIrrf;

        CriarSecao(container, $"{modalidade.Nome} - iterações do cálculo", secao => secao.Column(coluna =>
        {
            coluna.Spacing(10);
            coluna.Item().Text($"Rendimentos tributáveis: {Moeda(rendimentosTributaveis)} | INSS: {Moeda(valorInss)} | Percentual da pensão: {Percentual(entrada.Percentual)}").FontSize(8).FontColor(CinzaTexto);

            for (var indice = 0; indice < modalidade.Detalhes.Count; indice++)
            {
                var detalhe = modalidade.Detalhes[indice];
                var pensaoAnterior = indice == 0 ? 0m : modalidade.Detalhes[indice - 1].Pensao;
                var origemBaseIrrf = indice == 0 ? rendimentosTributaveis : baseIrrfInicial;
                var deducaoBaseIrrf = indice == 0 ? rendimentosTributaveis - baseIrrfInicial : pensaoAnterior;
                var descricaoBaseIrrf = indice == 0 ? "Rendimentos tributáveis - deduções do modelo" : "Base inicial de IR - pensão anterior";

                coluna.Item().Border(1).BorderColor(CinzaBorda).Column(cartao =>
                {
                    cartao.Item().Background(AzulPrimario).Padding(6).Text($"Iteração {detalhe.Sequencia}").FontColor(Colors.White).SemiBold();
                    cartao.Item().Padding(8).Column(memoria =>
                    {
                        memoria.Spacing(3);
                        AdicionarFormulaPensao(memoria, descricaoBaseIrrf, $"{Moeda(origemBaseIrrf)} - {Moeda(deducaoBaseIrrf)} = {Moeda(detalhe.BaseIrrf)}");
                        AdicionarFormulaPensao(memoria, "IR progressivo", $"{Moeda(detalhe.BaseIrrf)} x {Percentual(detalhe.Aliquota)} - {Moeda(detalhe.Deducao)} = {Moeda(detalhe.ImpostoAntesReducao)}");
                        AdicionarFormulaPensao(memoria, "IRRF após redução mensal", $"{Moeda(detalhe.ImpostoAntesReducao)} - {Moeda(detalhe.ReducaoMensal)} = {Moeda(detalhe.Imposto)}");
                        AdicionarFormulaPensao(memoria, "Base da pensão", $"{Moeda(rendimentosTributaveis)} - {Moeda(valorInss)} - {Moeda(detalhe.Imposto)} = {Moeda(detalhe.BasePensao)}");
                        AdicionarFormulaPensao(memoria, "Pensão calculada", $"{Moeda(detalhe.BasePensao)} x {Percentual(entrada.Percentual)} = {Moeda(detalhe.Pensao)}");
                    });
                });
            }
        }), $"Iterações: {modalidade.Iteracoes} | Total: {Moeda(modalidade.Total)}");
    }

    private static void AdicionarFormulaPensao(ColumnDescriptor coluna, string titulo, string formula)
    {
        coluna.Item().Text(titulo).FontSize(8).SemiBold().FontColor(AzulPrimario);
        coluna.Item().Text(formula).FontFamily("Courier New").FontSize(8);
    }

    private static void CriarSecao(IContainer container, string titulo, Action<IContainer> criarConteudo, string? destaque = null) => container.Column(coluna =>
    {
        coluna.Spacing(8);
        coluna.Item().Row(cabecalho =>
        {
            cabecalho.RelativeItem().Text(titulo).FontSize(12).SemiBold().FontColor(AzulPrimario);
            if (!string.IsNullOrWhiteSpace(destaque))
                cabecalho.AutoItem().Background(AzulClaro).PaddingHorizontal(8).PaddingVertical(4).Text(destaque).FontSize(8).SemiBold().FontColor(AzulPrimario);
        });
        coluna.Item().LineHorizontal(1).LineColor(CinzaBorda);
        coluna.Item().Element(criarConteudo);
    });

    private static void CabecalhoTabela(IContainer container, string texto) => container.Background(AzulPrimario).Padding(6).Text(texto).FontColor(Colors.White).SemiBold();
    private static void CelulaTabela(IContainer container, string texto, bool ehMaisVantajosa = false)
    {
        var celula = container.BorderBottom(1).BorderColor(ehMaisVantajosa ? VerdeBordaVantagem : CinzaBorda).Padding(6);
        if (ehMaisVantajosa)
            celula = celula.Background(VerdeClaroVantagem);

        var textoCelula = celula.Text(texto).FontColor(ehMaisVantajosa ? VerdeVantagem : CinzaTexto);
        if (ehMaisVantajosa)
            textoCelula.SemiBold();
    }
    private static void CelulaRotulo(IContainer container, string texto) => container.Background(AzulClaro).Padding(6).Text(texto).SemiBold();
    private static void CelulaValor(IContainer container, string texto) => container.BorderBottom(1).BorderColor(CinzaBorda).Padding(6).Text(texto);
    private static string Moeda(decimal valor) => valor.ToString("C2", CulturaPtBr);
    private static string Percentual(decimal valor) => valor.ToString("N2", CulturaPtBr) + "%";
    private static bool EhMaisVantajosa(ModalidadeIrrfDto modalidade, string? modalidadeMaisVantajosa) =>
        modalidade.Nome.Equals(modalidadeMaisVantajosa, StringComparison.OrdinalIgnoreCase);
}
