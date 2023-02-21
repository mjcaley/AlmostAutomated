Push-Location $PSScriptRoot/..

nerdctl build --namespace k8s.io --tag almost-migration:1.0 -f .\src\AlmostAutomated.Migration\Dockerfile .

Pop-Location
