using Microsoft.Data.Sqlite;

namespace CalculoIRRF.Infrastructure.Persistence;

/// <summary>
/// Popula de forma idempotente as tabelas históricas do INSS e do IRRF.
/// Os registros existentes são preservados para não sobrescrever manutenções locais.
/// A carga só é executada quando o banco ainda não está na <see cref="VersaoSementes"/> atual.
/// </summary>
public sealed class InicializadorBancoTributario(BancoTributario banco) : IInicializadorBancoTributario
{
    /// <summary>Incremente ao alterar as sementes ou os scripts abaixo, para que bancos existentes recebam a nova carga.</summary>
    private const int VersaoSementes = 3;
    private const double LimiteUltimaFaixaIrrf = 9_999_999_999_999.99d;

    private static readonly SementeFaixa[] FaixasInss =
    [
        new(2017, 1, 1, 1, 1_659.38d, 8d), new(2017, 1, 1, 2, 2_765.66d, 9d), new(2017, 1, 1, 3, 5_531.31d, 11d),
        new(2018, 1, 1, 1, 1_693.72d, 8d), new(2018, 1, 1, 2, 2_822.90d, 9d), new(2018, 1, 1, 3, 5_645.80d, 11d),
        new(2019, 1, 1, 1, 1_751.81d, 8d), new(2019, 1, 1, 2, 2_919.72d, 9d), new(2019, 1, 1, 3, 5_839.45d, 11d),
        new(2020, 1, 1, 1, 1_830.29d, 8d), new(2020, 1, 1, 2, 3_050.52d, 9d), new(2020, 1, 1, 3, 6_101.06d, 11d),
        new(2020, 3, 1, 1, 1_045.00d, 7.5d), new(2020, 3, 1, 2, 2_089.60d, 9d), new(2020, 3, 1, 3, 3_134.40d, 12d), new(2020, 3, 1, 4, 6_101.06d, 14d),
        new(2021, 1, 1, 1, 1_100.00d, 7.5d), new(2021, 1, 1, 2, 2_203.48d, 9d), new(2021, 1, 1, 3, 3_305.22d, 12d), new(2021, 1, 1, 4, 6_433.57d, 14d),
        new(2022, 1, 1, 1, 1_212.00d, 7.5d), new(2022, 1, 1, 2, 2_427.35d, 9d), new(2022, 1, 1, 3, 3_641.03d, 12d), new(2022, 1, 1, 4, 7_087.22d, 14d),
        new(2023, 1, 1, 1, 1_302.00d, 7.5d), new(2023, 1, 1, 2, 2_571.29d, 9d), new(2023, 1, 1, 3, 3_856.94d, 12d), new(2023, 1, 1, 4, 7_507.49d, 14d),
        new(2023, 5, 1, 1, 1_320.00d, 7.5d), new(2023, 5, 1, 2, 2_571.29d, 9d), new(2023, 5, 1, 3, 3_856.94d, 12d), new(2023, 5, 1, 4, 7_507.49d, 14d),
        new(2024, 1, 1, 1, 1_412.00d, 7.5d), new(2024, 1, 1, 2, 2_666.68d, 9d), new(2024, 1, 1, 3, 4_000.03d, 12d), new(2024, 1, 1, 4, 7_786.02d, 14d),
        new(2025, 1, 1, 1, 1_518.00d, 7.5d), new(2025, 1, 1, 2, 2_793.88d, 9d), new(2025, 1, 1, 3, 4_190.83d, 12d), new(2025, 1, 1, 4, 8_157.41d, 14d),
        new(2026, 1, 1, 1, 1_621.00d, 7.5d), new(2026, 1, 1, 2, 2_902.84d, 9d), new(2026, 1, 1, 3, 4_354.27d, 12d), new(2026, 1, 1, 4, 8_475.55d, 14d)
    ];

    private static readonly SementeIrrf[] FaixasIrrf =
    [
        new(2017, 1, 1, 1, 1_903.98d, 0d, 0d), new(2017, 1, 1, 2, 2_826.65d, 7.5d, 142.80d), new(2017, 1, 1, 3, 3_751.05d, 15d, 354.80d), new(2017, 1, 1, 4, 4_664.68d, 22.5d, 636.13d), new(2017, 1, 1, 5, LimiteUltimaFaixaIrrf, 27.5d, 869.36d),
        new(2023, 5, 1, 1, 2_112.00d, 0d, 0d), new(2023, 5, 1, 2, 2_826.65d, 7.5d, 158.40d), new(2023, 5, 1, 3, 3_751.05d, 15d, 370.40d), new(2023, 5, 1, 4, 4_664.68d, 22.5d, 651.73d), new(2023, 5, 1, 5, LimiteUltimaFaixaIrrf, 27.5d, 884.96d),
        new(2024, 2, 1, 1, 2_259.20d, 0d, 0d), new(2024, 2, 1, 2, 2_826.65d, 7.5d, 169.44d), new(2024, 2, 1, 3, 3_751.05d, 15d, 381.44d), new(2024, 2, 1, 4, 4_664.68d, 22.5d, 662.77d), new(2024, 2, 1, 5, LimiteUltimaFaixaIrrf, 27.5d, 896.00d),
        new(2025, 5, 1, 1, 2_428.80d, 0d, 0d), new(2025, 5, 1, 2, 2_826.65d, 7.5d, 182.16d), new(2025, 5, 1, 3, 3_751.05d, 15d, 394.16d), new(2025, 5, 1, 4, 4_664.68d, 22.5d, 675.49d), new(2025, 5, 1, 5, LimiteUltimaFaixaIrrf, 27.5d, 908.73d),
        new(2026, 1, 1, 1, 2_428.80d, 0d, 0d), new(2026, 1, 1, 2, 2_826.65d, 7.5d, 182.16d), new(2026, 1, 1, 3, 3_751.05d, 15d, 394.16d), new(2026, 1, 1, 4, 4_664.68d, 22.5d, 675.49d), new(2026, 1, 1, 5, LimiteUltimaFaixaIrrf, 27.5d, 908.73d)
    ];

    private static readonly SementeParametro[] DeducoesPorDependente = [new(2017, 1, 1, 189.59d)];
    private static readonly SementeParametro[] DescontosSimplificados = [new(2017, 1, 1, 0d), new(2023, 5, 1, 528.00d), new(2024, 2, 1, 564.80d), new(2025, 5, 1, 607.20d), new(2026, 1, 1, 607.20d)];
    private static readonly SementeParametro[] DescontosMinimos = [new(2017, 1, 1, 0d)];

    // Base do adicional de insalubridade (CLT, art. 192).
    private static readonly SementeParametro[] SalariosMinimos =
    [
        new(2017, 1, 1, 937.00d), new(2018, 1, 1, 954.00d), new(2019, 1, 1, 998.00d), new(2020, 1, 1, 1_039.00d), new(2020, 2, 1, 1_045.00d),
        new(2021, 1, 1, 1_100.00d), new(2022, 1, 1, 1_212.00d), new(2023, 1, 1, 1_302.00d), new(2023, 5, 1, 1_320.00d), new(2024, 1, 1, 1_412.00d),
        new(2025, 1, 1, 1_518.00d), new(2026, 1, 1, 1_621.00d)
    ];

    // Cota por filho conforme a remuneração do segurado. Até 10/2019 havia duas faixas; a EC 103/2019 deixou uma só.
    private static readonly SementeSalarioFamilia[] FaixasSalarioFamilia =
    [
        new(2017, 1, 1, 1, 859.88d, 44.09d), new(2017, 1, 1, 2, 1_292.43d, 31.07d),
        new(2018, 1, 1, 1, 877.67d, 45.00d), new(2018, 1, 1, 2, 1_319.18d, 31.71d),
        new(2019, 1, 1, 1, 907.77d, 46.54d), new(2019, 1, 1, 2, 1_364.43d, 32.80d),
        new(2019, 11, 1, 1, 1_364.43d, 46.54d),
        new(2020, 1, 1, 1, 1_425.56d, 48.62d), new(2021, 1, 1, 1, 1_503.25d, 51.27d), new(2022, 1, 1, 1, 1_655.98d, 56.47d),
        new(2023, 1, 1, 1, 1_754.18d, 59.82d), new(2024, 1, 1, 1, 1_819.26d, 62.04d), new(2025, 1, 1, 1, 1_906.04d, 65.00d),
        new(2026, 1, 1, 1, 1_980.38d, 67.54d)
    ];

    // Tabela anual exclusiva da PLR (Lei 10.101/2000, anexo), conforme as tabelas publicadas pela Receita Federal.
    private static readonly SementeIrrf[] FaixasPlr =
    [
        new(2017, 1, 1, 1, 6_677.55d, 0d, 0d), new(2017, 1, 1, 2, 9_922.28d, 7.5d, 500.82d), new(2017, 1, 1, 3, 13_167.00d, 15d, 1_244.99d), new(2017, 1, 1, 4, 16_380.38d, 22.5d, 2_232.51d), new(2017, 1, 1, 5, LimiteUltimaFaixaIrrf, 27.5d, 3_051.53d),
        new(2023, 5, 1, 1, 7_407.11d, 0d, 0d), new(2023, 5, 1, 2, 9_922.28d, 7.5d, 555.53d), new(2023, 5, 1, 3, 13_167.00d, 15d, 1_299.70d), new(2023, 5, 1, 4, 16_380.38d, 22.5d, 2_287.23d), new(2023, 5, 1, 5, LimiteUltimaFaixaIrrf, 27.5d, 3_106.25d),
        new(2024, 2, 1, 1, 7_640.80d, 0d, 0d), new(2024, 2, 1, 2, 9_922.28d, 7.5d, 573.06d), new(2024, 2, 1, 3, 13_167.00d, 15d, 1_317.23d), new(2024, 2, 1, 4, 16_380.38d, 22.5d, 2_304.76d), new(2024, 2, 1, 5, LimiteUltimaFaixaIrrf, 27.5d, 3_123.78d),
        new(2025, 5, 1, 1, 8_214.40d, 0d, 0d), new(2025, 5, 1, 2, 9_922.28d, 7.5d, 616.08d), new(2025, 5, 1, 3, 13_167.00d, 15d, 1_360.25d), new(2025, 5, 1, 4, 16_380.38d, 22.5d, 2_347.78d), new(2025, 5, 1, 5, LimiteUltimaFaixaIrrf, 27.5d, 3_166.80d)
    ];

    public async Task InicializarAsync(CancellationToken cancellationToken)
    {
        await using var conexao = await banco.AbrirAsync(cancellationToken);
        if (await ObterVersaoBancoAsync(conexao, cancellationToken) >= VersaoSementes)
            return;

        // Uma única transação: ou a carga inteira é gravada junto com a versão, ou nada muda.
        await using var transacao = conexao.BeginTransaction();
        await CriarTabelaReducaoMensalAsync(conexao, transacao, cancellationToken);
        await CriarTabelasDasCalculadorasAsync(conexao, transacao, cancellationToken);
        await CorrigirOuInserirReducaoMensal2026Async(conexao, transacao, cancellationToken);
        await CorrigirFaixasInss2022Async(conexao, transacao, cancellationToken);
        await InserirFaixasInssAusentesAsync(conexao, transacao, cancellationToken);
        await InserirFaixasIrrfAusentesAsync(conexao, transacao, cancellationToken);
        await InserirFaixasPlrAusentesAsync(conexao, transacao, cancellationToken);
        await InserirFaixasSalarioFamiliaAusentesAsync(conexao, transacao, cancellationToken);
        await InserirParametrosAusentesAsync(conexao, transacao, cancellationToken);
        await conexao.ExecutarAsync(transacao, $"PRAGMA user_version = {VersaoSementes}", cancellationToken);
        await transacao.CommitAsync(cancellationToken);
    }

    private static async Task<int> ObterVersaoBancoAsync(SqliteConnection conexao, CancellationToken cancellationToken)
    {
        await using var comando = conexao.CriarComando(null, "PRAGMA user_version");
        return Convert.ToInt32(await comando.ExecuteScalarAsync(cancellationToken));
    }

    private static Task CriarTabelaReducaoMensalAsync(SqliteConnection conexao, SqliteTransaction transacao, CancellationToken cancellationToken) => conexao.ExecutarAsync(transacao, """
        CREATE TABLE IF NOT EXISTS "ReducaoMensalIrrf" (
            "Id" INTEGER NOT NULL CONSTRAINT "PK_ReducaoMensalIrrf" PRIMARY KEY AUTOINCREMENT,
            "Competencia" TEXT NOT NULL,
            "Faixa" INTEGER NOT NULL,
            "LimiteRendimentos" REAL NOT NULL,
            "Multiplicador" REAL NOT NULL,
            "ValorBase" REAL NOT NULL
        );
        """, cancellationToken);

    private static Task CriarTabelasDasCalculadorasAsync(SqliteConnection conexao, SqliteTransaction transacao, CancellationToken cancellationToken) => conexao.ExecutarAsync(transacao, """
        CREATE TABLE IF NOT EXISTS "SalarioMinimo" (
            "Id" INTEGER NOT NULL CONSTRAINT "PK_SalarioMinimo" PRIMARY KEY AUTOINCREMENT,
            "Competencia" TEXT NOT NULL,
            "Valor" REAL NOT NULL
        );

        CREATE TABLE IF NOT EXISTS "SalarioFamilia" (
            "Id" INTEGER NOT NULL CONSTRAINT "PK_SalarioFamilia" PRIMARY KEY AUTOINCREMENT,
            "Competencia" TEXT NOT NULL,
            "Faixa" INTEGER NOT NULL,
            "LimiteRemuneracao" REAL NOT NULL,
            "Cota" REAL NOT NULL
        );

        CREATE TABLE IF NOT EXISTS "Plr" (
            "Id" INTEGER NOT NULL CONSTRAINT "PK_Plr" PRIMARY KEY AUTOINCREMENT,
            "Competencia" TEXT NOT NULL,
            "Faixa" INTEGER NOT NULL,
            "Valor" REAL NOT NULL,
            "Porcentagem" REAL NOT NULL,
            "Deducao" REAL NOT NULL
        );
        """, cancellationToken);

    private static Task CorrigirOuInserirReducaoMensal2026Async(SqliteConnection conexao, SqliteTransaction transacao, CancellationToken cancellationToken) => conexao.ExecutarAsync(transacao, """
        INSERT INTO "ReducaoMensalIrrf" ("Competencia", "Faixa", "LimiteRendimentos", "Multiplicador", "ValorBase")
        SELECT '2026-01-01 00:00:00', 1, 5000.00, 0.000000, 312.89
        WHERE NOT EXISTS (SELECT 1 FROM "ReducaoMensalIrrf" WHERE "Competencia" = '2026-01-01 00:00:00' AND "Faixa" = 1);

        INSERT INTO "ReducaoMensalIrrf" ("Competencia", "Faixa", "LimiteRendimentos", "Multiplicador", "ValorBase")
        SELECT '2026-01-01 00:00:00', 2, 7350.00, 0.133145, 978.62
        WHERE NOT EXISTS (SELECT 1 FROM "ReducaoMensalIrrf" WHERE "Competencia" = '2026-01-01 00:00:00' AND "Faixa" = 2);

        UPDATE "ReducaoMensalIrrf"
        SET "ValorBase" = 978.62
        WHERE "Competencia" = '2026-01-01 00:00:00'
          AND "Faixa" = 2
          AND "LimiteRendimentos" = 7350.00
          AND "Multiplicador" = 0.133145
          AND "ValorBase" = 7350.00;
        """, cancellationToken);

    /// <summary>
    /// Corrige os limites das faixas 2 e 3 de 01/2022 gravados por sementes antigas (Portaria Interministerial MTP/ME nº 12/2022).
    /// Só altera o valor quando ele ainda é exatamente o errado, preservando edições manuais.
    /// </summary>
    private static Task CorrigirFaixasInss2022Async(SqliteConnection conexao, SqliteTransaction transacao, CancellationToken cancellationToken) => conexao.ExecutarAsync(transacao, """
        UPDATE "Inss"
        SET "Valor" = 2427.35
        WHERE "Competencia" = '2022-01-01 00:00:00'
          AND "Faixa" = 2
          AND "Valor" = 2452.67;

        UPDATE "Inss"
        SET "Valor" = 3641.03
        WHERE "Competencia" = '2022-01-01 00:00:00'
          AND "Faixa" = 3
          AND "Valor" = 3679.00;
        """, cancellationToken);

    private static async Task InserirFaixasInssAusentesAsync(SqliteConnection conexao, SqliteTransaction transacao, CancellationToken cancellationToken)
    {
        var existentes = (await conexao.ListarAsync(transacao, "SELECT Competencia, Faixa FROM Inss", leitor => (leitor.GetDateTime(0), leitor.GetInt32(1)), cancellationToken)).ToHashSet();
        foreach (var item in FaixasInss.Where(item => !existentes.Contains((item.Competencia, item.Faixa))))
            await conexao.ExecutarAsync(transacao, "INSERT INTO Inss (Competencia, Faixa, Valor, Porcentagem) VALUES ($competencia, $faixa, $valor, $porcentagem)", cancellationToken,
                ("$competencia", item.Competencia), ("$faixa", item.Faixa), ("$valor", item.Limite), ("$porcentagem", item.Aliquota));
    }

    private static async Task InserirFaixasIrrfAusentesAsync(SqliteConnection conexao, SqliteTransaction transacao, CancellationToken cancellationToken)
    {
        var existentes = (await conexao.ListarAsync(transacao, "SELECT Competencia, Faixa FROM Irrf", leitor => (leitor.GetDateTime(0), leitor.GetInt32(1)), cancellationToken)).ToHashSet();
        foreach (var item in FaixasIrrf.Where(item => !existentes.Contains((item.Competencia, item.Faixa))))
            await conexao.ExecutarAsync(transacao, "INSERT INTO Irrf (Competencia, Faixa, Valor, Porcentagem, Deducao) VALUES ($competencia, $faixa, $valor, $porcentagem, $deducao)", cancellationToken,
                ("$competencia", item.Competencia), ("$faixa", item.Faixa), ("$valor", item.Limite), ("$porcentagem", item.Aliquota), ("$deducao", item.Deducao));
    }

    private static async Task InserirFaixasPlrAusentesAsync(SqliteConnection conexao, SqliteTransaction transacao, CancellationToken cancellationToken)
    {
        var existentes = (await conexao.ListarAsync(transacao, "SELECT Competencia, Faixa FROM Plr", leitor => (leitor.GetDateTime(0), leitor.GetInt32(1)), cancellationToken)).ToHashSet();
        foreach (var item in FaixasPlr.Where(item => !existentes.Contains((item.Competencia, item.Faixa))))
            await conexao.ExecutarAsync(transacao, "INSERT INTO Plr (Competencia, Faixa, Valor, Porcentagem, Deducao) VALUES ($competencia, $faixa, $valor, $porcentagem, $deducao)", cancellationToken,
                ("$competencia", item.Competencia), ("$faixa", item.Faixa), ("$valor", item.Limite), ("$porcentagem", item.Aliquota), ("$deducao", item.Deducao));
    }

    private static async Task InserirFaixasSalarioFamiliaAusentesAsync(SqliteConnection conexao, SqliteTransaction transacao, CancellationToken cancellationToken)
    {
        var existentes = (await conexao.ListarAsync(transacao, "SELECT Competencia, Faixa FROM SalarioFamilia", leitor => (leitor.GetDateTime(0), leitor.GetInt32(1)), cancellationToken)).ToHashSet();
        foreach (var item in FaixasSalarioFamilia.Where(item => !existentes.Contains((item.Competencia, item.Faixa))))
            await conexao.ExecutarAsync(transacao, "INSERT INTO SalarioFamilia (Competencia, Faixa, LimiteRemuneracao, Cota) VALUES ($competencia, $faixa, $limite, $cota)", cancellationToken,
                ("$competencia", item.Competencia), ("$faixa", item.Faixa), ("$limite", item.LimiteRemuneracao), ("$cota", item.Cota));
    }

    private static async Task InserirParametrosAusentesAsync(SqliteConnection conexao, SqliteTransaction transacao, CancellationToken cancellationToken)
    {
        await InserirParametrosAusentesAsync(conexao, transacao, "Dependente", DeducoesPorDependente, cancellationToken);
        await InserirParametrosAusentesAsync(conexao, transacao, "Simplificado", DescontosSimplificados, cancellationToken);
        await InserirParametrosAusentesAsync(conexao, transacao, "DescontoMinimo", DescontosMinimos, cancellationToken);
        await InserirParametrosAusentesAsync(conexao, transacao, "SalarioMinimo", SalariosMinimos, cancellationToken);
    }

    private static async Task InserirParametrosAusentesAsync(SqliteConnection conexao, SqliteTransaction transacao, string tabela, SementeParametro[] sementes, CancellationToken cancellationToken)
    {
        var existentes = (await conexao.ListarAsync(transacao, $"SELECT Competencia FROM {tabela}", leitor => leitor.GetDateTime(0), cancellationToken)).ToHashSet();
        foreach (var item in sementes.Where(item => !existentes.Contains(item.Competencia)))
            await conexao.ExecutarAsync(transacao, $"INSERT INTO {tabela} (Competencia, Valor) VALUES ($competencia, $valor)", cancellationToken,
                ("$competencia", item.Competencia), ("$valor", item.Valor));
    }

    private sealed record SementeFaixa(int Ano, int Mes, int Dia, int Faixa, double Limite, double Aliquota)
    {
        public DateTime Competencia => new(Ano, Mes, Dia);
    }

    private sealed record SementeIrrf(int Ano, int Mes, int Dia, int Faixa, double Limite, double Aliquota, double Deducao)
    {
        public DateTime Competencia => new(Ano, Mes, Dia);
    }

    private sealed record SementeParametro(int Ano, int Mes, int Dia, double Valor)
    {
        public DateTime Competencia => new(Ano, Mes, Dia);
    }

    private sealed record SementeSalarioFamilia(int Ano, int Mes, int Dia, int Faixa, double LimiteRemuneracao, double Cota)
    {
        public DateTime Competencia => new(Ano, Mes, Dia);
    }
}
