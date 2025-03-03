using CalculoIRRF.Model;
using Microsoft.EntityFrameworkCore;

namespace CalculoIRRF.DataBase;

public class CalculoImpostoContext(DbContextOptions<CalculoImpostoContext> options) : DbContext(options)
{



    public DbSet<InssGov> InssGov { get; set; }
    public DbSet<Inss> Inss { get; set; }
    public DbSet<IrrfRfb> IrrfRfb { get; set; }
    public DbSet<Irrf> Irrf { get; set; }
    public DbSet<Simplificado> Simplificado { get; set; }
    public DbSet<Dependente> Dependente { get; set; }
    public DbSet<DescontoMinimo> DescontoMinimo { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlite(@"Data Source = BancoDados/calculoIrrf.db");
    //}

}
