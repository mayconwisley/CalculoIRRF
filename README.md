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
  <img src="https://img.shields.io/badge/SQLite-Local%20database-003B57?logo=sqlite&logoColor=white" alt="SQLite" />
  <img src="https://img.shields.io/badge/QuestPDF-2025.12-FF4B4B" alt="QuestPDF" />
</p>

## Visão geral

A **Calculadora de Imposto** é uma central de cálculos trabalhistas e tributários executados localmente. Ela reúne a simulação de IRRF, INSS e FGTS, a pensão alimentícia e a indenização por estabilidade, com memória de cálculo e relatórios em PDF para conferência.

> Os resultados têm caráter de simulação. A conferência com a legislação vigente, o vínculo empregatício e os dados da competência continua sendo indispensável.

## Instalação

Baixe o instalador **CalculadoraDeImposto-X.Y.Z-setup.exe** na página de [Releases](https://github.com/mayconwisley/CalculoIRRF/releases) e execute-o.

- A instalação é feita para o usuário atual, em `%LOCALAPPDATA%\Programs\Calculadora de Imposto`, e não pede permissão de administrador.
- O .NET já vem incluído: não é preciso instalar nenhum pré-requisito.
- Ao instalar uma versão nova sobre a anterior, as tabelas editadas pelo usuário são preservadas. A desinstalação também mantém o banco de dados.
- Como o instalador não é assinado digitalmente, o Windows pode exibir o aviso do SmartScreen: selecione **Mais informações** e depois **Executar assim mesmo**.

## Manual do usuário

O passo a passo completo de cada tela, com prints, está no **[Manual do Usuário](docs/MANUAL.md)**, também disponível em **[PDF](docs/ManualDoUsuario.pdf)**. No aplicativo, o manual abre pelo botão **Manual do usuário** ou pela tecla **F1**, em qualquer janela.

O PDF é gerado a partir do Markdown. Depois de editar `docs/MANUAL.md` ou as imagens em `docs/imagens`, regenere-o na raiz do repositório:

```powershell
dotnet run --project .\tools\GeradorManual
```

## Recursos

### Simulação tributária

- Calcula **IRRF** pelas modalidades normal e simplificada.
- Considera dependentes, desconto simplificado, desconto mínimo e redução mensal do IRRF quando aplicável.
- Calcula **INSS** por faixas: alíquota única nas competências anteriores a março de 2020 e modelo progressivo nas posteriores.
- Calcula **FGTS padrão (8%)** e **FGTS para Jovem Aprendiz (2%)**.
- Compara as modalidades de IRRF e destaca a alternativa mais vantajosa.
- Mostra o salário líquido, descontando o INSS e o IRRF da modalidade mais vantajosa.
- Exibe indicadores, faixas utilizadas e fórmulas na memória de cálculo, com cada dedução da base do IRRF identificada.

### Calculadoras trabalhistas

Todas usam a mesma janela, com resumo, demonstrativo no formato de holerite, valores informativos (como o FGTS), memória de cálculo, observações e PDF:

| Calculadora | O que calcula |
| --- | --- |
| Salário bruto a partir do líquido | O salário bruto que, descontados INSS e IRRF, resulta no líquido desejado. |
| Horas extras e adicionais | Horas extras em duas faixas, adicional noturno com a hora reduzida e o reflexo no DSR, com o líquido do mês. |
| Insalubridade e periculosidade | Insalubridade de 10%, 20% ou 40% sobre o salário mínimo (ou outra base de convenção) e periculosidade de 30%, aplicando o maior quando os dois se aplicam. |
| Salário-família | Direito e valor pela remuneração e pelos filhos, com as duas faixas anteriores a 2020 e a cota proporcional na admissão e no desligamento. |
| 13º salário | 1ª e 2ª parcelas, com médias, avos e INSS e IRRF de tributação exclusiva. |
| Férias | Férias com 1/3, dias de direito conforme as faltas, venda de 1/3 (abono, isento) e adiantamento do 13º. |
| PLR | IRRF pela tabela anual exclusiva da Lei 10.101/2000, recalculado sobre o total do ano, com a dedução da pensão alimentícia e sem INSS e FGTS. |
| Rescisão | Verbas por motivo de desligamento, aviso prévio proporcional com projeção, férias vencidas e proporcionais, FGTS, multa e saque. |
| Custo do funcionário | Encargos por regime tributário (Lucro Real ou Presumido e Simples Nacional), provisões de 13º e férias e benefícios. |
| Pró-labore e autônomo | INSS de 11% até o teto, IRRF, ISS do autônomo e o custo para a empresa. |

As regras da CLT ficam em `Domain/Trabalhista/RegrasTrabalhistas`, e a apuração de INSS e IRRF comum a todas as calculadoras, em `Application/UseCases/TabelasDaCompetencia`.

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

- Mantém localmente faixas de INSS e IRRF, dedução por dependente, desconto simplificado, desconto mínimo, redução mensal, tabela anual da PLR, salário-família e salário mínimo, com o histórico desde 2017.
- Permite incluir, editar e remover registros por competência.
- Inicializa dados históricos de forma idempotente, sem sobrescrever manutenções locais.
- Atualiza tabelas de INSS e IRRF pela fonte oficial ou, quando ela ainda não publicou a tabela do ano, por duas fontes alternativas que concordem entre si; em caso de falha, preserva os dados locais.

### Experiência de uso

- Manual do usuário integrado: botão no cabeçalho e tecla F1 em qualquer janela.
- Tema claro, escuro e automático, seguindo a preferência do Windows no modo automático.
- Barras de rolagem finas e arredondadas, no padrão atual do Windows, com cores ajustadas a cada tema.
- Valores formatados no padrão brasileiro em todas as telas, inclusive nas tabelas de manutenção.
- Preferência de tema persistida no perfil do usuário.
- Renderização por software por padrão, reduzindo o consumo de memória; a aceleração por GPU pode ser reativada nas configurações locais.
- Processamento e persistência locais em SQLite.
- Identidade visual adaptada aos dois temas, com ícones Windows multirresolução.

## Tecnologias

| Tecnologia | Versão | Uso no projeto |
| --- | --- | --- |
| [.NET](https://dotnet.microsoft.com/) | 9 | Plataforma de execução e compilação. |
| C# | 13 | Linguagem principal da aplicação. |
| [WPF](https://learn.microsoft.com/dotnet/desktop/wpf/) | .NET 9 | Interface desktop, recursos e temas. |
| [SQLite](https://www.sqlite.org/) | — | Banco de dados local das tabelas tributárias. |
| [Microsoft.Data.Sqlite](https://learn.microsoft.com/dotnet/standard/data/sqlite/) | 9.0.2 | Acesso direto ao SQLite, com as tabelas mantidas em memória entre os cálculos. |
| [QuestPDF](https://www.questpdf.com/) | 2025.12.1 | Geração dos relatórios PDF. |
| [Html Agility Pack](https://html-agility-pack.net/) | 1.11.74 | Leitura das páginas HTML usadas na atualização das tabelas. |

## Arquitetura

O projeto aplica uma separação pragmática em camadas. O domínio não depende de WPF, SQLite ou bibliotecas de PDF.

```text
CalculoIRRF/
├── Domain/              Regras tributárias e de estabilidade independentes de UI e infraestrutura
├── Application/         Casos de uso, DTOs e portas de entrada/saída
├── Infrastructure/      SQLite, atualização das tabelas pela internet e relatórios PDF
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
- Para gerar e publicar instaladores: [Inno Setup 6](https://jrsoftware.org/isdl.php) e o [GitHub CLI](https://cli.github.com) autenticado (`gh auth login`).

## Executar localmente

No diretório raiz da solução:

```powershell
dotnet restore
dotnet build .\CalculoIRRF.sln
dotnet run --project .\CalculoIRRF\CalculoIRRF.csproj
```

O executável de desenvolvimento é gerado como `CalculadoraDeImposto.exe` em `CalculoIRRF\bin\<configuração>\net9.0-windows`.

## Gerar o instalador

O instalador é gerado localmente com o Inno Setup. O script publica o aplicativo para Windows x64 com o .NET incluído e grava o resultado em `artefatos\`:

```powershell
.\instalador\gerar-instalador.ps1 -Versao 1.2.0
```

| Arquivo | Função |
| --- | --- |
| `instalador\CalculadoraDeImposto.iss` | Definição do instalador: instalação por usuário, atalhos, idioma e preservação do banco. |
| `instalador\gerar-instalador.ps1` | Publica o aplicativo e compila o instalador. |
| `instalador\publicar-release.ps1` | Gera o instalador de uma tag e o publica no GitHub Releases. |
| `.githooks\pre-push` | Aciona a publicação quando uma tag de versão é enviada. |

## Publicar uma nova versão

A publicação acontece ao enviar uma tag no formato `vX.Y.Z`. Todo o processo roda na sua máquina; o GitHub recebe apenas a tag e o instalador pronto.

Ative os hooks do repositório uma única vez por clone:

```powershell
git config core.hooksPath .githooks
```

Depois, para cada versão:

```powershell
git tag v1.2.0
git push origin master v1.2.0
```

Durante o push, o hook:

1. Compila o aplicativo a partir de uma cópia isolada do código da tag (`git worktree`), com a versão da tag gravada no executável.
2. Gera o instalador com o Inno Setup. Se algo falhar, **o push é cancelado** e nenhuma versão quebrada chega ao GitHub.
3. Deixa um processo em segundo plano aguardando a tag chegar ao GitHub. Em seguida, ele cria o release com o instalador, as instruções de instalação e o SHA-256 do arquivo. O resultado é avisado em uma janela e registrado em `artefatos\release-vX.Y.Z.log`.

Pushes sem tag de versão não são afetados. Tags com sufixo, como `v1.2.0-beta.1`, são publicadas como *pré-lançamento*.

Para testar o processo sem enviar nada, use a simulação, que gera o instalador e mostra o que seria publicado:

```powershell
$env:CALCULADORA_SIMULAR_RELEASE = '1'
git push --dry-run origin v1.2.0
Remove-Item Env:CALCULADORA_SIMULAR_RELEASE
```

Se a tag já estiver no GitHub sem o release, por exemplo porque a conexão caiu, publique manualmente:

```powershell
.\instalador\publicar-release.ps1 -Tag v1.2.0
```

## Dados locais e configurações

| Item | Localização | Comportamento |
| --- | --- | --- |
| Tabelas tributárias | `BancoDados\calculoIrrf.db`, na pasta do aplicativo | Armazena faixas e parâmetros por competência. Instalado apenas na primeira instalação; atualizações e a desinstalação preservam o arquivo. |
| Tema e renderização | `%LOCALAPPDATA%\CalculoIRRF\settings.json` | Mantém a opção Claro, Escuro ou Automático. Com `"HardwareAcceleration": true`, a interface volta a ser renderizada pela GPU, com maior consumo de memória. |
| PDFs | Diretório escolhido pelo usuário | Gerados sob demanda para simulações tributárias, pensão e estabilidade. |

O nome `calculoIrrf.db` é mantido para preservar compatibilidade com instalações existentes.

## Atualização das tabelas pela internet

No começo do ano, alguns sites publicam as novas tabelas de INSS e IRRF antes das páginas do governo. Para a calculadora não ficar atrasada, a atualização consulta em paralelo a fonte oficial e duas fontes alternativas:

| Tabela | Fonte oficial | Fontes alternativas |
| --- | --- | --- |
| INSS | [gov.br/inss](https://www.gov.br/inss/pt-br/direitos-e-deveres/inscricao-e-contribuicao/tabela-de-contribuicao-mensal) | [debit.com.br](https://www.debit.com.br/tabelas/tabelas-inss) e [contabeis.com.br](https://www.contabeis.com.br/tabelas/inss/) |
| IRRF | [Receita Federal](https://www.gov.br/receitafederal/pt-br/assuntos/meu-imposto-de-renda/tabelas) | [debit.com.br](https://www.debit.com.br/tabelas/tabelas-irrf) e [contabeis.com.br](https://www.contabeis.com.br/tabelas/imposto-renda/) |

A tabela gravada é a de competência mais recente que tenha sido confirmada:

- a da fonte oficial vale sozinha;
- sem ela, a tabela só é aceita quando as duas fontes alternativas trazem exatamente os mesmos valores. Uma fonte sozinha, ou fontes que divergem, não alteram o banco, e a mensagem avisa qual fonte já tem a tabela nova.

As fontes alternativas do IRRF publicam apenas as faixas. Nesse caso, o desconto simplificado é calculado em 25% do limite da faixa isenta, como determina a Lei 9.250/1995, e a dedução por dependente e a redução mensal continuam com os valores cadastrados.

Cada fonte é lida por uma classe em `Infrastructure/Tributacao/Fontes`, e a escolha entre elas fica em `ConsultaDeFontes`. Como a estrutura das páginas públicas pode mudar, uma fonte com falha é apenas ignorada; se nenhuma tabela for confirmada, nada é gravado e a mensagem explica o que aconteceu com cada fonte.

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
- Atualizações online dependem da disponibilidade e da estrutura das páginas das fontes oficiais e alternativas.
