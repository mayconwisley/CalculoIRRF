<#
.SYNOPSIS
Gera o instalador de uma tag de versão e o publica no GitHub Releases.

.DESCRIPTION
Executado pelo hook .githooks\pre-push quando uma tag vX.Y.Z é enviada:
  1. Durante o push, gera o instalador a partir de uma cópia isolada (git worktree) do código da tag.
     Se a compilação falhar, o push é cancelado e nenhuma versão quebrada chega ao GitHub.
  2. O pre-push roda antes de a tag existir no GitHub, e o release depende dela. Por isso a publicação
     fica com um processo em segundo plano, que aguarda a tag chegar e cria o release com o instalador.
     O resultado é avisado em uma janela e registrado em artefatos\release-<tag>.log.

Também pode ser executado manualmente para publicar (ou republicar) uma tag que já está no GitHub.

.EXAMPLE
git tag v1.2.0
git push origin v1.2.0
# O hook gera o instalador e publica o release.

.EXAMPLE
.\instalador\publicar-release.ps1 -Tag v1.2.0
# Publica manualmente uma tag que já foi enviada.

.EXAMPLE
$env:CALCULADORA_SIMULAR_RELEASE = '1'; git push origin v1.2.0 --dry-run
# Simulação: gera o instalador e mostra o que seria publicado, sem enviar nada.
#>
param(
    [string] $Remoto = 'origin',
    [string[]] $Tag = @(),
    [string] $Instalador,
    [switch] $AguardarTag,
    [switch] $Simular
)

$ErrorActionPreference = 'Stop'
# No console, o PowerShell escreve em Unicode e os acentos aparecem normalmente. Por pipe, cada programa decodifica
# a saída do hook de um jeito: o VS Code usa UTF-8, e o Console do Gerenciador de Pacotes do Visual Studio usa a
# página de código ANSI. Para ficar legível em todos, as mensagens saem sem acentos nesse caso.
if ([Console]::IsOutputRedirected) {
    [Console]::OutputEncoding = New-Object Text.UTF8Encoding $false
    function Remove-Acentos([string] $texto) { $texto.Normalize([Text.NormalizationForm]::FormD) -replace '\p{Mn}', '' }
    # Valem também para gerar-instalador.ps1, que roda neste mesmo processo.
    function Write-Host {
        param([Parameter(Position = 0, ValueFromRemainingArguments = $true)] [object[]] $Object, [ConsoleColor] $ForegroundColor)
        Microsoft.PowerShell.Utility\Write-Host (Remove-Acentos "$Object")
    }
    function Write-Warning([string] $Message) { Microsoft.PowerShell.Utility\Write-Warning (Remove-Acentos $Message) }
    # Repassa a saída do dotnet, do git e do Inno Setup, que pode vir em português.
    function Out-Host { process { Microsoft.PowerShell.Core\Out-Host -InputObject (Remove-Acentos "$_") } }
}
$raiz = Split-Path $PSScriptRoot -Parent
$artefatos = Join-Path $raiz 'artefatos'
$padraoTag = '^v(\d+\.\d+\.\d+(-[0-9A-Za-z.-]+)?)$'
if ($env:CALCULADORA_SIMULAR_RELEASE -eq '1') { $Simular = $true }

# Comandos nativos (git, gh) escrevem progresso no stderr; no Windows PowerShell isso viraria erro com 'Stop'.
function Invoke-Nativo([string] $descricao, [scriptblock] $comando) {
    $anterior = $ErrorActionPreference
    $ErrorActionPreference = 'Continue'
    try { & $comando 2>&1 | ForEach-Object { "    $_" } | Out-Host }
    finally { $ErrorActionPreference = $anterior }
    if ($LASTEXITCODE -ne 0) { throw "$descricao falhou (código $LASTEXITCODE)." }
}

function Test-Nativo([scriptblock] $comando) {
    $anterior = $ErrorActionPreference
    $ErrorActionPreference = 'Continue'
    try { & $comando 2>&1 | Out-Null } finally { $ErrorActionPreference = $anterior }
    $LASTEXITCODE -eq 0
}

function Get-RepositorioGitHub {
    $url = (& git -C $raiz remote get-url $Remoto).Trim()
    if ($url -notmatch 'github\.com[:/](?<repositorio>[^/]+/[^/]+?)(\.git)?/?$') { throw "O remoto '$Remoto' ($url) não é um repositório do GitHub." }
    $Matches['repositorio']
}

function New-InstaladorDaTag([string] $nome) {
    $copia = Join-Path ([IO.Path]::GetTempPath()) ("CalculadoraDeImposto-$nome-" + [Guid]::NewGuid().ToString('N').Substring(0, 8))
    Write-Host "==> Preparando uma cópia isolada do código da tag $nome..."
    Invoke-Nativo "Preparar a cópia da tag $nome" { git -C $raiz worktree add --detach $copia $nome }
    try {
        $gerador = Join-Path $copia 'instalador\gerar-instalador.ps1'
        if (-not (Test-Path $gerador)) { throw "A tag $nome não contém instalador\gerar-instalador.ps1." }
        & $gerador -Versao $nome.Substring(1) -Raiz $copia -Saida $artefatos | Select-Object -Last 1
    }
    finally {
        Invoke-Nativo 'Remover a cópia da tag' { git -C $raiz worktree remove --force $copia }
    }
}

function Write-Notas([string] $nome, [string] $arquivo) {
    $versao = $nome.Substring(1)
    $hash = (Get-FileHash $arquivo -Algorithm SHA256).Hash.ToLowerInvariant()
    $repositorio = Get-RepositorioGitHub
    $notas = @"
## Instalação

1. Baixe **$(Split-Path $arquivo -Leaf)** em *Assets*, logo abaixo.
2. Execute o instalador. A instalação é feita para o usuário atual, não pede permissão de administrador e já inclui o .NET.
3. Ao atualizar uma versão anterior, as tabelas que você editou são preservadas.

Como o instalador não é assinado digitalmente, o Windows pode exibir o aviso do SmartScreen: selecione **Mais informações** e depois **Executar assim mesmo**.

O manual do usuário acompanha o aplicativo (botão **Manual do usuário** ou tecla **F1**) e também está disponível em [docs/MANUAL.md](https://github.com/$repositorio/blob/$nome/docs/MANUAL.md).

SHA-256 do instalador: ``$hash``

"@
    $caminho = Join-Path $artefatos "notas-$nome.md"
    [IO.File]::WriteAllText($caminho, $notas, (New-Object Text.UTF8Encoding $false))
    $caminho
}

function Publish-Release([string] $nome, [string] $arquivo) {
    $repositorio = Get-RepositorioGitHub
    $argumentos = @('release', 'create', $nome, $arquivo, '--repo', $repositorio, '--verify-tag',
        '--title', "Calculadora de Imposto $($nome.Substring(1))", '--notes-file', (Write-Notas $nome $arquivo), '--generate-notes')
    if ($nome -like '*-*') { $argumentos += '--prerelease' }
    if ($Simular) {
        Write-Host "==> Simulação: gh $($argumentos -join ' ')"
        return "https://github.com/$repositorio/releases/tag/$nome"
    }
    if ($AguardarTag) {
        Write-Host "==> Aguardando a tag $nome chegar ao GitHub..."
        $limite = (Get-Date).AddMinutes(15)
        while (-not (& git -C $raiz ls-remote --tags $Remoto "refs/tags/$nome")) {
            if ((Get-Date) -gt $limite) { throw "A tag $nome não apareceu no GitHub em 15 minutos. Verifique o push e publique com: .\instalador\publicar-release.ps1 -Tag $nome" }
            Start-Sleep -Seconds 3
        }
    }
    Write-Host "==> Criando o release $nome em $repositorio..."
    Invoke-Nativo "Criar o release $nome" { gh @argumentos }
    "https://github.com/$repositorio/releases/tag/$nome"
}

function Show-Aviso([string] $mensagem, [bool] $erro) {
    Add-Type -AssemblyName System.Windows.Forms
    $icone = if ($erro) { 'Error' } else { 'Information' }
    [System.Windows.Forms.MessageBox]::Show($mensagem, 'Calculadora de Imposto - publicação', 'OK', $icone, 'Button1', 'DefaultDesktopOnly') | Out-Null
}

# Modo manual ou processo em segundo plano: publica as tags informadas.
if ($Tag.Count -gt 0) {
    New-Item -ItemType Directory -Force -Path $artefatos | Out-Null
    foreach ($nome in $Tag) {
        if ($nome -notmatch $padraoTag) { throw "Tag inválida: '$nome'. Use o formato v1.2.0." }
        $log = Join-Path $artefatos "release-$nome.log"
        if ($AguardarTag) { Start-Transcript -Path $log -Force | Out-Null }
        try {
            if (-not $Instalador) { $Instalador = New-InstaladorDaTag $nome }
            $url = Publish-Release $nome $Instalador
            if ($Simular) { Write-Host "==> Simulação concluída; nada foi enviado ao GitHub ($url)." } else { Write-Host "==> Release publicado: $url" }
            if ($AguardarTag -and -not $Simular) { Show-Aviso "Release $nome publicado com o instalador.`n`n$url" $false }
        }
        catch {
            Write-Host "ERRO: $_"
            if ($AguardarTag -and -not $Simular) { Show-Aviso "Não foi possível publicar o release $nome.`n`n$_`n`nDetalhes em: $log" $true }
            throw
        }
        finally {
            if ($AguardarTag) { Stop-Transcript | Out-Null }
            $Instalador = $null
        }
    }
    exit 0
}

# Modo hook: o git informa no stdin uma linha por referência enviada: <ref local> <sha local> <ref remota> <sha remota>.
$entrada = if ([Console]::IsInputRedirected) { [Console]::In.ReadToEnd() } else { '' }
$tags = @($entrada -split "`n" | ForEach-Object { ($_.Trim() -split ' ')[0] } |
    Where-Object { $_ -match '^refs/tags/v' } | ForEach-Object { $_.Substring('refs/tags/'.Length) } |
    Where-Object { $_ -match $padraoTag } | Select-Object -Unique)
if ($tags.Count -eq 0) { exit 0 }

try {
    if (-not $Simular) {
        if (-not (Get-Command gh -ErrorAction SilentlyContinue)) { throw 'O GitHub CLI (gh) não está instalado: https://cli.github.com' }
        if (-not (Test-Nativo { gh auth status })) { throw 'O GitHub CLI não está autenticado. Execute: gh auth login' }
    }
    $repositorio = Get-RepositorioGitHub
    foreach ($nome in $tags) {
        if (-not $Simular -and (Test-Nativo { gh release view $nome --repo $repositorio })) {
            throw "Já existe um release $nome no GitHub. Use uma nova versão."
        }
        $arquivo = New-InstaladorDaTag $nome
        $comando = "-NoProfile -ExecutionPolicy Bypass -WindowStyle Hidden -File `"$PSCommandPath`" -Remoto `"$Remoto`" -Tag $nome -Instalador `"$arquivo`" -AguardarTag"
        if ($Simular) {
            # Executa a mesma etapa de segundo plano, mas aguardando e sem criar o release.
            Write-Host "==> Simulação da publicação em segundo plano:"
            Start-Process -FilePath 'powershell.exe' -ArgumentList "$comando -Simular" -Wait -NoNewWindow
            continue
        }
        Start-Process -FilePath 'powershell.exe' -ArgumentList $comando -WindowStyle Hidden
        Write-Host "==> Instalador pronto. O release $nome será publicado assim que a tag chegar ao GitHub (acompanhe em artefatos\release-$nome.log)."
    }
    exit 0
}
catch {
    Write-Host ''
    Write-Host "Push cancelado: $_" -ForegroundColor Red
    exit 1
}
