using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace CalculoIRRF.Model.Configuration;

public class DependenteConfiguration : IEntityTypeConfiguration<Dependente>
{
    public void Configure(EntityTypeBuilder<Dependente> builder)
    {
        builder.HasData(
       new Dependente { Id = 1, Valor = 189.59d, Competencia = DateTime.Parse("01/01/2015") }
       );
    }
}
