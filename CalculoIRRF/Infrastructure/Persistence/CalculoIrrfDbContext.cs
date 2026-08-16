using CalculoIRRF.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CalculoIRRF.Infrastructure.Persistence;

public sealed class CalculoIrrfDbContext(DbContextOptions<CalculoIrrfDbContext> options) : DbContext(options)
{
    internal DbSet<FaixaInssEntity> FaixasInss => Set<FaixaInssEntity>();
    internal DbSet<FaixaIrrfEntity> FaixasIrrf => Set<FaixaIrrfEntity>();
    internal DbSet<ParametroSimplificadoEntity> ParametrosSimplificados => Set<ParametroSimplificadoEntity>();
    internal DbSet<ParametroDependenteEntity> ParametrosDependentes => Set<ParametroDependenteEntity>();
    internal DbSet<ParametroDescontoMinimoEntity> ParametrosDescontoMinimo => Set<ParametroDescontoMinimoEntity>();
    internal DbSet<ReducaoIrrfMensalEntity> ReducoesMensaisIrrf => Set<ReducaoIrrfMensalEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FaixaInssEntity>(builder => builder.ToTable("Inss"));
        modelBuilder.Entity<FaixaIrrfEntity>(builder => builder.ToTable("Irrf"));
        modelBuilder.Entity<ParametroSimplificadoEntity>(builder => builder.ToTable("Simplificado"));
        modelBuilder.Entity<ParametroDependenteEntity>(builder => builder.ToTable("Dependente"));
        modelBuilder.Entity<ParametroDescontoMinimoEntity>(builder => builder.ToTable("DescontoMinimo"));
        modelBuilder.Entity<ReducaoIrrfMensalEntity>(builder => builder.ToTable("ReducaoMensalIrrf"));
    }
}
