; Instalador da Calculadora de Imposto (Inno Setup 6).
; Normalmente é compilado por instalador\gerar-instalador.ps1, que publica o aplicativo e informa a versão e as pastas:
;   ISCC /DAppVersion=1.2.0 /DAppVersionNumeric=1.2.0.0 /DPublishDir=<pasta do dotnet publish> /DOutputDir=<saída> CalculadoraDeImposto.iss

#ifndef AppVersion
  #define AppVersion "1.0.0"
#endif
#ifndef AppVersionNumeric
  #define AppVersionNumeric "1.0.0.0"
#endif
#ifndef PublishDir
  #error Informe /DPublishDir com a pasta gerada pelo dotnet publish (use instalador\gerar-instalador.ps1).
#endif
#ifndef OutputDir
  #define OutputDir "..\artefatos"
#endif

#define AppName "Calculadora de Imposto"
#define AppExe "CalculadoraDeImposto.exe"
#define AppPublisher "Maycon Wisley"
#define AppUrl "https://github.com/mayconwisley/CalculoIRRF"

[Setup]
; O AppId identifica o aplicativo entre as versões. Não altere: é ele que faz cada nova versão substituir a anterior.
AppId={{4D6A52F2-3EEC-44A6-9D19-4C7B94F97794}
AppName={#AppName}
AppVersion={#AppVersion}
AppVerName={#AppName} {#AppVersion}
AppPublisher={#AppPublisher}
AppPublisherURL={#AppUrl}
AppSupportURL={#AppUrl}/issues
AppUpdatesURL={#AppUrl}/releases
VersionInfoVersion={#AppVersionNumeric}
VersionInfoProductName={#AppName}
VersionInfoProductVersion={#AppVersionNumeric}
VersionInfoDescription=Instalador da {#AppName}
; Instalação por usuário e sem administrador: o aplicativo grava o banco SQLite na própria pasta,
; o que usuários comuns não podem fazer em "Arquivos de Programas".
PrivilegesRequired=lowest
DefaultDirName={autopf}\{#AppName}
DisableProgramGroupPage=yes
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
MinVersion=10.0
WizardStyle=modern
SetupIconFile=..\CalculoIRRF\Assets\icon-light.ico
UninstallDisplayIcon={app}\{#AppExe}
UninstallDisplayName={#AppName}
Compression=lzma2/ultra64
SolidCompression=yes
OutputDir={#OutputDir}
OutputBaseFilename=CalculadoraDeImposto-{#AppVersion}-setup
; Fecha o aplicativo aberto antes de atualizar os arquivos.
CloseApplications=yes
RestartApplications=no

[Languages]
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "{#PublishDir}\*"; DestDir: "{app}"; Excludes: "BancoDados\*"; Flags: ignoreversion recursesubdirs createallsubdirs
; O banco guarda as tabelas mantidas pelo usuário: é copiado só na primeira instalação, não é sobrescrito
; nas atualizações (novos dados de referência chegam pelo inicializador do próprio app) e permanece ao desinstalar.
Source: "{#PublishDir}\BancoDados\calculoIrrf.db"; DestDir: "{app}\BancoDados"; Flags: onlyifdoesntexist uninsneveruninstall

[Icons]
Name: "{autoprograms}\{#AppName}"; Filename: "{app}\{#AppExe}"; WorkingDir: "{app}"
Name: "{autodesktop}\{#AppName}"; Filename: "{app}\{#AppExe}"; WorkingDir: "{app}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#AppExe}"; Description: "{cm:LaunchProgram,{#AppName}}"; WorkingDir: "{app}"; Flags: nowait postinstall skipifsilent
