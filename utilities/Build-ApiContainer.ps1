Push-Location $PSScriptRoot/..

nerdctl build --namespace k8s.io --tag almost-api:1.0 -f .\src\AlmostAutomated.Api\Dockerfile .

Pop-Location
