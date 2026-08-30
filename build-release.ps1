param(
    [string]$Configuration = "Release",
    [string]$ServiceCredentialsPath = ""
)

$ErrorActionPreference = "Stop"
$projectRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$solutionPath = Join-Path $projectRoot "NanoTwitchLeafs.sln"
$outputPath = Join-Path $projectRoot "bin\$Configuration"
$artifactPath = Join-Path $projectRoot "artifacts\NanoTwitchLeafs-4.0.0"

if ($projectRoot.Length -gt 80) {
    throw "Der Quellordner-Pfad ist zu lang ($($projectRoot.Length) Zeichen). Bitte das Paket direkt nach C:\NTL entpacken und dort BUILD_NTL.cmd starten."
}

function Find-MSBuild {
    $command = Get-Command msbuild.exe -ErrorAction SilentlyContinue
    if ($command) { return $command.Source }

    $vswhere = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"
    if (Test-Path $vswhere) {
        $path = & $vswhere -latest -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe | Select-Object -First 1
        if ($path) { return $path }
    }

    throw "MSBuild wurde nicht gefunden. Visual Studio 2022 Build Tools mit '.NET-Desktopbuildtools' installieren."
}

$msbuild = Find-MSBuild

Write-Host "NuGet-Pakete werden über MSBuild wiederhergestellt ..."
& $msbuild $solutionPath /m:1 /t:Restore /p:RestorePackagesConfig=true
if ($LASTEXITCODE -ne 0) { throw "Paketwiederherstellung fehlgeschlagen." }

$restoredFiles = @(
    "packages\Microsoft.Extensions.DependencyInjection.Abstractions.10.0.0-preview.3.25171.5\lib\net462\Microsoft.Extensions.DependencyInjection.Abstractions.dll",
    "packages\SocketIoClientDotNet.0.9.13\lib\net45\SocketIoClientDotNet.dll"
)

foreach ($restoredFile in $restoredFiles) {
    $restoredPath = Join-Path $projectRoot $restoredFile
    if (-not (Test-Path $restoredPath)) {
        throw "Paketwiederherstellung unvollständig: $restoredFile fehlt. Bitte den Quellordner in einen kürzeren Pfad wie C:\NTL entpacken und erneut bauen."
    }
}

Write-Host "NanoTwitchLeafs wird gebaut ..."
& $msbuild $solutionPath /m:1 /t:Rebuild "/p:Configuration=$Configuration" "/p:Platform=Any CPU" /p:RestoreIgnoreFailedSources=true
if ($LASTEXITCODE -ne 0) { throw "MSBuild fehlgeschlagen." }

if (Test-Path $artifactPath) { Remove-Item $artifactPath -Recurse -Force }
New-Item -ItemType Directory -Path $artifactPath -Force | Out-Null
Copy-Item (Join-Path $outputPath "*") $artifactPath -Recurse -Force

$requiredFiles = @(
    "NanoTwitchLeafs.exe",
    "NanoTwitchLeafs.exe.config",
    "System.ValueTuple.dll"
)

foreach ($requiredFile in $requiredFiles) {
    $requiredPath = Join-Path $artifactPath $requiredFile
    if (-not (Test-Path $requiredPath)) {
        throw "Build unvollständig: $requiredFile fehlt im Ausgabepaket."
    }
}

if ($ServiceCredentialsPath) {
    if (-not (Test-Path $ServiceCredentialsPath)) { throw "Die angegebene ServiceCredentials-Datei wurde nicht gefunden." }
    Copy-Item $ServiceCredentialsPath (Join-Path $artifactPath "ServiceCredentials") -Force
} else {
    Write-Warning "Keine ServiceCredentials-Datei angegeben. Die EXE wird gebaut, Twitch/Streamlabs-Anmeldung benötigt diese Datei aber weiterhin."
}

$zipPath = "$artifactPath.zip"
if (Test-Path $zipPath) { Remove-Item $zipPath -Force }
Compress-Archive -Path (Join-Path $artifactPath "*") -DestinationPath $zipPath -CompressionLevel Optimal
Write-Host "Fertig: $zipPath"
