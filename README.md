# Calculo IRRF

Descrição completa do projeto "Calculo IRRF" — uma aplicação desktop (Windows Forms) em .NET 9 para cálculo do Imposto de Renda Retido na Fonte (IRRF), com suporte a tabelas locais e atualização online (scraping) das tabelas públicas.

Sumário
- Visão geral
- Principais funcionalidades
- Tecnologias e dependências
- Requisitos do sistema
- Estrutura do projeto
- Como compilar / executar (Visual Studio 2022 / dotnet)
- Banco de dados e migrações
- Como atualizar tabelas online (scraping)
- Testes e validação
- Contribuição e suporte
- Licença

Visão geral
----------
Aplicação Windows Forms que calcula IRRF considerando:
- Cálculo normal (com dependentes e INSS)
- Cálculo simplificado (dedução fixa)
- Cálculo progressivo por faixas
- Comparação de vantagem (normal vs simplificado)
- Manutenção de parâmetros: valor por dependente, dedução simplificada, desconto mínimo, faixas de INSS/IRRF
- Atualização online das tabelas de IRRF via scraping (HtmlAgilityPack) a partir de páginas públicas (ex.: contabeis.com.br)

Principais funcionalidades
--------------------------
- Forms para manutenção de:
  - `FrmDeducaoSimplificada` — gerenciar valores de dedução simplificada por competência
  - `FrmDescontoMinimo` — gerenciar valor mínimo para que haja desconto de IR
  - (Outros forms esperados no projeto para gerenciamento de faixas, dependentes, etc.)
- Serviços de cálculo no namespace `CalculoIRRF.Services.Calculo`:
  - `IrrfCalculo` — implementa regras de cálculo (Base normal, base simplificada, progressivo, descrições)
  - Serviços auxiliares para persistência e regras: `ISimplificadoServices`, `IDescontoMinimoServices`, `IIrrfServices`, `IDependenteServices`, etc.
- Persistência com Entity Framework Core (SQLite):
  - Migrations geradas em `CalculoIRRF\Migrations`
  - Seed dos dados iniciais (dependente, dedução simplificada, faixas INSS/IRRF)
- Atualização online:
  - `CalculoIRRF.Tributacao.IRRF.TributacaoRFB` usa `HtmlAgilityPack` para extrair faixas, deduções e valores de dependente de uma página pública
  - O resultado é persistido via serviços `IIrrfServices` (método `GravarRfb`)

Tecnologias e dependências
--------------------------
- .NET 9 (Target Framework — verificar no arquivo de projeto)
- C# 11 (construção com sintaxe de record-like DI em classes)
- Windows Forms (WinForms) — app desktop com atributo `[SupportedOSPlatform("windows")]`
- Entity Framework Core (provider SQLite)
  - `Microsoft.EntityFrameworkCore`
  - `Microsoft.EntityFrameworkCore.Sqlite`
  - `Microsoft.EntityFrameworkCore.Design`
  - `dotnet-ef` (ferramenta CLI para migrations)
- HtmlAgilityPack — para scraping de tabelas online
- Ferramentas comuns: Visual Studio 2022 (recomendado) ou CLI `dotnet`

Requisitos do sistema
---------------------
- Windows 10/11 (aplicação WinForms)
- .NET 9 Runtime instalado
- Visual Studio 2022 com workload de .NET Desktop Development (se for abrir/compilar na IDE)
- Conexão com internet para usar a funcionalidade de atualização online (scraping)

Estrutura do projeto (resumo)
-----------------------------
- `CalculoIRRF/` — projeto principal
  - `FrmDeducaoSimplificada.cs`, `FrmDescontoMinimo.cs` — forms de manutenção
  - `Services/` — lógica de negócio e serviços (ex.: `DescontoMinimoServices.cs`, `Calculo/IrrfCalculo.cs`)
  - `Repository/Interface/` — interfaces dos repositórios (ex.: `IIrrfRepository.cs`, `IDependenteRepository.cs`)
  - `Model/` — entidades EF Core (ex.: `Dependente.cs`, `DescontoMinimo.cs`, `Irrf.cs`, `IrrfRfb.cs`, `Simplificado.cs`)
  - `Migrations/` — migrations EF Core e snapshot (seed incluído)
  - `Tributacao/IRRF/` — scraping e transformação (`TributacaoRFB.cs`, `TributacaoRFBObj.cs`)
  - `DataBase/` — (esperado) contexto `CalculoImpostoContext` (usado pelo EF)
  - `Services/Validacao` — helpers de validação/formatação (ex.: `Validar`)

Como compilar e executar
-----------------------
Opção A — Visual Studio 2022 (recomendado)
1. Abra a solução no Visual Studio 2022.
2. Certifique-se de que o projeto de inicialização esteja correto (`CalculoIRRF`).
3. Restaure os pacotes NuGet (Build > Restore NuGet Packages).
4. Aplique as migrations (ver seção abaixo) ou deixe o projeto criar o banco ao iniciar, se implementado.
5. Compile (Build > Build Solution).
6. Execute (F5) — aplicativo será aberto como aplicação WinForms.

Opção B — CLI (.NET SDK)
1. Abra terminal na pasta da solução.
2. Restaurar pacotes:
   - `dotnet restore`
3. (Instale a ferramenta EF Core se necessário)
   - `dotnet tool install --global dotnet-ef` (ou `dotnet tool update --global dotnet-ef`)
4. Compilar:
   - `dotnet build`
5. Aplicar migrations (ver abaixo) e executar:
   - `dotnet ef database update --project CalculoIRRF` (ou use o PM> Console no Visual Studio)
   - `dotnet run --project CalculoIRRF`

Banco de dados e migrations
---------------------------
- Projeto já inclui migrations em `CalculoIRRF\Migrations` (arquivo `Initial` e snapshot).
- Provider usado nas migrations: SQLite (observe `annotation "Sqlite:Autoincrement"`).
- Seeds iniciais incluem valores para `Dependente`, `DescontoMinimo`, `Inss`, `Irrf` e `Simplificado`.
- Comando para aplicar migrations:
  - CLI: `dotnet ef database update --project CalculoIRRF`
  - Visual Studio Package Manager Console (PM>):
    - `Update-Database -Project CalculoIRRF`
- Se precisar criar nova migration:
  - `dotnet ef migrations add NomeDaMigration --project CalculoIRRF`
  - `dotnet ef database update --project CalculoIRRF`

Observações sobre scraping / atualização online
----------------------------------------------
- A classe `TributacaoRFB` (arquivo `CalculoIRRF/Tributacao/IRRF/TributacaoRFB.cs`) busca a tabela em `https://www.contabeis.com.br/tabelas/imposto-renda/` e tenta extrair:
  - vigência da tabela
  - faixas (base, alíquota, dedução)
  - valor por dependente e dedução simplificada (25% da primeira faixa)
- Requisitos:
  - Dependência `HtmlAgilityPack`
  - Mudanças no layout do site podem quebrar o parser; trate com logs e validações.
- A atualização verifica se já existe vigência gravada (`IIrrfServices.IsGov`) e grava via `IIrrfServices.GravarRfb`.
- Segurança/ética: respeite o robots.txt e termos do site; este scraping é de exemplo para atualização automática — validar uso em produção.

Principais classes e responsabilidades
--------------------------------------
- `IrrfCalculo` — realiza cálculos do IRRF:
  - `BaseIrrfNormal()`, `BaseIrrfSimplificado()`
  - `Normal()`, `Simplificado()`, `NormalProgressivo()`
  - Métodos para gerar descrições detalhadas do cálculo (`DescricaoCalculoNormal`, etc.)
- `DescontoMinimoServices` — CRUD e leitura do valor mínimo para desconto
- `TributacaoRFB` — extrai e grava tabelas de IRRF obtidas online
- Repositórios (`*Repository`) — interface para persistência (CRUD + consultas específicas)
- Models: `Dependente`, `DescontoMinimo`, `Inss`, `Irrf`, `IrrfRfb`, `Simplificado`
- Validação e formatação: `Validar` (usado nas forms)

Boas práticas e pontos de atenção
--------------------------------
- Tratar all exceptions com logs adequados; evitar mostrar mensagens de erro técnicas ao usuário final.
- Ao usar scraping, incluir mecanismo de retry/backoff e validar conteúdo antes de gravar.
- Testar cálculos com valores conhecidos (unidades de teste ajudam a validar regressões).
- Separar responsabilidade: cálculo puro (unit testável) deve ser independente de UI e banco.
- Verifique formatação regional (pt-BR) ao parsear `MM/yyyy` e valores com `,` (vírgula) — o projeto mostra tratamentos usando `Validar`.

Testes e validação
------------------
- Recomenda-se criar testes unitários para:
  - Regras de cálculo em `IrrfCalculo` (normal, simplificado, progressivo)
  - Regras de formatação/validação em `Validar`
  - Parsers da `TributacaoRFB` usando HTML de exemplo (mocks)
- Dados de seed nas migrations permitem testes manuais rápidos.

Contribuição
------------
1. Abra uma issue descrevendo o problema/feature.
2. Crie uma branch com convenção `feature/` ou `fix/`.
3. Submeta PR com descrição clara do que foi alterado e por quê.
4. Mantenha compatibilidade com .NET 9 e padrão de codificação do projeto.

Suporte / Troubleshooting
-------------------------
- Se o app não iniciar: verifique se o runtime .NET 9 está instalado.
- Problemas com EF / migrations: execute `dotnet ef database update` e verifique se a cadeia de conexão SQLite (se presente) aponta para local gravável.
- Erro ao usar atualização online: verifique se a página fonte mudou (HTML diferente). Consulte `CalculoIRRF/Tributacao/IRRF/TributacaoRFB.cs` para adaptar os seletores XPath.
- Mensagens de parsing de datas: confirme cultura/região ao converter `MM/yyyy` e outras datas.

Exemplo rápido de execução (CLI)
-------------------------------
1. Restaurar:
   - `dotnet restore`
2. Build:
   - `dotnet build`
3. Aplicar migrations:
   - `dotnet ef database update --project CalculoIRRF`
4. Executar:
   - `dotnet run --project CalculoIRRF`

Observações finais
------------------
- Projeto focado em ambiente Windows (WinForms) e usa EF Core + SQLite para persistência local.
- Seed inicial já incluiu tabelas e valores base; reveja `Migrations` se precisar atualizar valores.
- Ao integrar scraping em produção, tratar limites, caches e validações para evitar gravação de dados corrompidos.

Contato
-------
- Para dúvidas sobre implementação ou execução, abra uma issue no repositório.
