param
(
    [Parameter(Mandatory)]
    [String]
    $GithubToken,

    [Parameter(Mandatory)]
    [String]
    $RepositoryUri,

    [Parameter(Mandatory)]
    [String]
    $SourcesDirectory,

    [Parameter(Mandatory)]
    [String]
    $GithubFailureCommentFile,

    [Parameter(Mandatory)]
    [String]
    $StatusContext,

    [String]
    $TestSummaryPath = "",

    [String]
    $HtmlReportPath = ""
)

Import-Module $Env:SYSTEM_DEFAULTWORKINGDIRECTORY\$Env:BUILD_REPOSITORY_TITLE\tools\devops\automation\scripts\MaciosCI.psd1
$statuses = New-GitHubStatusesObjectFromUrl -Url "$RepositoryUri" -Token $GitHubToken

Write-Host "Found tests"
$testsPath = "$SourcesDirectory/artifacts/mac-test-package/tests"
$packageRoot = "$SourcesDirectory/artifacts/mac-test-package"
Write-Host "Tests path is $testsPath"

# Pre-flight check: verify dotnet is available
$dotnetPath = Get-Command dotnet -ErrorAction SilentlyContinue
if ($dotnetPath) {
  Write-Host "dotnet found at: $($dotnetPath.Source)"
  Write-Host "dotnet version: $(dotnet --version)"
} else {
  Write-Host "##vso[task.logissue type=error]dotnet is not on the PATH. All tests will fail."
  Write-Host "PATH: $Env:PATH"
}

# print enviroment
dir env:

# Claim that the tests timed out before we start
Set-Content -Path "$GithubFailureCommentFile" -Value "Tests timed out"

# Create a directory for per-test output
$testOutputDir = "$SourcesDirectory/mac-test-output"
if (-not (Test-Path -Path $testOutputDir)) {
  New-Item -ItemType Directory -Path $testOutputDir -Force | Out-Null
}

# Read configuration from Make.config
$makeConfig = "$packageRoot/Make.config"
$includeMac = $false
$includeMacCatalyst = $false
$dotnetTfm = ""

if (Test-Path -Path $makeConfig) {
  foreach ($line in Get-Content -Path $makeConfig) {
    if ($line -match "^INCLUDE_MAC=(.+)$") {
      $includeMac = -not [string]::IsNullOrWhiteSpace($Matches[1])
    }
    if ($line -match "^INCLUDE_MACCATALYST=(.+)$") {
      $includeMacCatalyst = -not [string]::IsNullOrWhiteSpace($Matches[1])
    }
    if ($line -match "^DOTNET_TFM=(.+)$") {
      $dotnetTfm = $Matches[1].Trim()
    }
  }
}

if ([string]::IsNullOrEmpty($dotnetTfm)) {
  Write-Host "##vso[task.logissue type=error]Could not determine DOTNET_TFM from $makeConfig"
  exit 1
}

Write-Host "INCLUDE_MAC=$includeMac, INCLUDE_MACCATALYST=$includeMacCatalyst, DOTNET_TFM=$dotnetTfm"

# Build the run-packaged-macos-tests tool
$scriptDir = "$packageRoot/scripts/run-packaged-macos-tests"
$toolDll = "$scriptDir/bin/Debug/run-packaged-macos-tests.dll"
Write-Host "Building run-packaged-macos-tests tool..."
dotnet build "$scriptDir/run-packaged-macos-tests.csproj"
if ($LASTEXITCODE -ne 0) {
  Write-Host "##vso[task.logissue type=error]Failed to build run-packaged-macos-tests tool"
  exit 1
}

# Build tool arguments
$toolArgs = @(
  "exec", $toolDll,
  "--tests-directory", $testsPath,
  "--dotnet-tfm", $dotnetTfm,
  "--test-output-dir", $testOutputDir
)

if ($includeMac) {
  $toolArgs += "--include-mac"
}
if ($includeMacCatalyst) {
  $toolArgs += "--include-maccatalyst"
}

if ($TestSummaryPath -ne "") {
  $toolArgs += @("--test-summary-path", $TestSummaryPath)
}

if ($HtmlReportPath -ne "") {
  $crashReportsDir = "$Env:HOME/Library/Logs/DiagnosticReports"
  $toolArgs += @("--html-report-path", $HtmlReportPath)
  $toolArgs += @("--title", $StatusContext)

  if (Test-Path -Path $crashReportsDir) {
    $toolArgs += @("--crash-reports-dir", $crashReportsDir)
  }

  # Construct vsdrops URI for link rewriting
  $vsdropsPrefix = $Env:VSDROPSPREFIX
  $testPrefix = $Env:MAC_TEST_PREFIX
  if ($vsdropsPrefix -and $testPrefix) {
    $buildNumber = $Env:BUILD_BUILDNUMBER
    $buildId = $Env:BUILD_BUILDID
    $jobAttempt = $Env:SYSTEM_JOBATTEMPT
    $vsdropsUri = "$vsdropsPrefix/$buildNumber/$buildId/$testPrefix-$jobAttempt/;/tests/"
    $toolArgs += @("--vsdrops-uri", $vsdropsUri)
  }
}

Write-Host "Running: dotnet $($toolArgs -join ' ')"
& dotnet @toolArgs
$testExitCode = $LASTEXITCODE

if ($testExitCode -ne 0) {
  # We failed, so write to the comment file why we failed.
  Set-Content -Path "$GithubFailureCommentFile" -Value "Tests failed (see test output for details)"
  Write-Host "##vso[task.setvariable variable=TESTS_JOBSTATUS;isOutput=true]Failed"
  exit 1
} else {
  # We succeeded, so remove the failure comment file.
  Remove-Item -Path "$GithubFailureCommentFile"
  Write-Host "##vso[task.setvariable variable=TESTS_JOBSTATUS;isOutput=true]Succeeded"
  exit 0
}
