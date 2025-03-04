﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace CalculoIRRF.Model.Configuration;

public class InssConfiguration : IEntityTypeConfiguration<Inss>
{
    public void Configure(EntityTypeBuilder<Inss> builder)
    {
        builder.HasData(
            new Inss { Id = 1, Faixa = 1, Valor = 1045.00d, Porcentagem = 7.5d, Competencia = DateTime.Parse("01/03/2020") },
            new Inss { Id = 2, Faixa = 2, Valor = 2089.60d, Porcentagem = 9d, Competencia = DateTime.Parse("01/03/2020") },
            new Inss { Id = 3, Faixa = 3, Valor = 3134.40d, Porcentagem = 12d, Competencia = DateTime.Parse("01/03/2020") },
            new Inss { Id = 4, Faixa = 4, Valor = 6101.06d, Porcentagem = 14d, Competencia = DateTime.Parse("01/03/2020") },
            new Inss { Id = 5, Faixa = 1, Valor = 1212.00d, Porcentagem = 7.5d, Competencia = DateTime.Parse("01/01/2022") },
            new Inss { Id = 6, Faixa = 2, Valor = 2452.67d, Porcentagem = 9d, Competencia = DateTime.Parse("01/01/2022") },
            new Inss { Id = 7, Faixa = 3, Valor = 3679.00d, Porcentagem = 12d, Competencia = DateTime.Parse("01/01/2022") },
            new Inss { Id = 8, Faixa = 4, Valor = 7087.22d, Porcentagem = 14d, Competencia = DateTime.Parse("01/01/2022") },
            new Inss { Id = 9, Faixa = 1, Valor = 1302.00d, Porcentagem = 7.5d, Competencia = DateTime.Parse("01/01/2023") },
            new Inss { Id = 10, Faixa = 2, Valor = 2571.29d, Porcentagem = 9d, Competencia = DateTime.Parse("01/01/2023") },
            new Inss { Id = 11, Faixa = 3, Valor = 3856.94d, Porcentagem = 12d, Competencia = DateTime.Parse("01/01/2023") },
            new Inss { Id = 12, Faixa = 4, Valor = 7507.49d, Porcentagem = 14d, Competencia = DateTime.Parse("01/01/2023") },
            new Inss { Id = 13, Faixa = 1, Valor = 1320.00d, Porcentagem = 7.5d, Competencia = DateTime.Parse("01/05/2023") },
            new Inss { Id = 14, Faixa = 2, Valor = 2571.29d, Porcentagem = 9d, Competencia = DateTime.Parse("01/05/2023") },
            new Inss { Id = 15, Faixa = 3, Valor = 3856.94d, Porcentagem = 12d, Competencia = DateTime.Parse("01/05/2023") },
            new Inss { Id = 16, Faixa = 4, Valor = 7507.49d, Porcentagem = 14d, Competencia = DateTime.Parse("01/05/2023") },
            new Inss { Id = 17, Faixa = 1, Valor = 1412.00d, Porcentagem = 7.5d, Competencia = DateTime.Parse("01/01/2024") },
            new Inss { Id = 18, Faixa = 2, Valor = 2666.68d, Porcentagem = 9d, Competencia = DateTime.Parse("01/01/2024") },
            new Inss { Id = 19, Faixa = 3, Valor = 4000.03d, Porcentagem = 12d, Competencia = DateTime.Parse("01/01/2024") },
            new Inss { Id = 20, Faixa = 4, Valor = 7786.02d, Porcentagem = 14d, Competencia = DateTime.Parse("01/01/2024") },
            new Inss { Id = 21, Faixa = 1, Valor = 1518.00d, Porcentagem = 7.5d, Competencia = DateTime.Parse("01/01/2025") },
            new Inss { Id = 22, Faixa = 2, Valor = 2793.88d, Porcentagem = 9d, Competencia = DateTime.Parse("01/01/2025") },
            new Inss { Id = 23, Faixa = 3, Valor = 4190.83d, Porcentagem = 12d, Competencia = DateTime.Parse("01/01/2025") },
            new Inss { Id = 24, Faixa = 4, Valor = 8157.41d, Porcentagem = 14d, Competencia = DateTime.Parse("01/01/2025") }
        );
    }
}
