namespace CalculoIRRF.Application.Management;

/// <summary>
/// Resultado de uma atualização pela internet. A tabela gravada vem da fonte oficial ou, enquanto ela não publica
/// a competência mais recente, de fontes alternativas que apresentam os mesmos valores.
/// </summary>
/// <param name="Fontes">Nomes das fontes que publicaram os valores gravados.</param>
/// <param name="Oficial">Indica se os valores gravados vieram da fonte oficial.</param>
/// <param name="Observacoes">Avisos para o usuário, como uma competência mais nova ainda sem confirmação.</param>
public sealed record AtualizacaoTabelaResultado(
    DateOnly Competencia,
    int QuantidadeFaixas,
    IReadOnlyList<string> Fontes,
    bool Oficial,
    IReadOnlyList<string> Observacoes);
