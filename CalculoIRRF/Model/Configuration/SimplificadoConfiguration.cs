using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace CalculoIRRF.Model.Configuration;

public class SimplificadoConfiguration : IEntityTypeConfiguration<Simplificado>
{
    public void Configure(EntityTypeBuilder<Simplificado> builder)
    {
        builder.HasData(
        new Simplificado { Id = 1, Valor = 528.00d, Competencia = DateTime.Parse("01/05/2023") },
            new Simplificado { Id = 2, Valor = 564.80d, Competencia = DateTime.Parse("01/02/2024") }
        );

    }
}
