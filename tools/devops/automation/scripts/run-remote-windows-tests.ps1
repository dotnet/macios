if ("$Env:BUILD_REPOSITORY_TITLE" -eq "") {
    $remoteUrl = & git remote get-url --push origin
    $repoTitle = [System.IO.Path]::GetFilename($remoteUrl)
    $Env:BUILD_REPOSITORY_TITLE = $repoTitle
}
if ("$Env:DOTNET" -eq "") {
    $Env:DOTNET = "$Env:BUILD_SOURCESDIRECTORY\$Env:BUILD_REPOSITORY_TITLE\tests\dotnet\Windows\bin\dotnet\dotnet.exe"
}
$Env:ServerAddress = $Env:MAC_AGENT_IP
$Env:ServerUser = $Env:MAC_AGENT_USER
$Env:_DotNetRootRemoteDirectory = "/Users/$Env:MAC_AGENT_USER/Library/Caches/maui/PairToMac/Sdks/dotnet/"

if ("$Env:TESTFILTER" -eq "") {
    $Env:TESTFILTER = "Category=RemoteWindows|Category=RemoteWindowsInclusive"
}

$createdPasswordFile = $null
try {
    if ([string]::IsNullOrEmpty($Env:XMA_PASSWORD_FILE)) {
        $Env:XMA_PASSWORD_FILE = $Env:ServerPasswordFile
    }
    if ([string]::IsNullOrEmpty($Env:XMA_PASSWORD_FILE) -and ![string]::IsNullOrEmpty($Env:XMA_PASSWORD)) {
        . "$PSScriptRoot/remote-password-file.ps1"
        $createdPasswordFile = New-RemotePasswordFile -Password $Env:XMA_PASSWORD
        $Env:XMA_PASSWORD_FILE = $createdPasswordFile
    }
    $Env:ServerPasswordFile = $Env:XMA_PASSWORD_FILE
    $Env:ServerPassword = $null
    $Env:XMA_PASSWORD = $null

    & $Env:DOTNET `
        test `
        "$Env:BUILD_SOURCESDIRECTORY/$Env:BUILD_REPOSITORY_TITLE/tests/dotnet/UnitTests/DotNetUnitTests.csproj" `
        --filter "$Env:TESTFILTER" `
        --verbosity quiet `
        --settings $Env:BUILD_SOURCESDIRECTORY/$Env:BUILD_REPOSITORY_TITLE/tests/dotnet/Windows/config.runsettings `
        "--results-directory:$Env:BUILD_SOURCESDIRECTORY/$Env:BUILD_REPOSITORY_TITLE/jenkins-results/windows-remote-tests/" `
        "--logger:console;verbosity=detailed" `
        "--logger:trx;LogFileName=$Env:BUILD_SOURCESDIRECTORY/$Env:BUILD_REPOSITORY_TITLE/jenkins-results/windows-remote-dotnet-tests.trx" `
        "--logger:html;LogFileName=$Env:BUILD_SOURCESDIRECTORY/$Env:BUILD_REPOSITORY_TITLE/jenkins-results/windows-remote-dotnet-tests.html" `
        "-bl:$Env:BUILD_SOURCESDIRECTORY/$Env:BUILD_REPOSITORY_TITLE/tests/dotnet/Windows/windows-remote-dotnet-tests.binlog"
    $exitCode = $LASTEXITCODE
} finally {
    if ($createdPasswordFile) {
        Remove-RemotePasswordFile -Path $createdPasswordFile
        $Env:XMA_PASSWORD_FILE = $null
        $Env:ServerPasswordFile = $null
    }
}

exit $exitCode
