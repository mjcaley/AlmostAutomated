[CmdletBinding()]
param (
    [Parameter()]
    [string]
    $Tag = "latest"
)

Push-Location $PSScriptRoot/..

nerdctl build --namespace k8s.io --tag "almost-migration:$Tag" -f .\src\AlmostAutomated.Migration\Dockerfile .

Pop-Location
