using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace CalculoIRRF.Model.Configuration;

public class DescontoMinimoConfiguration : IEntityTypeConfiguration<DescontoMinimo>
{
    public void Configure(EntityTypeBuilder<DescontoMinimo> builder)
    {
        builder.HasData(
            new DescontoMinimo { Id = 1, Valor = 10d, Competencia = DateTime.Parse("01/05/2023") }
        );
    }
}
