if ("$Env:BUILD_REPOSITORY_TITLE" -eq "") {
    $remoteUrl = & git remote get-url --push origin
    $repoTitle = [System.IO.Path]::GetFilename($remoteUrl)
    $Env:BUILD_REPOSITORY_TITLE = $repoTitle
}
if ("$Env:DOTNET" -eq "") {
    $Env:DOTNET = "$Env:BUILD_SOURCESDIRECTORY\$Env:BUILD_REPOSITORY_TITLE\tests\dotnet\Windows\bin\dotnet\dotnet.exe"
    $Env:PATH = "$Env:BUILD_SOURCESDIRECTORY\$Env:BUILD_REPOSITORY_TITLE\tests\dotnet\Windows\bin\dotnet;$Env:PATH"
}

# The Arcade SDK imported by the Xamarin.MacDev submodule overrides
# NuGetPackageRoot in CI, breaking the netstandard2.0 build.
# Pass the correct path explicitly.
$Env:NuGetPackageRoot = "$Env:BUILD_SOURCESDIRECTORY/$Env:BUILD_REPOSITORY_TITLE/packages/"

# The Xamarin.MacDev submodule has its own NuGet.config, which clears all
# package sources and only adds a few of them. This means that any package
# sources we add to our own NuGet.config (such as darc feeds for unreleased
# .NET packages) aren't available when building the submodule.
# Make every project in this repository use our NuGet.config.
# This mirrors what Make.config does for the macOS build.
$Env:RestoreConfigFile = "$Env:BUILD_SOURCESDIRECTORY/$Env:BUILD_REPOSITORY_TITLE/NuGet.config"

$Env:MAC_AGENT_IP = ""
$Env:MAC_AGENT_USER = ""
$Env:XMA_PASSWORD = ""

& $Env:DOTNET `
    test `
    "$Env:BUILD_SOURCESDIRECTORY/$Env:BUILD_REPOSITORY_TITLE/tests/dotnet/UnitTests/DotNetUnitTests.csproj" `
    --filter "Category=Windows|Category=WindowsInclusive" `
    --verbosity quiet `
    --settings $Env:BUILD_SOURCESDIRECTORY/$Env:BUILD_REPOSITORY_TITLE/tests/dotnet/Windows/config.runsettings `
    "--results-directory:$Env:BUILD_SOURCESDIRECTORY/$Env:BUILD_REPOSITORY_TITLE/jenkins-results/windows-remote-tests/" `
    "--logger:console;verbosity=detailed" `
    "--logger:trx;LogFileName=$Env:BUILD_SOURCESDIRECTORY/$Env:BUILD_REPOSITORY_TITLE/jenkins-results/windows-local-dotnet-tests.trx" `
    "--logger:html;LogFileName=$Env:BUILD_SOURCESDIRECTORY/$Env:BUILD_REPOSITORY_TITLE/jenkins-results/windows-local-dotnet-tests.html" `
    "-bl:$Env:BUILD_SOURCESDIRECTORY/$Env:BUILD_REPOSITORY_TITLE/tests/dotnet/Windows/windows-local-dotnet-tests.binlog"
