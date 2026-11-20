function New-RemotePasswordFile {
    param (
        [Parameter(Mandatory = $true)]
        [AllowEmptyString()]
        [string] $Password,
        [string] $Directory = [System.IO.Path]::GetTempPath()
    )

    if ([string]::IsNullOrEmpty($Password)) {
        throw "The remote Mac password is empty."
    }

    $credentialDirectory = Join-Path $Directory "macios-remote-auth-$([guid]::NewGuid().ToString('N'))"
    $passwordFile = Join-Path $credentialDirectory "password"
    try {
        New-Item -ItemType Directory -Path $credentialDirectory -ErrorAction Stop | Out-Null
        if ($IsWindows) {
            $identity = [System.Security.Principal.WindowsIdentity]::GetCurrent()
            try {
                $sid = $identity.User.Value
            } finally {
                $identity.Dispose()
            }
            & icacls.exe $credentialDirectory /inheritance:r /grant:r "*$($sid):(OI)(CI)F" | Out-Null
            if ($LASTEXITCODE -ne 0) {
                throw "Could not restrict access to the remote Mac password directory."
            }
        } else {
            & chmod 700 $credentialDirectory
            if ($LASTEXITCODE -ne 0) {
                throw "Could not restrict access to the remote Mac password directory."
            }
        }

        [System.IO.File]::WriteAllText($passwordFile, $Password, [System.Text.UTF8Encoding]::new($false))
        if (!$IsWindows) {
            & chmod 600 $passwordFile
            if ($LASTEXITCODE -ne 0) {
                throw "Could not restrict access to the remote Mac password file."
            }
        }
        return $passwordFile
    } catch {
        if (Test-Path -LiteralPath $passwordFile) {
            Remove-Item -LiteralPath $passwordFile -Force
        }
        if (Test-Path -LiteralPath $credentialDirectory) {
            Remove-Item -LiteralPath $credentialDirectory -Force
        }
        throw
    }
}

function Remove-RemotePasswordFile {
    param (
        [Parameter(Mandatory = $true)]
        [string] $Path
    )

    if (Test-Path -LiteralPath $Path) {
        Remove-Item -LiteralPath $Path -Force -ErrorAction Stop
    }
    $directory = Split-Path -Parent $Path
    if (Test-Path -LiteralPath $directory) {
        Remove-Item -LiteralPath $directory -Force -ErrorAction Stop
    }
}
