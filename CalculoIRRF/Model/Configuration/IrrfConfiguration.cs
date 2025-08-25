using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace CalculoIRRF.Model.Configuration;

public class IrrfConfiguration : IEntityTypeConfiguration<Irrf>
{
	public void Configure(EntityTypeBuilder<Irrf> builder)
	{
		builder.HasData(
			new Irrf { Id = 1, Faixa = 1, Valor = 1903.98d, Porcentagem = 0, Deducao = 0, Competencia = DateTime.Parse("01/01/2015") },
			   new Irrf { Id = 2, Faixa = 2, Valor = 2826.65d, Porcentagem = 7.50d, Deducao = 142.80d, Competencia = DateTime.Parse("01/01/2015") },
			   new Irrf { Id = 3, Faixa = 3, Valor = 3751.05d, Porcentagem = 15d, Deducao = 354.80d, Competencia = DateTime.Parse("01/01/2015") },
			   new Irrf { Id = 4, Faixa = 4, Valor = 4664.68d, Porcentagem = 22.50d, Deducao = 636.13d, Competencia = DateTime.Parse("01/01/2015") },
			   new Irrf { Id = 5, Faixa = 5, Valor = 99999999999.99d, Porcentagem = 27.50d, Deducao = 869.36d, Competencia = DateTime.Parse("01/01/2015") },
			   new Irrf { Id = 6, Faixa = 1, Valor = 2112.00d, Porcentagem = 0, Deducao = 0, Competencia = DateTime.Parse("01/05/2023") },
			   new Irrf { Id = 8, Faixa = 2, Valor = 2826.65d, Porcentagem = 7.50d, Deducao = 158.40d, Competencia = DateTime.Parse("01/05/2023") },
			   new Irrf { Id = 9, Faixa = 3, Valor = 3751.05d, Porcentagem = 15d, Deducao = 370.40d, Competencia = DateTime.Parse("01/05/2023") },
			   new Irrf { Id = 10, Faixa = 4, Valor = 4664.68d, Porcentagem = 22.50d, Deducao = 651.73d, Competencia = DateTime.Parse("01/05/2023") },
			   new Irrf { Id = 11, Faixa = 5, Valor = 99999999999.99d, Porcentagem = 27.50d, Deducao = 884.96d, Competencia = DateTime.Parse("01/05/2023") },
			   new Irrf { Id = 12, Faixa = 1, Valor = 2259.20d, Porcentagem = 0, Deducao = 0, Competencia = DateTime.Parse("01/02/2024") },
			   new Irrf { Id = 13, Faixa = 2, Valor = 2826.65d, Porcentagem = 7.50d, Deducao = 169.44d, Competencia = DateTime.Parse("01/02/2024") },
			   new Irrf { Id = 14, Faixa = 3, Valor = 3751.05d, Porcentagem = 15d, Deducao = 381.44d, Competencia = DateTime.Parse("01/02/2024") },
			   new Irrf { Id = 15, Faixa = 4, Valor = 4664.68d, Porcentagem = 22.50d, Deducao = 662.77d, Competencia = DateTime.Parse("01/02/2024") },
			   new Irrf { Id = 16, Faixa = 5, Valor = 99999999999.99d, Porcentagem = 27.50d, Deducao = 896.00d, Competencia = DateTime.Parse("01/02/2024") },
			   new Irrf { Id = 17, Faixa = 1, Valor = 2428.80d, Porcentagem = 0, Deducao = 0, Competencia = DateTime.Parse("01/05/2025") },
			   new Irrf { Id = 18, Faixa = 2, Valor = 2826.65d, Porcentagem = 7.50d, Deducao = 169.44d, Competencia = DateTime.Parse("01/05/2025") },
			   new Irrf { Id = 19, Faixa = 3, Valor = 3751.05d, Porcentagem = 15d, Deducao = 381.44d, Competencia = DateTime.Parse("01/05/2025") },
			   new Irrf { Id = 20, Faixa = 4, Valor = 4664.68d, Porcentagem = 22.50d, Deducao = 662.77d, Competencia = DateTime.Parse("01/05/2025") },
			   new Irrf { Id = 21, Faixa = 5, Valor = 99999999999.99d, Porcentagem = 27.50d, Deducao = 896.00d, Competencia = DateTime.Parse("01/05/2025") }
	   );
	}
}
