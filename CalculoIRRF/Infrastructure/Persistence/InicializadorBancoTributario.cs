using Microsoft.EntityFrameworkCore;

namespace CalculoIRRF.Infrastructure.Persistence;

/// <summary>Aplica alterações incrementais ao banco SQLite distribuído com a aplicação.</summary>
public sealed class InicializadorBancoTributario(CalculoIrrfDbContext context) : IInicializadorBancoTributario
{
    public async Task InicializarAsync(CancellationToken cancellationToken)
    {
        await context.Database.ExecuteSqlRawAsync("""
            CREATE TABLE IF NOT EXISTS "ReducaoMensalIrrf" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_ReducaoMensalIrrf" PRIMARY KEY AUTOINCREMENT,
                "Competencia" TEXT NOT NULL,
                "Faixa" INTEGER NOT NULL,
                "LimiteRendimentos" REAL NOT NULL,
                "Multiplicador" REAL NOT NULL,
                "ValorBase" REAL NOT NULL
            );
            """, cancellationToken);

        await context.Database.ExecuteSqlRawAsync("""
            INSERT INTO "ReducaoMensalIrrf" ("Competencia", "Faixa", "LimiteRendimentos", "Multiplicador", "ValorBase")
            SELECT '2026-01-01 00:00:00', 1, 5000.00, 0.000000, 312.89
            WHERE NOT EXISTS (SELECT 1 FROM "ReducaoMensalIrrf" WHERE "Competencia" = '2026-01-01 00:00:00' AND "Faixa" = 1);

            INSERT INTO "ReducaoMensalIrrf" ("Competencia", "Faixa", "LimiteRendimentos", "Multiplicador", "ValorBase")
            SELECT '2026-01-01 00:00:00', 2, 7350.00, 0.133145, 978.62
            WHERE NOT EXISTS (SELECT 1 FROM "ReducaoMensalIrrf" WHERE "Competencia" = '2026-01-01 00:00:00' AND "Faixa" = 2);
            """, cancellationToken);
    }
}
