# Calculadora de Imposto

Aplicação desktop WPF em .NET 9 para simular os encargos incidentes sobre rendimentos no Brasil. O cálculo considera IRRF, INSS e FGTS, com parâmetros locais por competência e atualização das tabelas oficiais.

## Funcionalidades

- Simulação de IRRF pelas modalidades normal e simplificada, incluindo redução mensal e dependentes.
- Cálculo progressivo do INSS, FGTS padrão de 8% e FGTS de 2% específico para Jovem Aprendiz.
- Exportação dos resultados de imposto e pensão para PDF profissional, com resumo e detalhamento das faixas calculadas.
- Comparação entre as modalidades de IRRF para indicar a mais vantajosa.
- Simulação de pensão alimentícia a partir do resultado tributário.
- Manutenção local das tabelas de INSS, IRRF, dedução simplificada, dependentes, desconto mínimo e redução mensal.
- Atualização online das tabelas oficiais de INSS e IRRF.
- Dados persistidos localmente em SQLite e preferência de tema armazenada no computador.
- Seed idempotente com as competências de 2017 a 2026 do INSS e do IRRF, preservando dados mantidos manualmente pelo usuário.

## Arquitetura

O projeto é organizado em camadas, mantendo as regras tributárias independentes de interface e persistência:

- `Domain/`: cálculos progressivos e regras tributárias puras.
- `Application/`: casos de uso, DTOs e contratos.
- `Infrastructure/`: EF Core, SQLite e atualizadores das fontes oficiais.
- `Presentation/`: MVVM, navegação, notificações e tema.
- `Views/`: telas WPF e code-behind mínimo.

Os nomes técnicos que contêm `IRRF` permanecem intencionalmente nas regras, entidades e tabelas referentes especificamente a esse tributo. Eles não representam o nome do produto.

## Requisitos

- Windows 10 ou superior.
- .NET 9 SDK para compilar ou .NET 9 Desktop Runtime para executar a aplicação publicada.

## Executar

No diretório da solução:

```powershell
dotnet restore
dotnet build
dotnet run --project .\CalculoIRRF\CalculoIRRF.csproj
```

O executável gerado chama-se `CalculadoraDeImposto.exe`. O banco de dados SQLite é copiado para a saída em `BancoDados\calculoIrrf.db`; o nome do arquivo foi preservado para manter compatibilidade com as instalações existentes.

## Observações

- Os resultados são simulações e devem ser conferidos com a legislação e os dados aplicáveis à competência.
- A atualização online depende da disponibilidade e do formato das páginas oficiais. Caso a consulta falhe, os dados locais são preservados.
- Para competências anteriores a março de 2020, o INSS é calculado pelo regime de alíquota única; a partir dessa competência, é aplicado o regime progressivo.
