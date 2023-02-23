[CmdletBinding()]
param (
    [Parameter()]
    [string]
    $Tag = "latest"
)

Push-Location $PSScriptRoot/..

nerdctl build --namespace k8s.io --tag "almost-api:$Tag" -f .\src\AlmostAutomated.Api\Dockerfile .

Pop-Location
