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

# Claim that the tests timed out before we start
Set-Content -Path "$GithubFailureCommentFile" -Value "Tests timed out"

$macTest = @("dontlink", "introspection", "linksdk", "linkall", "monotouch-test")
foreach ($t in $macTest) {
  $testName = "exec-$t"
  Write-Host "Execution test $testName"
  make -d -C $testsPath $testName -f packaged-macos-tests.mk
  if ($LastExitCode -eq 0) {
    Write-Host "$t succeeded"
  } else {
    Write-Host "$t failed with error $LastExitCode"
    $failures.Add($t)
  }
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
      $sb.AppendLine("* $test``: Failed")
    }
    $sb.AppendLine("</details>")
    Set-Content -Path $TestSummaryPath -Value $sb.ToString()
  }
  Write-Host "TestSummary written to $TestSummaryPath"
}

# Generate HTML report
if ($HtmlReportPath -ne "") {
  $reportDir = Split-Path -Path $HtmlReportPath -Parent
  if (-not (Test-Path -Path $reportDir)) {
    New-Item -ItemType Directory -Path $reportDir -Force | Out-Null
  }
  $htmlSb = [System.Text.StringBuilder]::new()
  $htmlSb.AppendLine("<!DOCTYPE html>")
  $htmlSb.AppendLine("<html>")
  $htmlSb.AppendLine("<head><title>macOS Test Results - $StatusContext</title>")
  $htmlSb.AppendLine("<style>")
  $htmlSb.AppendLine("body { font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Helvetica, Arial, sans-serif; margin: 40px; }")
  $htmlSb.AppendLine("table { border-collapse: collapse; width: 100%; max-width: 800px; }")
  $htmlSb.AppendLine("th, td { border: 1px solid #ddd; padding: 12px 16px; text-align: left; }")
  $htmlSb.AppendLine("th { background-color: #f6f8fa; font-weight: 600; }")
  $htmlSb.AppendLine(".passed { color: #1a7f37; font-weight: 600; }")
  $htmlSb.AppendLine(".failed { color: #cf222e; font-weight: 600; }")
  $htmlSb.AppendLine("h1 { border-bottom: 1px solid #d0d7de; padding-bottom: 8px; }")
  $htmlSb.AppendLine(".summary { margin: 16px 0; padding: 12px; border-radius: 6px; }")
  $htmlSb.AppendLine(".summary.pass { background-color: #dafbe1; }")
  $htmlSb.AppendLine(".summary.fail { background-color: #ffebe9; }")
  $htmlSb.AppendLine("</style>")
  $htmlSb.AppendLine("</head>")
  $htmlSb.AppendLine("<body>")
  $htmlSb.AppendLine("<h1>macOS Test Results - $StatusContext</h1>")
  if ($failedCount -eq 0) {
    $htmlSb.AppendLine("<div class=`"summary pass`">&#x2705; All $passedCount tests passed.</div>")
  } else {
    $htmlSb.AppendLine("<div class=`"summary fail`">&#x274C; $failedCount tests failed, $passedCount tests passed.</div>")
  }
  $htmlSb.AppendLine("<table>")
  $htmlSb.AppendLine("<tr><th>Test Suite</th><th>Result</th></tr>")
  foreach ($t in $macTest) {
    if ($failures.Contains($t)) {
      $htmlSb.AppendLine("<tr><td>$t</td><td class=`"failed`">Failed</td></tr>")
    } else {
      $htmlSb.AppendLine("<tr><td>$t</td><td class=`"passed`">Passed</td></tr>")
    }
  }
  $htmlSb.AppendLine("</table>")
  $htmlSb.AppendLine("</body></html>")
  Set-Content -Path $HtmlReportPath -Value $htmlSb.ToString()
  Write-Host "HTML report written to $HtmlReportPath"
}

# Set TESTS_JOBSTATUS output variable
if ($failures.Count -ne 0) {
  Write-Host "##vso[task.setvariable variable=TESTS_JOBSTATUS;isOutput=true]Failed"
  exit 1
} else {
  Write-Host "##vso[task.setvariable variable=TESTS_JOBSTATUS;isOutput=true]Succeeded"
  exit 0
}
