param
(
    [Parameter(Mandatory)]
    [String]
    $SourceVersion,

    [String]
    $IsPr
)

$ErrorActionPreference = "Stop"
$PSNativeCommandUseErrorActionPreference = $true

if($IsPr.ToLower() -eq "true") {
    Write-Host "Working on a PR, Undoing the github merge with main."

    # GitHub's merge commit has the target branch first and the queued PR head second.
    $commit = git rev-parse --verify "$SourceVersion^2"

    Write-Host "Checking out queued PR commit $commit"
    git checkout --detach $commit
    git submodule update --recursive --init
    git clean -xffd
    git submodule foreach --recursive git clean -xffd
    $hash = git rev-parse HEAD

    Write-Host "##vso[task.setvariable variable=GIT_HASH;isOutput=true]$hash"
} else {
    Write-Host "Working on a branch, using current commit"
    $hash = git rev-parse HEAD
    Write-Host "##vso[task.setvariable variable=GIT_HASH;isOutput=true]$hash"
}
