using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CalculoIRRF.DataBase;

public class CalculoImpostoContextFactory : IDesignTimeDbContextFactory<CalculoImpostoContext>
{
    public CalculoImpostoContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CalculoImpostoContext>();
        optionsBuilder.UseSqlite("Data Source = BancoDados/calculoIrrf.db");
        return new CalculoImpostoContext(optionsBuilder.Options);
    }
}
