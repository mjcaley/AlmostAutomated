[CmdletBinding()]
param (
    [Parameter()]
    [string]
    $Tag = "latest"
)

Push-Location $PSScriptRoot/..

nerdctl build --namespace k8s.io --tag "almost-web:$Tag" -f .\src\AlmostAutomated.Web.Client\Dockerfile .

Pop-Location
