param(
    [Parameter(Mandatory)][int] $RootProcessId,
    [Parameter(Mandatory)][string] $OutputDirectory,
    [Parameter(Mandatory)][string] $DotNetPath
)

$ErrorActionPreference = 'Stop'
New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null

function Invoke-DiagnosticTool {
    param([string] $FilePath, [string[]] $Arguments, [string] $LogPath, [int] $TimeoutSeconds = 60)

    $startInfo = [System.Diagnostics.ProcessStartInfo]::new($FilePath)
    $startInfo.UseShellExecute = $false
    $startInfo.RedirectStandardOutput = $true
    $startInfo.RedirectStandardError = $true
    foreach ($argument in $Arguments) {
        $startInfo.ArgumentList.Add($argument)
    }
    $process = [System.Diagnostics.Process]::new()
    $process.StartInfo = $startInfo
    try {
        if (-not $process.Start()) {
            throw "Could not start $FilePath"
        }
        # Drain both pipes concurrently, including when a diagnostic tool stalls.
        $stdout = $process.StandardOutput.ReadToEndAsync()
        $stderr = $process.StandardError.ReadToEndAsync()
        if (-not $process.WaitForExit($TimeoutSeconds * 1000)) {
            $process.Kill($true)
            $process.WaitForExit()
            Add-Content "$OutputDirectory/collector-errors.txt" "$FilePath timed out after $TimeoutSeconds seconds"
        }
        Set-Content $LogPath ($stdout.GetAwaiter().GetResult() + $stderr.GetAwaiter().GetResult())
        if ($process.ExitCode -ne 0) {
            Add-Content "$OutputDirectory/collector-errors.txt" "$FilePath exited with $($process.ExitCode); see $LogPath"
        }
    } finally {
        $process.Dispose()
    }
}

$allProcesses = @(Get-CimInstance Win32_Process)
$allProcesses | Select-Object ProcessId, ParentProcessId, Name, CreationDate, ExecutablePath, CommandLine |
    ConvertTo-Json -Depth 4 | Set-Content "$OutputDirectory/processes.json"
$ids = [System.Collections.Generic.HashSet[int]]::new()
[void] $ids.Add($RootProcessId)
do {
    $added = $false
    foreach ($entry in $allProcesses) {
        if ($ids.Contains([int] $entry.ParentProcessId) -and $ids.Add([int] $entry.ProcessId)) {
            $added = $true
        }
    }
} while ($added)
$targets = @($allProcesses | Where-Object {
    $ids.Contains([int] $_.ProcessId) -and $_.Name -in @('dotnet.exe', 'MSBuild.exe')
})
$targets | Select-Object ProcessId, ParentProcessId, CommandLine |
    Format-List | Out-String -Width 4096 | Set-Content "$OutputDirectory/dump-targets.txt"

for ($snapshot = 1; $snapshot -le 2; $snapshot++) {
    foreach ($entry in $targets) {
        $targetId = [int] $entry.ProcessId
        try {
            $process = Get-Process -Id $targetId -ErrorAction Stop
            $process.Threads | Select-Object Id, ThreadState, TotalProcessorTime, @{
                Name = 'WaitReason'
                Expression = { if ($_.ThreadState -eq 'Wait') { $_.WaitReason } }
            } |
                Format-Table | Out-String -Width 4096 |
                Set-Content "$OutputDirectory/threads-$targetId-$snapshot.txt"
        } catch {
            Add-Content "$OutputDirectory/collector-errors.txt" "Thread snapshot $snapshot, PID ${targetId}: $_"
        }
        try {
            Invoke-DiagnosticTool -FilePath $Env:REMOTE_BUILD_PROCDUMP `
                -Arguments @('-accepteula', '-ma', "$targetId", "$OutputDirectory/process-$targetId-$snapshot.dmp") `
                -LogPath "$OutputDirectory/procdump-$targetId-$snapshot.log" -TimeoutSeconds 60
        } catch {
            Add-Content "$OutputDirectory/collector-errors.txt" "Snapshot $snapshot, PID ${targetId}: $_"
        }
    }
    if ($snapshot -eq 1) {
        Start-Sleep -Seconds 15
    }
}

Invoke-DiagnosticTool -FilePath $DotNetPath -Arguments @('--info') -LogPath "$OutputDirectory/dotnet-info.txt"
Invoke-DiagnosticTool -FilePath $DotNetPath -Arguments @('workload', 'list') -LogPath "$OutputDirectory/workloads.txt"

$Env:DOTNET_ROOT = Split-Path $DotNetPath
$Env:DOTNET_ROLL_FORWARD = 'Major'
foreach ($dump in Get-ChildItem "$OutputDirectory/*.dmp") {
    Invoke-DiagnosticTool -FilePath $Env:REMOTE_BUILD_DOTNET_DUMP `
        -Arguments @('analyze', $dump.FullName, '-c', 'clrthreads', '-c', 'clrstack -all', '-c', 'exit') `
        -LogPath "$($dump.FullName).stacks.txt" -TimeoutSeconds 60
}

if (Test-Path "$OutputDirectory/collector-errors.txt") {
    Get-Content "$OutputDirectory/collector-errors.txt" | Write-Warning
    exit 1
}
