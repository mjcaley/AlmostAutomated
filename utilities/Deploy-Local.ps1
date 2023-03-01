[CmdletBinding()]
param (
    [Parameter()]
    [string]
    $DBPassword = "password",

    [Parameter()]
    [switch]
    $DryRun
)

Push-Location $PSScriptRoot/../infrastructure

if ($DryRun)
{
    pulumi --stack dev preview
}
else {
    pulumi --stack dev up
}

Pop-Location
