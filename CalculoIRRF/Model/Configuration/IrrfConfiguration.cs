using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace CalculoIRRF.Model.Configuration;

public class IrrfConfiguration : IEntityTypeConfiguration<Irrf>
{
    public void Configure(EntityTypeBuilder<Irrf> builder)
    {
        builder.HasData(
           new Irrf { Id = 1, Faixa = 1, Valor = 1903.98m, Porcentagem = 0, Deducao = 0, Competencia = DateTime.Parse("01/01/2015") },
           new Irrf { Id = 2, Faixa = 2, Valor = 2826.65m, Porcentagem = 7.50m, Deducao = 142.80m, Competencia = DateTime.Parse("01/01/2015") },
           new Irrf { Id = 3, Faixa = 3, Valor = 3751.05m, Porcentagem = 15m, Deducao = 354.80m, Competencia = DateTime.Parse("01/01/2015") },
           new Irrf { Id = 4, Faixa = 4, Valor = 4664.68m, Porcentagem = 22.50m, Deducao = 636.13m, Competencia = DateTime.Parse("01/01/2015") },
           new Irrf { Id = 5, Faixa = 5, Valor = 99999999999.99m, Porcentagem = 27.50m, Deducao = 869.36m, Competencia = DateTime.Parse("01/01/2015") },
           new Irrf { Id = 6, Faixa = 1, Valor = 2112.00m, Porcentagem = 0, Deducao = 0, Competencia = DateTime.Parse("01/05/2023") },
           new Irrf { Id = 8, Faixa = 2, Valor = 2826.65m, Porcentagem = 7.50m, Deducao = 158.40m, Competencia = DateTime.Parse("01/05/2023") },
           new Irrf { Id = 9, Faixa = 3, Valor = 3751.05m, Porcentagem = 15m, Deducao = 370.40m, Competencia = DateTime.Parse("01/05/2023") },
           new Irrf { Id = 10, Faixa = 4, Valor = 4664.68m, Porcentagem = 22.50m, Deducao = 651.73m, Competencia = DateTime.Parse("01/05/2023") },
           new Irrf { Id = 11, Faixa = 5, Valor = 99999999999.99m, Porcentagem = 27.50m, Deducao = 884.96m, Competencia = DateTime.Parse("01/05/2023") },
           new Irrf { Id = 12, Faixa = 1, Valor = 2259.20m, Porcentagem = 0, Deducao = 0, Competencia = DateTime.Parse("01/02/2024") },
           new Irrf { Id = 13, Faixa = 2, Valor = 2826.65m, Porcentagem = 7.50m, Deducao = 169.44m, Competencia = DateTime.Parse("01/02/2024") },
           new Irrf { Id = 14, Faixa = 3, Valor = 3751.05m, Porcentagem = 15m, Deducao = 381.44m, Competencia = DateTime.Parse("01/02/2024") },
           new Irrf { Id = 15, Faixa = 4, Valor = 4664.68m, Porcentagem = 22.50m, Deducao = 662.77m, Competencia = DateTime.Parse("01/02/2024") },
           new Irrf { Id = 16, Faixa = 5, Valor = 99999999999.99m, Porcentagem = 27.50m, Deducao = 896.00m, Competencia = DateTime.Parse("01/02/2024") }
       );
    }
}
