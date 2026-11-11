Import-Module "$Env:SYSTEM_DEFAULTWORKINGDIRECTORY/$Env:BUILD_REPOSITORY_TITLE/tools/devops/automation/scripts/MaciosCI.psd1"

# Zip up all the binlogs into one file
$collectScript = "$Env:MAC_AGENT_BUILD_SOURCESDIRECTORY/$Env:BUILD_REPOSITORY_TITLE/tests/dotnet/Windows/collect-binlogs.sh"
$quotedCollectScript = "'" + $collectScript.Replace("'", "'\''") + "'"
Invoke-SshCommand `
  -RemoteHost "$Env:MAC_AGENT_IP" `
  -RemoteUserName "$Env:MAC_AGENT_USER" `
  -- `
  "$quotedCollectScript"

$targetDirectory = Join-Path "$Env:BUILD_ARTIFACTSTAGINGDIRECTORY" "windows-binlogs"
New-Item -Path "$targetDirectory" -ItemType "directory" -Force

# Check file existence
Invoke-SshCommand `
  -RemoteHost "$Env:MAC_AGENT_IP" `
  -RemoteUserName "$Env:MAC_AGENT_USER" `
  -- `
  ls -la "remote_build_testing/windows-remote-logs.zip"

# Copy the zip from the remote Mac to this machine
Invoke-SshDownload `
  -RemoteHost "$Env:MAC_AGENT_IP" `
  -RemoteUserName "$Env:MAC_AGENT_USER" `
  -Source "remote_build_testing/windows-remote-logs.zip" `
  -Target (Join-Path "$targetDirectory" "windows-remote-logs.zip")

$reportDirectory = Join-Path (Join-Path $Env:BUILD_SOURCESDIRECTORY $Env:BUILD_REPOSITORY_TITLE) "jenkins-results"
New-Item -Path "$reportDirectory" -ItemType "directory" -Force
Copy-Item -LiteralPath (Join-Path "$targetDirectory" "windows-remote-logs.zip") -Destination "$reportDirectory"
