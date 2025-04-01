$Env:PATH = "$(Build.SourcesDirectory)\$Env:BUILD_REPOSITORY_TITLE\tests\dotnet\Windows\bin\dotnet;$env:PATH"
$Env:DOTNET = "$(Build.SourcesDirectory)\$Env:BUILD_REPOSITORY_TITLE\tests\dotnet\Windows\bin\dotnet\dotnet.exe"
& $(Build.SourcesDirectory)\$Env:BUILD_REPOSITORY_TITLE\tests\dotnet\Windows\bin\dotnet\dotnet.exe `
    test `
    "$(Build.SourcesDirectory)/$Env:BUILD_REPOSITORY_TITLE/tests/dotnet/UnitTests/DotNetUnitTests.csproj" `
    --filter Category=Windows `
    --verbosity quiet `
    --settings $(Build.SourcesDirectory)/$Env:BUILD_REPOSITORY_TITLE/tests/dotnet/Windows/config.runsettings `
    "--results-directory:$(Build.SourcesDirectory$Env:BUILD_REPOSITORY_TITLE/jenkins-results/" `
    "--logger:console;verbosity=detailed" `
    "--logger:trx;LogFileName=$(Build.SourcesDirectory)/$Env:BUILD_REPOSITORY_TITLE/jenkins-results/windows-dotnet-tests.trx" `
    "--logger:html;LogFileName=$(Build.SourcesDirectory)/$Env:BUILD_REPOSITORY_TITLE/jenkins-results/windows-dotnet-tests.html" `
    "-bl:$(Build.SourcesDirectory)/$Env:BUILD_REPOSITORY_TITLE/tests/dotnet/Windows/run-dotnet-tests.binlog"
