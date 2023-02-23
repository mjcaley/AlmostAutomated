[CmdletBinding()]
param (
    [Parameter()]
    [string]
    $Tag = "latest"
)

Push-Location $PSScriptRoot

./Build-MigrationContainer.ps1 -Tag $Tag
./Build-ApiContainer.ps1 -Tag $Tag

Pop-Location
