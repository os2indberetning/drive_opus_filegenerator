; This file is a script that allows to build the instalation package
; The script may be executed from the console-mode compiler - iscc "c:\isetup\samples\my script.iss" or from the Inno Setup Compiler UI
#define AppId "{{3e3325b5-fb3a-4626-ae10-7e689fa70e61}"
#define AppSourceDir "..\OpusFileGenerator\bin\Debug"
#define AppName "OpusFileGenerator"
#define AppVersion "1.0"
#define AppPublisher "Digital Identity"
#define AppURL "http://digital-identity.dk/"

[Setup]
AppId={#AppId}
AppName={#AppName}
AppVersion={#AppVersion}
AppPublisher={#AppPublisher}
AppPublisherURL={#AppURL}
AppSupportURL={#AppURL}
AppUpdatesURL={#AppURL}
DefaultDirName={pf}\{#AppPublisher}\{#AppName}
DefaultGroupName={#AppName}
DisableProgramGroupPage=yes
OutputBaseFilename={#AppName}
Compression=lzma
SolidCompression=yes
SourceDir= {#SourcePath}\{#AppSourceDir}
OutputDir={#SourcePath}

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "*.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "*.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "*.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "*.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "*.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "appsettings.json"; DestDir: "{app}"; Flags: ignoreversion onlyifdoesntexist

[Run]
Filename: "{app}\{#AppName}.exe"; Parameters: "install" 

[UninstallRun]
Filename: "{app}\{#AppName}.exe"; Parameters: "uninstall"
