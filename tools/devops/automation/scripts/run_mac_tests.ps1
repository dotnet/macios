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
Write-Host "Tests path is $testsPath"

# print enviroment
dir env:

[System.Collections.Generic.List[string]]$failures = @()
# Track [FAIL] lines per test suite for the GitHub comment
$failLinesByTest = @{}

# Claim that the tests timed out before we start
Set-Content -Path "$GithubFailureCommentFile" -Value "Tests timed out"

# Create a directory for per-test output
$testOutputDir = "$SourcesDirectory/mac-test-output"
if (-not (Test-Path -Path $testOutputDir)) {
  New-Item -ItemType Directory -Path $testOutputDir -Force | Out-Null
}

$macTest = @("dontlink", "introspection", "linksdk", "linkall", "monotouch-test")

foreach ($t in $macTest) {
  $testName = "exec-$t"
  Write-Host "Execution test $testName"

  $stdoutFile = "$testOutputDir/$t-stdout.txt"
  $stderrFile = "$testOutputDir/$t-stderr.txt"

  # Run the test and capture stdout/stderr separately
  $proc = Start-Process -FilePath "make" `
    -ArgumentList "-d", "-C", $testsPath, $testName, "-f", "packaged-macos-tests.mk" `
    -RedirectStandardOutput $stdoutFile `
    -RedirectStandardError $stderrFile `
    -NoNewWindow -Wait -PassThru

  if ($proc.ExitCode -eq 0) {
    Write-Host "$t succeeded"
  } else {
    Write-Host "$t failed with error $($proc.ExitCode)"
    $failures.Add($t)
  }

  # Extract [FAIL] lines from stdout/stderr
  $failLines = @()
  foreach ($logFile in @($stdoutFile, $stderrFile)) {
    if (Test-Path -Path $logFile) {
      $failLines += @(Get-Content -Path $logFile | ForEach-Object {
        $idx = $_.IndexOf("[FAIL]")
        if ($idx -ge 0) { $_.Substring($idx + 6).TrimStart() }
      } | Where-Object { $_ })
    }
  }
  $failLinesByTest[$t] = $failLines
}
if ($failures.Count -ne 0) {
  # post status and comment in the build
  $failedTestsStr = [string]::Join(",",$failures)
  # build message
  $msg = [System.Text.StringBuilder]::new()
  $msg.AppendLine("Failed tests are:")
  $msg.AppendLine("")
  foreach ($test in $failures)
  {
      $msg.AppendLine("* $test")
  }

  # We failed, so write to the comment file why we failed.
  Set-Content -Path "$GithubFailureCommentFile" -Value "$msg"

  $passedCount = $macTest.Count - $failures.Count
  $failedCount = $failures.Count
} else {
  # We succeeded, so remove the failure comment file.
  Remove-Item -Path "$GithubFailureCommentFile"

  $passedCount = $macTest.Count
  $failedCount = 0
}

# Generate TestSummary.md
if ($TestSummaryPath -ne "") {
  $summaryDir = Split-Path -Path $TestSummaryPath -Parent
  if (-not (Test-Path -Path $summaryDir)) {
    New-Item -ItemType Directory -Path $summaryDir -Force | Out-Null
  }
  if ($failedCount -eq 0) {
    Set-Content -Path $TestSummaryPath -Value "# :tada: All $passedCount tests passed :tada:"
  } else {
    $sb = [System.Text.StringBuilder]::new()
    $sb.AppendLine("# Test results")
    $sb.AppendLine("<details>")
    $sb.AppendLine("<summary>$failedCount tests failed, $passedCount tests passed.</summary>")
    $sb.AppendLine("")
    $sb.AppendLine("## Failed tests")
    $sb.AppendLine("")
    foreach ($test in $failures) {
      $sb.AppendLine("* ${test}: Failed")
      # Show first 3 [FAIL] lines from the test output
      $testFailLines = $failLinesByTest[$test]
      if ($testFailLines -and $testFailLines.Count -gt 0) {
        $maxShow = [Math]::Min($testFailLines.Count, 3)
        for ($i = 0; $i -lt $maxShow; $i++) {
          $sb.AppendLine("    * ``$($testFailLines[$i])``")
        }
        if ($testFailLines.Count -gt 3) {
          $sb.AppendLine("    * ... and $($testFailLines.Count - 3) more failures")
        }
      }
    }
    $sb.AppendLine("</details>")
    Set-Content -Path $TestSummaryPath -Value $sb.ToString()
  }
  Write-Host "TestSummary written to $TestSummaryPath"
}

# Generate HTML report using C# tool
if ($HtmlReportPath -ne "") {
  $crashReportsDir = "$Env:HOME/Library/Logs/DiagnosticReports"

  # Build the report generator tool (uses the same pattern as run-with-timeout)
  make -C $testsPath -f packaged-macos-tests.mk build-mac-test-report-generator
  $toolDll = [System.IO.Path]::GetFullPath("$testsPath/../scripts/mac-test-report-generator/bin/Debug/mac-test-report-generator.dll")

  $toolArgs = @("exec", $toolDll, "--title", $StatusContext, "--output", $HtmlReportPath, "--test-output-dir", $testOutputDir)
  foreach ($t in $macTest) {
    $result = if ($failures.Contains($t)) { "fail" } else { "pass" }
    $toolArgs += @("--test", "$t`:$result")
  }
  if (Test-Path -Path $crashReportsDir) {
    $toolArgs += @("--crash-reports-dir", $crashReportsDir)
  }

  # Construct vsdrops URI for link rewriting (same pattern as create-windows-html-report)
  $vsdropsPrefix = $Env:VSDROPSPREFIX
  $testPrefix = $Env:MAC_TEST_PREFIX
  if ($vsdropsPrefix -and $testPrefix) {
    $buildNumber = $Env:BUILD_BUILDNUMBER
    $buildId = $Env:BUILD_BUILDID
    $jobAttempt = $Env:SYSTEM_JOBATTEMPT
    $vsdropsUri = "$vsdropsPrefix/$buildNumber/$buildId/$testPrefix-$jobAttempt/;/tests/"
    $toolArgs += @("--vsdrops-uri", $vsdropsUri)
  }

  Write-Host "Running HTML report generator: dotnet $($toolArgs -join ' ')"
  & dotnet @toolArgs
  if ($LASTEXITCODE -ne 0) {
    Write-Host "Warning: HTML report generation failed with exit code $LASTEXITCODE"
  }
}

# Set TESTS_JOBSTATUS output variable
if ($failures.Count -ne 0) {
  Write-Host "##vso[task.setvariable variable=TESTS_JOBSTATUS;isOutput=true]Failed"
  exit 1
} else {
  Write-Host "##vso[task.setvariable variable=TESTS_JOBSTATUS;isOutput=true]Succeeded"
  exit 0
}
