﻿// <auto-generated />
using System;
using CalculoIRRF.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CalculoIRRF.Migrations
{
    [DbContext(typeof(CalculoImpostoContext))]
    partial class CalculoImpostoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.2");

            modelBuilder.Entity("CalculoIRRF.Model.Dependente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Competencia")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Valor")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Dependente");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Competencia = new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Valor = 189.59m
                        });
                });

            modelBuilder.Entity("CalculoIRRF.Model.DescontoMinimo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Competencia")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Valor")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("DescontoMinimo");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Competencia = new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Valor = 10m
                        });
                });

            modelBuilder.Entity("CalculoIRRF.Model.Inss", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Competencia")
                        .HasColumnType("TEXT");

                    b.Property<int>("Faixa")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Porcentagem")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Valor")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Inss");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Competencia = new DateTime(2020, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Faixa = 1,
                            Porcentagem = 7.5m,
                            Valor = 1045.00m
                        },
                        new
                        {
                            Id = 2,
                            Competencia = new DateTime(2020, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Faixa = 2,
                            Porcentagem = 9m,
                            Valor = 2089.60m
                        },
                        new
                        {
                            Id = 3,
                            Competencia = new DateTime(2020, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Faixa = 3,
                            Porcentagem = 12m,
                            Valor = 3134.40m
                        },
                        new
                        {
                            Id = 4,
                            Competencia = new DateTime(2020, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Faixa = 4,
                            Porcentagem = 14m,
                            Valor = 6101.06m
                        },
                        new
                        {
                            Id = 5,
                            Competencia = new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Faixa = 1,
                            Porcentagem = 7.5m,
                            Valor = 1212.00m
                        },
                        new
                        {
                            Id = 6,
                            Competencia = new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Faixa = 2,
                            Porcentagem = 9m,
                            Valor = 2452.67m
                        },
                        new
                        {
                            Id = 7,
                            Competencia = new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Faixa = 3,
                            Porcentagem = 12m,
                            Valor = 3679.00m
                        },
                        new
                        {
                            Id = 8,
                            Competencia = new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Faixa = 4,
                            Porcentagem = 14m,
                            Valor = 7087.22m
                        },
                        new
                        {
                            Id = 9,
                            Competencia = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Faixa = 1,
                            Porcentagem = 7.5m,
                            Valor = 1302.00m
                        },
                        new
                        {
                            Id = 10,
                            Competencia = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Faixa = 2,
                            Porcentagem = 9m,
                            Valor = 2571.29m
                        },
                        new
                        {
                            Id = 11,
                            Competencia = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Faixa = 3,
                            Porcentagem = 12m,
                            Valor = 3856.94m
                        },
                        new
                        {
                            Id = 12,
                            Competencia = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Faixa = 4,
                            Porcentagem = 14m,
                            Valor = 7507.49m
                        },
                        new
                        {
                            Id = 13,
                            Competencia = new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Faixa = 1,
                            Porcentagem = 7.5m,
                            Valor = 1320.00m
                        },
                        new
                        {
                            Id = 14,
                            Competencia = new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Faixa = 2,
                            Porcentagem = 9m,
                            Valor = 2571.29m
                        },
                        new
                        {
                            Id = 15,
                            Competencia = new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Faixa = 3,
                            Porcentagem = 12m,
                            Valor = 3856.94m
                        },
                        new
                        {
                            Id = 16,
                            Competencia = new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Faixa = 4,
                            Porcentagem = 14m,
                            Valor = 7507.49m
                        },
                        new
                        {
                            Id = 17,
                            Competencia = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Faixa = 1,
                            Porcentagem = 7.5m,
                            Valor = 1412.00m
                        },
                        new
                        {
                            Id = 18,
                            Competencia = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Faixa = 2,
                            Porcentagem = 9m,
                            Valor = 2666.68m
                        },
                        new
                        {
                            Id = 19,
                            Competencia = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Faixa = 3,
                            Porcentagem = 12m,
                            Valor = 4000.03m
                        },
                        new
                        {
                            Id = 20,
                            Competencia = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Faixa = 4,
                            Porcentagem = 14m,
                            Valor = 7786.02m
                        },
                        new
                        {
                            Id = 21,
                            Competencia = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Faixa = 1,
                            Porcentagem = 7.5m,
                            Valor = 1518.00m
                        },
                        new
                        {
                            Id = 22,
                            Competencia = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Faixa = 2,
                            Porcentagem = 9m,
                            Valor = 2793.88m
                        },
                        new
                        {
                            Id = 23,
                            Competencia = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Faixa = 3,
                            Porcentagem = 12m,
                            Valor = 4190.83m
                        },
                        new
                        {
                            Id = 24,
                            Competencia = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Faixa = 4,
                            Porcentagem = 14m,
                            Valor = 8157.41m
                        });
                });

            modelBuilder.Entity("CalculoIRRF.Model.InssGov", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Aliquota")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("BaseCaculo")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DataAtualizacao")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("TEXT");

                    b.Property<int>("Sequencia")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("InssGov");
                });

            modelBuilder.Entity("CalculoIRRF.Model.Irrf", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Competencia")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Deducao")
                        .HasColumnType("TEXT");

                    b.Property<int>("Faixa")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Porcentagem")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Valor")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Irrf");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Competencia = new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Deducao = 0m,
                            Faixa = 1,
                            Porcentagem = 0m,
                            Valor = 1903.98m
                        },
                        new
                        {
                            Id = 2,
                            Competencia = new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Deducao = 142.80m,
                            Faixa = 2,
                            Porcentagem = 7.50m,
                            Valor = 2826.65m
                        },
                        new
                        {
                            Id = 3,
                            Competencia = new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Deducao = 354.80m,
                            Faixa = 3,
                            Porcentagem = 15m,
                            Valor = 3751.05m
                        },
                        new
                        {
                            Id = 4,
                            Competencia = new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Deducao = 636.13m,
                            Faixa = 4,
                            Porcentagem = 22.50m,
                            Valor = 4664.68m
                        },
                        new
                        {
                            Id = 5,
                            Competencia = new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Deducao = 869.36m,
                            Faixa = 5,
                            Porcentagem = 27.50m,
                            Valor = 99999999999.99m
                        },
                        new
                        {
                            Id = 6,
                            Competencia = new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Deducao = 0m,
                            Faixa = 1,
                            Porcentagem = 0m,
                            Valor = 2112.00m
                        },
                        new
                        {
                            Id = 8,
                            Competencia = new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Deducao = 158.40m,
                            Faixa = 2,
                            Porcentagem = 7.50m,
                            Valor = 2826.65m
                        },
                        new
                        {
                            Id = 9,
                            Competencia = new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Deducao = 370.40m,
                            Faixa = 3,
                            Porcentagem = 15m,
                            Valor = 3751.05m
                        },
                        new
                        {
                            Id = 10,
                            Competencia = new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Deducao = 651.73m,
                            Faixa = 4,
                            Porcentagem = 22.50m,
                            Valor = 4664.68m
                        },
                        new
                        {
                            Id = 11,
                            Competencia = new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Deducao = 884.96m,
                            Faixa = 5,
                            Porcentagem = 27.50m,
                            Valor = 99999999999.99m
                        },
                        new
                        {
                            Id = 12,
                            Competencia = new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Deducao = 0m,
                            Faixa = 1,
                            Porcentagem = 0m,
                            Valor = 2259.20m
                        },
                        new
                        {
                            Id = 13,
                            Competencia = new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Deducao = 169.44m,
                            Faixa = 2,
                            Porcentagem = 7.50m,
                            Valor = 2826.65m
                        },
                        new
                        {
                            Id = 14,
                            Competencia = new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Deducao = 381.44m,
                            Faixa = 3,
                            Porcentagem = 15m,
                            Valor = 3751.05m
                        },
                        new
                        {
                            Id = 15,
                            Competencia = new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Deducao = 662.77m,
                            Faixa = 4,
                            Porcentagem = 22.50m,
                            Valor = 4664.68m
                        },
                        new
                        {
                            Id = 16,
                            Competencia = new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Deducao = 896.00m,
                            Faixa = 5,
                            Porcentagem = 27.50m,
                            Valor = 99999999999.99m
                        });
                });

            modelBuilder.Entity("CalculoIRRF.Model.IrrfRfb", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Aliquota")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("BaseCaculo")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DataAtualizacao")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Deducao")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Dependente")
                        .HasColumnType("TEXT");

                    b.Property<int>("Sequencia")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Simplificado")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("IrrfRfb");
                });

            modelBuilder.Entity("CalculoIRRF.Model.Simplificado", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Competencia")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Valor")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Simplificado");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Competencia = new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Valor = 528.00m
                        },
                        new
                        {
                            Id = 2,
                            Competencia = new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Valor = 564.80m
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
