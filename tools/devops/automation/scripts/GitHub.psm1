<#
    .SYNOPSIS
        Returns the target url to be used when setting the status. The target url allows users to get back to the CI event that updated the status.
#>
function Get-TargetUrl {
    $targetUrl = $Env:SYSTEM_TEAMFOUNDATIONCOLLECTIONURI + "$Env:SYSTEM_TEAMPROJECT/_build/index?buildId=$Env:BUILD_BUILDID&view=ms.vss-test-web.test-result-details"
    return $targetUrl
}

<#
    .SYNOPSIS
        Returns the url to the Html Report index page stored in xamarin-storage.
#>
function Get-XamarinStorageIndexUrl {
    param (
        [Parameter(Mandatory)]
        [String]
        $Path
    )

    return "http://xamarin-storage/$Path/jenkins-results/tests/index.html"
}

<#
    .SYNOPSIS
        Sets a new status in github for the current build.
    .DESCRIPTION

    .PARAMETER Status
        The status value to be set in GitHub. The available values are:

        * error
        * failure
        * pending
        * success

        If the wrong value is passed a validation error with be thrown.

    .PARAMETER Description
        The description that will be added with the status update. This allows us to add a human readable string
        to understand why the status was updated.
    
    .PARAMETER Context
        The context to be used. A status can contain several contexts. The context must be passed to associate
        the status with a specific event.

    .EXAMPLE
        Set-GitHubStatus -Status "error" -Description "Not enough free space in the host." -Context "VSTS iOS device tests."

    .NOTES
        This cmdlet depends on the following environment variables. If one or more of the variables is missing an
        InvalidOperationException will be thrown:

        * SYSTEM_TEAMFOUNDATIONCOLLECTIONURI: The uri of the vsts collection. Needed to be able to calculate the target url.
        * SYSTEM_TEAMPROJECT: The team project executing the build. Needed to be able to calculate the target url.
        * BUILD_BUILDID: The current build id. Needed to be able to calculate the target url.
        * BUILD_REVISION: The revision of the current build. Needed to know the commit whose status to change.
        * GITHUB_TOKEN: OAuth or PAT token to interact with the GitHub API.
#>
function Set-GitHubStatus {
    param
    (
        [Parameter(Mandatory)]
        [String]
        [ValidateScript({
            $("error", "failure", "pending", "success").Contains($_) #validate that the status is in the range of valid values
        })]
        $Status,

        [Parameter(Mandatory)]
        [String]
        $Description,

        [Parameter(Mandatory)]
        [String]
        $Context,

        [String]
        $TargetUrl
    )

    # assert that all the env vars that are needed are present, else we do have an error
    $envVars = @{
        "SYSTEM_TEAMFOUNDATIONCOLLECTIONURI" = $Env:SYSTEM_TEAMFOUNDATIONCOLLECTIONURI;
        "SYSTEM_TEAMPROJECT" = $Env:SYSTEM_TEAMPROJECT;
        "BUILD_BUILDID" = $Env:BUILD_BUILDID;
        "BUILD_REVISION" = $Env:BUILD_REVISION;
        "GITHUB_TOKEN" = $Env:GITHUB_TOKEN;
    }

    foreach ($key in $envVars.Keys) {
        if (-not($envVars[$key])) {
            Write-Debug "Enviroment varible missing $key"
            throw [System.InvalidOperationException]::new("Environment variable missing: $key")
        }
    }

    # use the GitHub API to set the status for the given commit
    $detailsUrl = ""
    if ($TargetUrl) {
        $detailsUrl = $TargetUrl
    } else {
        $detailsUrl = Get-TargetUrl
    }
    $payload= @{
        state = $Status
        target_url = $detailsUrl
        description = $Description
        context = $Context
    }
    $url = "https://api.github.com/repos/xamarin/xamarin-macios/statuses/$Env:BUILD_REVISION"

    $headers = @{
        Authorization = ("token {0}" -f $Env:GITHUB_TOKEN)
    }

    return Invoke-RestMethod -Uri $url -Headers $headers -Method "POST" -Body ($payload | ConvertTo-json) -ContentType 'application/json'
}

<#
    .SYNOPSIS
        Add a new comment for the commit on GitHub.

    .PARAMETER Header
        The header to be used in the comment.

    .PARAMETER Description
        A show description to be added in the comment, this will show as a short version of the comment on GitHub.
    
    .PARAMETER Message
        A longer string that contains the full comment message. Will be shown when the comment is expanded.

    .PARAMETER Emoji
        Optional string representing and emoji to be used in the comments header.

    .EXAMPLE
        New-GitHubComment -Header "Tests failed catastrophically" -Emoji ":fire:" -Description "Not enough free space in the host."

    .NOTES
        This cmdlet depends on the following environment variables. If one or more of the variables is missing an
        InvalidOperationException will be thrown:

        * SYSTEM_TEAMFOUNDATIONCOLLECTIONURI: The uri of the vsts collection. Needed to be able to calculate the target url.
        * SYSTEM_TEAMPROJECT: The team project executing the build. Needed to be able to calculate the target url.
        * BUILD_BUILDID: The current build id. Needed to be able to calculate the target url.
        * BUILD_REVISION: The revision of the current build. Needed to know the commit whose status to change.
        * GITHUB_TOKEN: OAuth or PAT token to interact with the GitHub API.
#>
function New-GitHubComment {
    param
    (
        [Parameter(Mandatory)]
        [String]
        $Header,
        
        [Parameter(Mandatory)]
        [String]
        $Description,

        [String]
        $Message,

        [String]
        $Emoji #optionally use an emoji
    )

    # assert that all the env vars that are needed are present, else we do have an error
    $envVars = @{
        "SYSTEM_TEAMFOUNDATIONCOLLECTIONURI" = $Env:SYSTEM_TEAMFOUNDATIONCOLLECTIONURI;
        "SYSTEM_TEAMPROJECT" = $Env:SYSTEM_TEAMPROJECT;
        "BUILD_DEFINITIONNAME" = $Env:BUILD_DEFINITIONNAME;
        "BUILD_REVISION" = $Env:BUILD_REVISION;
        "GITHUB_TOKEN" = $Env:GITHUB_TOKEN;
    }

    foreach ($key in $envVars.Keys) {
        if (-not($envVars[$key])) {
            Write-Debug "Enviroment varible missing $key"
            throw [System.InvalidOperationException]::new("Environment variable missing: $key")
        }
    }

    $targetUrl = Get-TargetUrl
    # build the message, which will be sent to github, users can use markdown
    $msg = [System.Text.StringBuilder]::new()
    $msg.AppendLine("### $Emoji $Header $Emoji")
    $msg.AppendLine()
    $msg.AppendLine($Description)
    if ($Message) { # only if message is not null or empty
        $msg.AppendLine()
        $msg.AppendLine($Message)
    }
    $msg.AppendLine()
    $msg.AppendLine("[Pipeline]($targetUrl) on Agent $Env:TESTS_BOT") # Env:TESTS_BOT is added by the pipeline as a variable coming from the execute tests job

    $url = "https://api.github.com/repos/xamarin/xamarin-macios/commits/$Env:BUILD_REVISION/comments"
    $payload = @{
        body = $msg.ToString()
    }

    $headers = @{
        Authorization = ("token {0}" -f $Env:GITHUB_TOKEN)
    }

    $request = Invoke-RestMethod -Uri $url -Headers $headers -Method "POST" -Body ($payload | ConvertTo-Json) -ContentType 'application/json'
    Write-Host $request
    return $request
}

<#
    .SYNOPSIS
        Add a new comment that contains the result summaries of the test run.

    .PARAMETER Header
        The header to be used in the comment.

    .PARAMETER Description
        A show description to be added in the comment, this will show as a short version of the comment on GitHub.
    
    .PARAMETER Message
        A longer string that contains the full comment message. Will be shown when the comment is expanded.

    .PARAMETER Emoji
        Optional string representing and emoji to be used in the comments header.

    .EXAMPLE
        New-GitHubComment -Header "Tests failed catastrophically" -Emoji ":fire:" -Description "Not enough free space in the host."

    .NOTES
        This cmdlet depends on the following environment variables. If one or more of the variables is missing an
        InvalidOperationException will be thrown:

        * SYSTEM_TEAMFOUNDATIONCOLLECTIONURI: The uri of the vsts collection. Needed to be able to calculate the target url.
        * SYSTEM_TEAMPROJECT: The team project executing the build. Needed to be able to calculate the target url.
        * BUILD_BUILDID: The current build id. Needed to be able to calculate the target url.
        * BUILD_REVISION: The revision of the current build. Needed to know the commit whose status to change.
        * GITHUB_TOKEN: OAuth or PAT token to interact with the GitHub API.
#>
function New-GitHubCommentFromFile {
    param (

        [Parameter(Mandatory)]
        [String]
        $Header,
        
        [Parameter(Mandatory)]
        [String]
        $Description,

        [Parameter(Mandatory)]
        [String]
        [ValidateScript({
            Test-Path -Path $_ -PathType Leaf 
        })]
        $Path,

        [String]
        $Emoji #optionally use an emoji
    )

    # read the file, create a message and use the New-GithubComment function
    $msg = [System.Text.StringBuilder]::new()
    foreach ($line in Get-Content -Path $Path)
    {
        $msg.AppendLine($line)
    }
    return New-GithubComment -Header $Header -Description $Description -Message $msg.ToString() -Emoji $Emoji
}

<#
    .SYNOPSIS
        Test if the current job is successful or not.
#>
function Test-JobSuccess {

    param (
        [Parameter(Mandatory)]
        [String]
        $Status
    )

    # return if the status is one of the failure ones
    return $Status -eq "Succeeded"
}

<# 
    .SYNOPSIS
        Add a new comment that contains the summaries to the Html Report as well as set the status accordingly.

    .PARAMETER Context
        The context to be used to link the status and the device test run in the GitHub status API.

    .PARAMETER TestSummaryPath
        The path to the generated test summary.

    .EXAMPLE
        New-GitHubSummaryComment -Context "$Env:CONTEXT" -TestSummaryPath "$Env:SYSTEM_DEFAULTWORKINGDIRECTORY/xamarin/xamarin-macios/tests/TestSummary.md"
    .NOTES
        This cmdlet depends on the following environment variables. If one or more of the variables is missing an
        InvalidOperationException will be thrown:

        * SYSTEM_TEAMFOUNDATIONCOLLECTIONURI: The uri of the vsts collection. Needed to be able to calculate the target url.
        * SYSTEM_TEAMPROJECT: The team project executing the build. Needed to be able to calculate the target url.
        * BUILD_BUILDID: The current build id. Needed to be able to calculate the target url.
        * BUILD_REVISION: The revision of the current build. Needed to know the commit whose status to change.
        * GITHUB_TOKEN: OAuth or PAT token to interact with the GitHub API.

#>
function New-GitHubSummaryComment {
    param (
        [Parameter(Mandatory)]
        [String]
        $Context,

        [Parameter(Mandatory)]
        [String]
        $TestSummaryPath
    )

    $envVars = @{
        "SYSTEM_TEAMFOUNDATIONCOLLECTIONURI" = $Env:SYSTEM_TEAMFOUNDATIONCOLLECTIONURI;
        "SYSTEM_TEAMPROJECT" = $Env:SYSTEM_TEAMPROJECT;
        "BUILD_DEFINITIONNAME" = $Env:BUILD_DEFINITIONNAME;
        "BUILD_REVISION" = $Env:BUILD_REVISION;
        "GITHUB_TOKEN" = $Env:GITHUB_TOKEN;
    }

    foreach ($key in $envVars.Keys) {
        if (-not($envVars[$key])) {
            Write-Debug "Environment variable missing: $key"
            throw [System.InvalidOperationException]::new("Environment variable missing: $key")
        }
    }

    $vstsTargetUrl = Get-TargetUrl
    # build the links to provide extra info to the monitoring person, we need to make sure of a few things
    # 1. We do have the xamarin-storage path
    # 2. We did reach the xamarin-storage, stored in the env var XAMARIN_STORAGE_REACHED
    $headerSb = [System.Text.StringBuilder]::new()
    $headerSb.AppendLine(); # new line to start the list
    $headerSb.AppendLine("* [Azure DevOps]($vstsTargetUrl)")
    if ($Env:VSDROPS_INDEX) {
        # we did generate an index with the files in vsdrops
        $headerSb.AppendLine("* [Html Report (VSDrops)]($Env:VSDROPS_INDEX)")
    }
    $headerLinks = $headerSb.ToString()
    $request = $null

    if (-not (Test-Path $TestSummaryPath -PathType Leaf)) {
        Write-Host "No test summary found"
        Set-GitHubStatus -Status "failure" -Description "Tests failed catastrophically on $Context (no summary found)." -Context "$Context"
        $request = New-GitHubComment -Header "Tests failed catastrophically on $Context (no summary found)." -Emoji ":fire:" -Description "Result file $TestSummaryPath not found. $headerLinks"
    } else {
        if (Test-JobSuccess -Status $Env:TESTS_JOBSTATUS) {
            Set-GitHubStatus -Status "success" -Description "Device tests passed on $Context." -Context "$Context"
            $request = New-GitHubCommentFromFile -Header "Device tests passed on $Context." -Description "Device tests passed on $Context. $headerLinks"  -Emoji ":white_check_mark:" -Path $TestSummaryPath
        } else {
            Set-GitHubStatus -Status "failure" -Description "Device tests failed on $Context." -Context "$Context"
            $request = New-GitHubCommentFromFile -Header "Device tests failed on $Context" -Description "Device tests failed on $Context. $headerLinks" -Emoji ":x:" -Path $TestSummaryPath
        }
    }
    return $request
}

<# 
    .SYNOPSIS
        Get the information of a PR in GitHub.

    .PARAMETER ChangeId
        The Id whose labels we want to retrieve.
#>
function Get-GitHubPRInfo {
    param (
        [Parameter(Mandatory)]
        [String]
        $ChangeId
    )

    $envVars = @{
        "GITHUB_TOKEN" = $Env:GITHUB_TOKEN;
    }

    foreach ($key in $envVars.Keys) {
        if (-not($envVars[$key])) {
            Write-Debug "Environment variable missing: $key"
            throw [System.InvalidOperationException]::new("Environment variable missing: $key")
        }
    }

    $url = "https://api.github.com/repos/xamarin/xamarin-macios/pulls/$ChangeId"

    $headers = @{
        Authorization = ("token {0}" -f $Env:GITHUB_TOKEN)
    }

    $request = Invoke-RestMethod -Uri $url -Headers $headers -Method "POST" -ContentType 'application/json'
    Write-Host $request
    return $request
}

<#
    .SYNOPSIS
        Class used to represent a single file to be added to a gist.
#>
class GistFile
{
    [ValidateNotNullOrEmpty ()]
    [string]
    $Name
    [ValidateNotNullOrEmpty ()]
    [string]
    $Path
    [ValidateNotNullOrEmpty ()]
    [string]
    $Type

    GistFile ($Name, $Path, $Type) {
        # validate that the path does exist
        if (Test-Path -Path $Path -PathType Leaf) {
            $this.Path = $Path
        } else {
            throw [System.InvalidOperationException]::new("Path could not be found: $Path")
        }
        $this.Name = $Name
        $this.Type = $Type
     }

    [hashtable] ConvertToHashTable () {
        # ugly workaround to get decent new lines
        $file= [System.Text.StringBuilder]::new()
        foreach ($line in Get-Content -Path $this.Path)
        {
            $file.AppendLine($line)
        }

        return @{
            content = $file.ToString()
            filename = $this.Name;
            language = $this.Type;
        }
    }
}

<# 
    .SYNOPSIS
        Creates a new gist that will contain the given collection of files and returns the urlobject defintion, this
        is usefull when the 'using' statement generates problems.
#>
function New-GistObjectDefinition {
    param (

        [ValidateNotNullOrEmpty ()]
        [string]
        $Name, 

        [ValidateNotNullOrEmpty ()]
        [string]
        $Path,

        [ValidateNotNullOrEmpty ()]
        [string]
        $Type
    )
    return [GistFile]::new($Name, $Path, $Type)
}

<# 
    .SYNOPSIS
        Creates a new gist that will contain the given collection of files and returns the url
#>
function New-GistWithFiles {
    param (

        [ValidateNotNullOrEmpty ()]
        [string]
        $Description, 

        [Parameter(Mandatory)]
        [GistFile[]]
        $Files,

        [switch]
        $IsPublic=$false # default to false, better save than sorry
    )

    $envVars = @{
        "GITHUB_TOKEN" = $Env:GITHUB_TOKEN;
    }

    foreach ($key in $envVars.Keys) {
        if (-not($envVars[$key])) {
            Write-Debug "Environment variable missing: $key"
            throw [System.InvalidOperationException]::new("Environment variable missing: $key")
        }
    }

    # create the hashtable that will contain all the information of all types
    $payload = @{
        description = $Description;
        files = @{}; # each file is the name of the file + the hashtable of the data to be used
    }

    # switchs are converted to {\"IsPresent\"=>true} in json :/ and the ternary operator might not be in all machines
    if ($IsPublic) {
        $payload["public"] = $true
    } else {
        $payload["public"] = $false
    }

    foreach ($g in $Files) {
        # add the file using its name + the hashtable that is used by GitHub
        $payload["files"].Add($g.Name, $g.ConvertToHashTable())
    }

    $url = "https://api.github.com/gists"
    $payloadJson = $payload | ConvertTo-Json
    Write-Host "Url is $url"
    Write-Host "Payload is $payloadJson"

    $headers = @{
        Accept = "application/vnd.github.v3+json";
        Authorization = ("token {0}" -f $Env:GITHUB_TOKEN);
    } 

    $request = Invoke-RestMethod -Uri $url -Headers $headers -Method "POST" -Body $payloadJson -ContentType 'application/json'
    Write-Host $request
    return $request.html_url
}

# module exports, any other functions are private and should not be used outside the module.
Export-ModuleMember -Function Set-GitHubStatus
Export-ModuleMember -Function New-GitHubComment
Export-ModuleMember -Function New-GitHubCommentFromFile
Export-ModuleMember -Function New-GitHubSummaryComment 
Export-ModuleMember -Function Test-JobSuccess 
Export-ModuleMember -Function Get-GitHubPRInfo
Export-ModuleMember -Function New-GistWithFiles 
Export-ModuleMember -Function New-GistObjectDefinition 
