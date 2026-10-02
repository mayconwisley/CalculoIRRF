<#
.SYNOPSIS
Publica a Calculadora de Imposto e gera o instalador com o Inno Setup.

.DESCRIPTION
Publica o aplicativo para Windows x64 com o .NET incluído (o usuário não precisa instalar o runtime),
compila instalador\CalculadoraDeImposto.iss e devolve o caminho do instalador gerado.
Não envia nada ao GitHub: a publicação é feita por publicar-release.ps1.

.EXAMPLE
.\instalador\gerar-instalador.ps1 -Versao 1.2.0
#>
param(
    [Parameter(Mandatory = $true)] [string] $Versao,
    # Raiz do código-fonte a compilar. Na publicação de uma tag, é a cópia isolada (worktree) daquela tag.
    [string] $Raiz,
    [string] $Saida
)

$ErrorActionPreference = 'Stop'
# Por pipe, a saída vai em UTF-8; no console, o PowerShell já escreve em Unicode.
# Quando roda pelo hook, publicar-release.ps1 também tira os acentos das mensagens.
if ([Console]::IsOutputRedirected) { [Console]::OutputEncoding = New-Object Text.UTF8Encoding $false }
# No Windows PowerShell 5.1, $PSScriptRoot ainda não existe nos valores padrão do bloco param.
if (-not $Raiz) { $Raiz = Split-Path $PSScriptRoot -Parent }
if (-not $Saida) { $Saida = Join-Path (Split-Path $PSScriptRoot -Parent) 'artefatos' }

if ($Versao -notmatch '^\d+\.\d+\.\d+(-[0-9A-Za-z.-]+)?$') {
    throw "Versão inválida: '$Versao'. Use o formato 1.2.0 ou 1.2.0-beta.1."
}
# O Windows exige versão numérica com quatro partes nos metadados do instalador.
$versaoNumerica = ($Versao -split '-')[0] + '.0'

function Find-Iscc {
    $candidatos = @()
    if ($env:ISCC) { $candidatos += $env:ISCC }
    $comando = Get-Command ISCC.exe -ErrorAction SilentlyContinue
    if ($comando) { $candidatos += $comando.Source }
    foreach ($chave in @(
            'HKLM:\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Inno Setup 6_is1',
            'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Inno Setup 6_is1',
            'HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Inno Setup 6_is1')) {
        $pasta = (Get-ItemProperty $chave -ErrorAction SilentlyContinue).InstallLocation
        if ($pasta) { $candidatos += (Join-Path $pasta 'ISCC.exe') }
    }
    $candidatos += (Join-Path ${env:ProgramFiles(x86)} 'Inno Setup 6\ISCC.exe')
    $candidatos += (Join-Path $env:ProgramFiles 'Inno Setup 6\ISCC.exe')

    $iscc = $candidatos | Where-Object { $_ -and (Test-Path $_) } | Select-Object -First 1
    if (-not $iscc) {
        throw 'Inno Setup 6 não encontrado. Instale-o (https://jrsoftware.org/isdl.php) ou informe o caminho do ISCC.exe na variável de ambiente ISCC.'
    }
    $iscc
}

$iscc = Find-Iscc
$projeto = Join-Path $Raiz 'CalculoIRRF\CalculoIRRF.csproj'
$script = Join-Path $Raiz 'instalador\CalculadoraDeImposto.iss'
$publicacao = Join-Path ([IO.Path]::GetTempPath()) ("CalculadoraDeImposto-publicacao-" + [Guid]::NewGuid().ToString('N'))

try {
    Write-Host "==> Publicando a versão $Versao (win-x64, .NET incluído)..."
    # SatelliteResourceLanguages=pt-BR descarta as traduções do .NET e do WPF para outros idiomas, reduzindo o instalador.
    & dotnet publish $projeto -c Release -r win-x64 --self-contained true -o $publicacao --nologo `
        "-p:Version=$Versao" '-p:SatelliteResourceLanguages=pt-BR' '-p:DebugType=none' | Out-Host
    if ($LASTEXITCODE -ne 0) { throw "dotnet publish falhou (código $LASTEXITCODE)." }

    if (-not (Test-Path (Join-Path $publicacao 'Manual\ManualDoUsuario.pdf'))) {
        Write-Warning 'O manual em PDF não está na publicação; gere-o com: dotnet run --project .\tools\GeradorManual'
    }

    New-Item -ItemType Directory -Force -Path $Saida | Out-Null
    Write-Host "==> Compilando o instalador com $iscc..."
    & $iscc /Q "/DAppVersion=$Versao" "/DAppVersionNumeric=$versaoNumerica" "/DPublishDir=$publicacao" "/DOutputDir=$Saida" $script | Out-Host
    if ($LASTEXITCODE -ne 0) { throw "O Inno Setup não conseguiu compilar o instalador (código $LASTEXITCODE)." }
}
finally {
    if (Test-Path $publicacao) { Remove-Item -LiteralPath $publicacao -Recurse -Force }
}

$instalador = Join-Path $Saida "CalculadoraDeImposto-$Versao-setup.exe"
if (-not (Test-Path $instalador)) { throw "Instalador não encontrado em $instalador." }
Write-Host ("==> Instalador gerado: {0} ({1:N1} MB)" -f $instalador, ((Get-Item $instalador).Length / 1MB))
$instalador
