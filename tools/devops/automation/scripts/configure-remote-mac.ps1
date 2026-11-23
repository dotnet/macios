$ErrorActionPreference = 'Stop'

foreach ($name in @('MAC_AGENT_IP', 'MAC_AGENT_USER', 'MAC_AGENT_REMOTE_NUGET_CONFIG_FILE', 'XMA_PASSWORD_FILE')) {
    if ([string]::IsNullOrWhiteSpace([Environment]::GetEnvironmentVariable($name))) {
        throw "The $name environment variable must be set to configure the remote Mac."
    }
}
if (!(Test-Path -LiteralPath $Env:XMA_PASSWORD_FILE -PathType Leaf)) {
    throw "The remote Mac password file '$Env:XMA_PASSWORD_FILE' does not exist."
}

$repository = Join-Path $Env:BUILD_SOURCESDIRECTORY $Env:BUILD_REPOSITORY_TITLE
$dotnet = Join-Path $repository 'tests/dotnet/Windows/bin/dotnet/dotnet.exe'
$project = Join-Path $repository 'tests/dotnet/MySimpleApp/iOS/MySimpleApp.csproj'
$binlog = Join-Path $repository 'tests/dotnet/Windows/configure-mac.binlog'

# MSBuild unescapes percent sequences in property values.
$Env:ServerAddress = $Env:MAC_AGENT_IP.Replace('%', '%25')
$Env:ServerUser = $Env:MAC_AGENT_USER.Replace('%', '%25')
$Env:ServerPasswordFile = $Env:XMA_PASSWORD_FILE.Replace('%', '%25')
$Env:RemoteNuGetConfigFile = $Env:MAC_AGENT_REMOTE_NUGET_CONFIG_FILE.Replace('%', '%25')
$Env:OverwriteRemoteNuGetConfig = 'true'
$Env:ServerPassword = $null
$Env:XMA_PASSWORD = $null

Push-Location (Split-Path -Parent $project)
try {
    $remoteDotNetRoot = @(& $dotnet msbuild $project -t:ConfigureMac -getProperty:RemoteDotNetRoot -nologo -verbosity:quiet "-bl:$binlog")
    if ($LASTEXITCODE -ne 0) {
        throw "ConfigureMac failed with exit code $LASTEXITCODE."
    }
} finally {
    Pop-Location
}
if ($remoteDotNetRoot.Count -ne 1 -or !$remoteDotNetRoot[0].StartsWith('/')) {
    throw "ConfigureMac did not return an absolute remote .NET directory."
}

$escapedPath = $remoteDotNetRoot[0].Replace('%', '%AZP25').Replace("`r", '%0D').Replace("`n", '%0A')
Write-Host "##vso[task.setvariable variable=MAC_AGENT_DOTNET_ROOT]$escapedPath"
