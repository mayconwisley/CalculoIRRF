# Calculadora de Imposto

<p align="center">
  <picture>
    <source media="(prefers-color-scheme: dark)" srcset="CalculoIRRF/Assets/logo-dark.png" />
    <img src="CalculoIRRF/Assets/logo-light.png" width="180" alt="Logo da Calculadora de Imposto" />
  </picture>
</p>

<p align="center">
  Aplicação desktop para simular IRRF, INSS, FGTS, pensão alimentícia e indenização de estabilidade.
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet&logoColor=white" alt=".NET 9" />
  <img src="https://img.shields.io/badge/C%23-13-239120?logo=csharp&logoColor=white" alt="C# 13" />
  <img src="https://img.shields.io/badge/WPF-Windows-0078D4?logo=windows&logoColor=white" alt="WPF para Windows" />
  <img src="https://img.shields.io/badge/Entity%20Framework%20Core-9.0-512BD4?logo=dotnet&logoColor=white" alt="Entity Framework Core 9" />
  <img src="https://img.shields.io/badge/SQLite-Local%20database-003B57?logo=sqlite&logoColor=white" alt="SQLite" />
  <img src="https://img.shields.io/badge/QuestPDF-2025.12-FF4B4B" alt="QuestPDF" />
</p>

## Visão geral

A **Calculadora de Imposto** é uma central de cálculos trabalhistas e tributários executados localmente. Ela reúne a simulação de IRRF, INSS e FGTS, a pensão alimentícia e a indenização por estabilidade, com memória de cálculo e relatórios em PDF para conferência.

> Os resultados têm caráter de simulação. A conferência com a legislação vigente, o vínculo empregatício e os dados da competência continua sendo indispensável.

## Recursos

### Simulação tributária

- Calcula **IRRF** pelas modalidades normal e simplificada.
- Considera dependentes, desconto simplificado, desconto mínimo e redução mensal do IRRF quando aplicável.
- Calcula **INSS** por faixas: alíquota única nas competências anteriores a março de 2020 e modelo progressivo nas posteriores.
- Calcula **FGTS padrão (8%)** e **FGTS para Jovem Aprendiz (2%)**.
- Compara as modalidades de IRRF e destaca a alternativa mais vantajosa.
- Exibe indicadores, faixas utilizadas e fórmulas na memória de cálculo.

### Pensão alimentícia e documentos

- Simula pensão alimentícia com percentual e outros descontos configuráveis.
- Compara os resultados das modalidades de tributação na composição da pensão.
- Exporta relatórios em PDF com resumo executivo e memória de cálculo.

### Cálculo de estabilidade

- Apura indenização proporcional aos dias restantes de estabilidade.
- Calcula 13º salário e férias proporcionais, incluindo o adicional de um terço.
- Calcula FGTS de 8% e multa rescisória de 40%, com complementos informados pelo usuário.
- Exibe a memória de cálculo para conferência dos valores.
- Gera um demonstrativo em PDF inspirado no relatório legado, com as verbas, os dados considerados e o **Total a Receber**.

### Gestão de tabelas

- Mantém localmente faixas de INSS e IRRF, dedução por dependente, desconto simplificado, desconto mínimo e redução mensal.
- Permite incluir, editar e remover registros por competência.
- Inicializa dados históricos de forma idempotente, sem sobrescrever manutenções locais.
- Atualiza tabelas de INSS e IRRF a partir das fontes oficiais configuradas na aplicação; em caso de falha, preserva os dados locais.

### Experiência de uso

- Tema claro, escuro e automático, seguindo a preferência do Windows no modo automático.
- Controles de rolagem utilizam a aparência nativa do WPF, preservando a visibilidade e a usabilidade em todos os temas.
- Preferência de tema persistida no perfil do usuário.
- Processamento e persistência locais em SQLite.
- Identidade visual adaptada aos dois temas, com ícones Windows multirresolução.

## Tecnologias

| Tecnologia | Versão | Uso no projeto |
| --- | --- | --- |
| [.NET](https://dotnet.microsoft.com/) | 9 | Plataforma de execução e compilação. |
| C# | 13 | Linguagem principal da aplicação. |
| [WPF](https://learn.microsoft.com/dotnet/desktop/wpf/) | .NET 9 | Interface desktop, recursos e temas. |
| [Entity Framework Core](https://learn.microsoft.com/ef/core/) | 9.0.2 | Acesso a dados e mapeamento da persistência. |
| [SQLite](https://www.sqlite.org/) | — | Banco de dados local das tabelas tributárias. |
| [QuestPDF](https://www.questpdf.com/) | 2025.12.1 | Geração dos relatórios PDF. |
| [Html Agility Pack](https://html-agility-pack.net/) | 1.11.74 | Leitura das fontes HTML usadas nas atualizações oficiais. |

## Arquitetura

O projeto aplica uma separação pragmática em camadas. O domínio não depende de WPF, EF Core, SQLite ou bibliotecas de PDF.

```text
CalculoIRRF/
├── Domain/              Regras tributárias e de estabilidade independentes de UI e infraestrutura
├── Application/         Casos de uso, DTOs e portas de entrada/saída
├── Infrastructure/      EF Core, SQLite, atualizadores oficiais e relatórios PDF
├── Presentation/        MVVM, serviços WPF, comportamentos e gerenciamento de tema
├── Views/               Janelas e composição visual em XAML
├── Assets/              Logos e ícones para os temas claro e escuro
└── BancoDados/          Banco SQLite distribuído com a aplicação
```

### Fluxo da simulação tributária

1. A tela WPF encaminha os valores ao `MainWindowViewModel`.
2. O caso de uso na camada `Application` valida e orquestra a simulação.
3. A porta de consulta obtém o perfil tributário adequado à competência.
4. As calculadoras do `Domain` executam as regras sem conhecimento de infraestrutura.
5. A `Presentation` transforma o resultado em indicadores, comparativos e memória de cálculo.

### Fluxo do cálculo de estabilidade

1. A tela coleta média remuneratória, dias-base, demissão, término da estabilidade e complementos.
2. `SimularEstabilidadeUseCase` valida e encaminha os dados ao domínio.
3. `CalculadoraEstabilidade` calcula indenização proporcional, 13º, férias, adicional de 1/3, FGTS e multa de 40%.
4. A apresentação exibe a memória de cálculo e permite gerar o demonstrativo PDF.

## Pré-requisitos

- Windows 10 ou superior.
- **.NET 9 SDK** para compilar e desenvolver.
- **.NET 9 Desktop Runtime** para executar uma publicação dependente do framework.

## Executar localmente

No diretório raiz da solução:

```powershell
dotnet restore
dotnet build .\CalculoIRRF.sln
dotnet run --project .\CalculoIRRF\CalculoIRRF.csproj
```

O executável de desenvolvimento é gerado como `CalculadoraDeImposto.exe` em `CalculoIRRF\bin\<configuração>\net9.0-windows`.

## Publicar para Windows

Exemplo de publicação dependente do runtime para Windows 64 bits:

```powershell
dotnet publish .\CalculoIRRF\CalculoIRRF.csproj `
  --configuration Release `
  --runtime win-x64 `
  --self-contained false `
  --output .\publish
```

Para uma distribuição sem pré-requisito do runtime .NET, altere `--self-contained` para `true`.

## Dados locais e configurações

| Item | Localização | Comportamento |
| --- | --- | --- |
| Tabelas tributárias | `CalculoIRRF\BancoDados\calculoIrrf.db` | Copiado para a saída; armazena faixas e parâmetros por competência. |
| Tema | `%LOCALAPPDATA%\CalculoIRRF\settings.json` | Mantém a opção Claro, Escuro ou Automático. |
| PDFs | Diretório escolhido pelo usuário | Gerados sob demanda para simulações tributárias, pensão e estabilidade. |

O nome `calculoIrrf.db` é mantido para preservar compatibilidade com instalações existentes.

## Atualização das tabelas oficiais

A atualização online consulta as fontes oficiais configuradas para INSS e IRRF. Como a estrutura das páginas públicas pode mudar, a operação é tratada como complementar: uma falha de consulta não substitui, apaga nem invalida os registros locais já existentes.

Revise os valores atualizados antes de utilizá-los em cálculos que exijam precisão legal ou contábil.

## Identidade visual

O aplicativo seleciona automaticamente a logo apropriada ao tema ativo. Os arquivos também podem ser reutilizados em instaladores e materiais de distribuição:

| Tema claro | Tema escuro |
| --- | --- |
| [Logo PNG](CalculoIRRF/Assets/logo-light.png) · [Ícone ICO](CalculoIRRF/Assets/icon-light.ico) | [Logo PNG](CalculoIRRF/Assets/logo-dark.png) · [Ícone ICO](CalculoIRRF/Assets/icon-dark.ico) |

## Limitações e responsabilidade

- A aplicação não substitui sistemas de folha de pagamento, contadores ou orientação jurídica.
- A exatidão da simulação depende da competência e dos parâmetros tributários mantidos no banco local.
- No cálculo de estabilidade, cabe ao usuário informar corretamente as datas, a média remuneratória, os dias-base e os complementos aplicáveis ao vínculo.
- Atualizações online dependem da disponibilidade e da estrutura das páginas das fontes oficiais.
