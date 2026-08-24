<#
Configure decisions unit tests.
#>

$ScriptDir = Split-Path -parent $MyInvocation.MyCommand.Path
$ConfigureDecisionsScript = Join-Path $ScriptDir "bash/configure-decisions.sh"

function Invoke-ConfigureDecisions {
	param (
		[hashtable] $Variables
	)

	$environment = @(
		"PATH=$Env:PATH"
	)

	foreach ($entry in $Variables.GetEnumerator()) {
		$environment += "$($entry.Key)=$($entry.Value)"
	}

	$output = & env -i @environment bash $ConfigureDecisionsScript 2>&1
	if ($LASTEXITCODE -ne 0) {
		throw "configure-decisions.sh failed with exit code $LASTEXITCODE`: $output"
	}

	return $output
}

Describe 'Configure decisions' {
	It 'runs mac tests when Mac Catalyst is enabled' {
		$output = Invoke-ConfigureDecisions @{
			"CONFIGURE_PLATFORMS_DOTNET_PLATFORMS" = "MacCatalyst"
			"CONFIGURE_PLATFORMS_INCLUDE_DOTNET_MACCATALYST" = "1"
		}

		$output | Should -Contain "Setting the variable RUN_MAC_TESTS=true"
	}

	It 'does not run mac tests when only iOS is enabled' {
		$output = Invoke-ConfigureDecisions @{
			"CONFIGURE_PLATFORMS_DOTNET_PLATFORMS" = "iOS"
			"CONFIGURE_PLATFORMS_INCLUDE_DOTNET_IOS" = "1"
		}

		$output | Should -Contain "Setting the variable RUN_MAC_TESTS=false"
	}
}
