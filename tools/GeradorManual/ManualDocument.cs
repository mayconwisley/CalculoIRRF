using System.Globalization;
using System.Text;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using MdTable = Markdig.Extensions.Tables.Table;
using MdTableCell = Markdig.Extensions.Tables.TableCell;
using MdTableRow = Markdig.Extensions.Tables.TableRow;

namespace GeradorManual;

/// <summary>
/// Converte o subconjunto de Markdown usado em docs/MANUAL.md em PDF: capa, sumário com páginas, uma seção por página
/// inicial e rodapé numerado. O Markdown continua sendo a fonte única do conteúdo; o PDF é sempre gerado a partir dele.
/// </summary>
public sealed class ManualDocument
{
    private const string Azul = "#1D4ED8";
    private const string AzulClaro = "#EFF6FF";
    private const string Texto = "#1F2937";
    private const string TextoSecundario = "#6B7280";
    private const string Borda = "#D1D5DB";
    private const string FundoNota = "#F1F5F9";
    private const string FundoAlternado = "#F9FAFB";
    private const string Fonte = "Segoe UI";
    private const string FonteCodigo = "Consolas";
    private static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("pt-BR");

    private readonly string _pastaManual;
    private readonly string? _logo;
    private readonly string _titulo = "Manual do Usuário";
    private readonly string _subtitulo = string.Empty;
    private readonly ParagraphBlock? _descricao;
    private readonly QuoteBlock? _aviso;
    private readonly List<Secao> _secoes = [];

    public ManualDocument(string caminhoManual)
    {
        _pastaManual = Path.GetDirectoryName(caminhoManual)!;
        var logo = Path.GetFullPath(Path.Combine(_pastaManual, "..", "CalculoIRRF", "Assets", "logo-light.png"));
        _logo = File.Exists(logo) ? logo : null;

        var markdown = Markdown.Parse(File.ReadAllText(caminhoManual), new MarkdownPipelineBuilder().UsePipeTables().Build());
        Secao? atual = null;
        var antesDasSecoes = true;
        var noSumario = false;
        foreach (var bloco in markdown)
        {
            switch (bloco)
            {
                case HeadingBlock { Level: 1 } h1:
                    var partes = TextoSimples(h1.Inline).Split(" — ", 2);
                    _titulo = partes[0];
                    _subtitulo = partes.Length > 1 ? partes[1] : string.Empty;
                    break;
                case HeadingBlock { Level: 2 } h2:
                    antesDasSecoes = false;
                    var nome = TextoSimples(h2.Inline);
                    // O sumário do Markdown é substituído pelo do PDF, que mostra o número da página de cada seção.
                    noSumario = nome == "Sumário";
                    if (!noSumario)
                        _secoes.Add(atual = new Secao(nome, Ancora(nome), []));
                    break;
                case ParagraphBlock paragrafo when antesDasSecoes:
                    _descricao ??= paragrafo;
                    break;
                case QuoteBlock nota when antesDasSecoes:
                    _aviso ??= nota;
                    break;
                default:
                    if (!antesDasSecoes && !noSumario)
                        atual?.Blocos.Add(bloco);
                    break;
            }
        }
    }

    public void GerarPdf(string caminho) => CriarDocumento().GeneratePdf(caminho);

    public int GerarPrevia(string pasta)
    {
        var paginas = CriarDocumento().GenerateImages(new ImageGenerationSettings { ImageFormat = ImageFormat.Png, RasterDpi = 100 }).ToList();
        for (var indice = 0; indice < paginas.Count; indice++)
            File.WriteAllBytes(Path.Combine(pasta, $"pagina-{indice + 1:00}.png"), paginas[indice]);
        return paginas.Count;
    }

    private Document CriarDocumento() => Document.Create(Compor).WithMetadata(new DocumentMetadata
    {
        Title = $"{_titulo} — {_subtitulo}",
        Author = _subtitulo,
        Subject = "Manual do usuário final"
    });

    private void Compor(IDocumentContainer documento)
    {
        documento.Page(pagina =>
        {
            ConfigurarPagina(pagina);
            pagina.Content().Element(Capa);
        });

        documento.Page(pagina =>
        {
            ConfigurarPagina(pagina);
            pagina.Header().PaddingBottom(14).BorderBottom(0.75f).BorderColor(Borda).PaddingBottom(6).Row(cabecalho =>
            {
                cabecalho.RelativeItem().Text(_subtitulo).FontSize(8.5f).FontColor(TextoSecundario);
                cabecalho.AutoItem().Text(_titulo).FontSize(8.5f).FontColor(TextoSecundario);
            });
            pagina.Footer().PaddingTop(10).AlignCenter().Text(rodape =>
            {
                rodape.DefaultTextStyle(estilo => estilo.FontSize(8.5f).FontColor(TextoSecundario));
                rodape.Span("Página ");
                rodape.CurrentPageNumber();
                rodape.Span(" de ");
                rodape.TotalPages();
            });
            pagina.Content().Column(coluna =>
            {
                coluna.Spacing(8);
                coluna.Item().Element(Sumario);
                coluna.Item().PageBreak();
                for (var indice = 0; indice < _secoes.Count; indice++)
                    AdicionarSecao(coluna, _secoes[indice], indice == 0);
            });
        });
    }

    private void AdicionarSecao(ColumnDescriptor coluna, Secao secao, bool primeira)
    {
        var unidades = new List<(bool EhTitulo, Block? Bloco, Action<IContainer> Desenhar)> { (true, null, container => TituloSecao(container, secao.Titulo)) };
        unidades.AddRange(secao.Blocos.Select(bloco => (bloco is HeadingBlock, (Block?)bloco, (Action<IContainer>)(container => Bloco(container, bloco)))));

        for (var inicio = 0; inicio < unidades.Count;)
        {
            // Um título nunca fica sozinho no pé da página: segue junto com os títulos seguintes e o primeiro conteúdo.
            var fim = inicio;
            while (unidades[fim].EhTitulo && fim < unidades.Count - 1)
                fim++;
            var grupo = unidades.GetRange(inicio, fim - inicio + 1);
            var item = inicio == 0 ? coluna.Item().Section(secao.Ancora).PaddingTop(primeira ? 0 : 22) : coluna.Item();

            if (grupo.Count == 1)
                item.Element(grupo[0].Desenhar);
            else
            {
                // Parágrafos, imagens e notas vão inteiros com o título; tabelas e listas podem continuar na página seguinte.
                var conteudoCurto = grupo[^1].Bloco is ParagraphBlock or QuoteBlock or HeadingBlock;
                (conteudoCurto ? item.ShowEntire() : item.EnsureSpace(180)).Column(juntos =>
                {
                    juntos.Spacing(8);
                    foreach (var unidade in grupo)
                        juntos.Item().Element(unidade.Desenhar);
                });
            }
            inicio = fim + 1;
        }
    }

    private static void ConfigurarPagina(PageDescriptor pagina)
    {
        pagina.Size(PageSizes.A4);
        pagina.MarginHorizontal(52);
        pagina.MarginVertical(42);
        pagina.PageColor(Colors.White);
        pagina.DefaultTextStyle(estilo => estilo.FontFamily(Fonte).FontSize(10.5f).FontColor(Texto).LineHeight(1.35f));
    }

    private void Capa(IContainer container) => container.Column(coluna =>
    {
        coluna.Item().Height(90);
        if (_logo is not null)
            coluna.Item().AlignCenter().Width(120).Image(_logo);
        coluna.Item().PaddingTop(26).AlignCenter().Text(_titulo).FontSize(30).Bold().FontColor(Azul);
        coluna.Item().PaddingTop(2).AlignCenter().Text(_subtitulo).FontSize(16).FontColor(TextoSecundario);
        if (_descricao is not null)
            coluna.Item().PaddingTop(34).PaddingHorizontal(36).Text(texto =>
            {
                texto.AlignCenter();
                Inlines(texto, _descricao.Inline);
            });
        if (_aviso is not null)
            coluna.Item().PaddingTop(28).PaddingHorizontal(24).Element(c => Nota(c, _aviso));
        var edicao = DateTime.Today.ToString("MMMM 'de' yyyy", Cultura);
        coluna.Item().PaddingTop(90).AlignCenter().Text($"Edição de {char.ToUpper(edicao[0], Cultura)}{edicao[1..]}").FontSize(10).FontColor(TextoSecundario);
    });

    private void Sumario(IContainer container) => container.Column(coluna =>
    {
        coluna.Item().PaddingBottom(12).Text("Sumário").FontSize(22).Bold().FontColor(Azul);
        foreach (var secao in _secoes)
            coluna.Item().SectionLink(secao.Ancora).BorderBottom(0.5f).BorderColor(Borda).PaddingVertical(6).Row(linha =>
            {
                linha.RelativeItem().Text(secao.Titulo).FontSize(11.5f);
                linha.AutoItem().Text(texto => texto.BeginPageNumberOfSection(secao.Ancora).FontSize(11.5f).FontColor(Azul).Bold());
            });
    });

    private static void TituloSecao(IContainer container, string titulo) => container.PaddingBottom(4).Column(coluna =>
    {
        coluna.Item().Text(titulo).FontSize(20).Bold().FontColor(Azul);
        coluna.Item().PaddingTop(4).LineHorizontal(1.5f).LineColor(Azul);
    });

    private void Bloco(IContainer container, Block bloco)
    {
        switch (bloco)
        {
            case HeadingBlock titulo:
                container.PaddingTop(8).Text(TextoSimples(titulo.Inline)).FontSize(titulo.Level == 3 ? 13.5f : 12f).Bold();
                break;
            case ParagraphBlock paragrafo when ImagemUnica(paragrafo) is { } imagem:
                Imagem(container, imagem);
                break;
            case ParagraphBlock paragrafo:
                container.Text(texto => Inlines(texto, paragrafo.Inline));
                break;
            case ListBlock lista:
                Lista(container, lista);
                break;
            case MdTable tabela:
                Tabela(container, tabela);
                break;
            case QuoteBlock nota:
                Nota(container, nota);
                break;
            case ThematicBreakBlock:
                container.PaddingVertical(6).LineHorizontal(0.75f).LineColor(Borda);
                break;
            case CodeBlock codigo:
                container.Background(FundoNota).Padding(8).Text(codigo.Lines.ToString()).FontFamily(FonteCodigo).FontSize(9);
                break;
        }
    }

    private void Inlines(TextDescriptor texto, ContainerInline? inlines, bool negrito = false, bool italico = false)
    {
        if (inlines is null)
            return;

        foreach (var inline in inlines)
        {
            switch (inline)
            {
                case LiteralInline literal:
                    Estilizar(texto.Span(literal.Content.ToString()), negrito, italico);
                    break;
                case EmphasisInline enfase:
                    Inlines(texto, enfase, negrito || enfase.DelimiterCount >= 2, italico || enfase.DelimiterCount == 1);
                    break;
                case CodeInline codigo:
                    texto.Span(codigo.Content).FontFamily(FonteCodigo).FontSize(9.5f).BackgroundColor(FundoNota);
                    break;
                case LinkInline { IsImage: false } link:
                    var rotulo = TextoSimples(link);
                    var url = link.Url ?? string.Empty;
                    var span = url.StartsWith('#') ? texto.SectionLink(rotulo, url[1..]) : texto.Hyperlink(rotulo, url);
                    Estilizar(span.FontColor(Azul).Underline(), negrito, italico);
                    break;
                case LineBreakInline quebra:
                    texto.Span(quebra.IsHard ? "\n" : " ");
                    break;
                case ContainerInline outro:
                    Inlines(texto, outro, negrito, italico);
                    break;
            }
        }
    }

    private static void Estilizar(TextSpanDescriptor span, bool negrito, bool italico)
    {
        if (negrito) span.Bold();
        if (italico) span.Italic();
    }

    private void Lista(IContainer container, ListBlock lista) => container.PaddingLeft(4).Column(coluna =>
    {
        coluna.Spacing(3);
        var numero = int.TryParse(lista.OrderedStart, out var inicio) ? inicio : 1;
        foreach (var item in lista.OfType<ListItemBlock>())
        {
            var marcador = lista.IsOrdered ? $"{numero++}." : "•";
            coluna.Item().Row(linha =>
            {
                linha.ConstantItem(lista.IsOrdered ? 18 : 12).Text(marcador).Bold().FontColor(lista.IsOrdered ? Texto : Azul);
                linha.RelativeItem().Column(conteudo =>
                {
                    conteudo.Spacing(3);
                    foreach (var bloco in item)
                        conteudo.Item().Element(c => Bloco(c, bloco));
                });
            });
        }
    });

    private void Imagem(IContainer container, LinkInline imagem)
    {
        var caminho = Path.GetFullPath(Path.Combine(_pastaManual, imagem.Url ?? string.Empty));
        var legenda = TextoSimples(imagem);
        // Prints pequenos (como janelas de aviso) mantêm o tamanho natural em vez de serem ampliados até a largura da página.
        var larguraNatural = LarguraPng(caminho) * 0.75f;
        container.ShowEntire().PaddingVertical(4).AlignCenter().MaxWidth(Math.Max(larguraNatural, 120)).Column(coluna =>
        {
            coluna.Item().Border(0.75f).BorderColor(Borda).Image(caminho).FitWidth();
            if (legenda.Length > 0)
                coluna.Item().PaddingTop(4).AlignCenter().Text(legenda).FontSize(8.5f).Italic().FontColor(TextoSecundario);
        });
    }

    private void Tabela(IContainer container, MdTable tabela)
    {
        var linhas = tabela.OfType<MdTableRow>().ToList();
        var colunas = linhas.Max(linha => linha.Count);
        // Largura proporcional ao texto mais longo de cada coluna; colunas curtas (como "Nº") têm largura fixa para não quebrar o título.
        var pesos = Enumerable.Range(0, colunas)
            .Select(indice => Math.Min(linhas.Max(linha => indice < linha.Count ? TextoSimples((ContainerBlock)linha[indice]).Length : 0), 60))
            .ToArray();

        container.PaddingVertical(4).DefaultTextStyle(estilo => estilo.FontSize(9.5f)).Table(grade =>
        {
            grade.ColumnsDefinition(definicao =>
            {
                foreach (var peso in pesos)
                {
                    if (peso <= 3)
                        definicao.ConstantColumn(32);
                    else
                        definicao.RelativeColumn(peso);
                }
            });

            if (linhas.FirstOrDefault(linha => linha.IsHeader) is { } cabecalho)
                grade.Header(titulos =>
                {
                    foreach (var celula in cabecalho.OfType<MdTableCell>())
                        titulos.Cell().Background(AzulClaro).BorderBottom(1).BorderColor(Azul).Padding(5)
                            .DefaultTextStyle(estilo => estilo.Bold()).Element(c => ConteudoCelula(c, celula));
                });

            var indiceLinha = 0;
            foreach (var linha in linhas.Where(linha => !linha.IsHeader))
            {
                var fundo = indiceLinha++ % 2 == 1 ? FundoAlternado : "#FFFFFF";
                foreach (var celula in linha.OfType<MdTableCell>())
                    grade.Cell().Background(fundo).BorderBottom(0.5f).BorderColor(Borda).Padding(5).Element(c => ConteudoCelula(c, celula));
            }
        });
    }

    private void ConteudoCelula(IContainer container, MdTableCell celula) => container.Column(coluna =>
    {
        foreach (var bloco in celula)
            coluna.Item().Element(c => Bloco(c, bloco));
    });

    private void Nota(IContainer container, QuoteBlock nota) => container.PaddingVertical(4)
        .BorderLeft(3).BorderColor(Azul).Background(FundoNota).PaddingVertical(8).PaddingHorizontal(10)
        .Column(coluna =>
        {
            coluna.Spacing(4);
            foreach (var bloco in nota)
                coluna.Item().Element(c => Bloco(c, bloco));
        });

    private static LinkInline? ImagemUnica(ParagraphBlock paragrafo)
    {
        var itens = paragrafo.Inline?
            .Where(inline => inline is not LineBreakInline && !(inline is LiteralInline literal && literal.Content.IsEmptyOrWhitespace()))
            .ToList();
        return itens is [LinkInline { IsImage: true } imagem] ? imagem : null;
    }

    private static string TextoSimples(ContainerInline? inlines)
    {
        var texto = new StringBuilder();
        Acumular(texto, inlines);
        return texto.ToString().Trim();
    }

    private static string TextoSimples(ContainerBlock bloco) =>
        string.Join(" ", bloco.Descendants<ParagraphBlock>().Select(paragrafo => TextoSimples(paragrafo.Inline)));

    private static void Acumular(StringBuilder texto, ContainerInline? inlines)
    {
        if (inlines is null)
            return;
        foreach (var inline in inlines)
        {
            switch (inline)
            {
                case LiteralInline literal: texto.Append(literal.Content); break;
                case CodeInline codigo: texto.Append(codigo.Content); break;
                case LineBreakInline: texto.Append(' '); break;
                case ContainerInline filho: Acumular(texto, filho); break;
            }
        }
    }

    /// <summary>Mesmo formato das âncoras que o GitHub gera para os títulos, para os links do sumário valerem nos dois formatos.</summary>
    private static string Ancora(string titulo)
    {
        var ancora = new StringBuilder();
        foreach (var caractere in titulo.ToLowerInvariant())
        {
            if (char.IsLetterOrDigit(caractere) || caractere is '-' or '_')
                ancora.Append(caractere);
            else if (caractere == ' ')
                ancora.Append('-');
        }
        return ancora.ToString();
    }

    private static float LarguraPng(string caminho)
    {
        Span<byte> cabecalho = stackalloc byte[24];
        using var arquivo = File.OpenRead(caminho);
        return arquivo.Read(cabecalho) == 24 ? (cabecalho[16] << 24) | (cabecalho[17] << 16) | (cabecalho[18] << 8) | cabecalho[19] : 0;
    }

    private sealed record Secao(string Titulo, string Ancora, List<Block> Blocos);
}
