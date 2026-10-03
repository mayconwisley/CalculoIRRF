namespace CalculoIRRF.Application.Management;

/// <summary>Atualização pela internet de cada tabela que tem fonte online, usada pela janela de manutenção.</summary>
public interface IAtualizadorTabelas
{
    bool TemAtualizacaoOnline(TipoTabelaTributaria tipo);

    /// <summary>Página oficial consultada na atualização da tabela.</summary>
    Uri FonteOficial(TipoTabelaTributaria tipo);

    /// <summary>Órgão responsável pela página oficial, como "INSS" ou "Receita Federal".</summary>
    string NomeFonteOficial(TipoTabelaTributaria tipo);

    Task<AtualizacaoTabelaResultado> AtualizarAsync(TipoTabelaTributaria tipo, CancellationToken cancellationToken);
}
