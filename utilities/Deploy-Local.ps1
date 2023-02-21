[CmdletBinding()]
param (
    [Parameter()]
    [string]
    $DBPassword = "password",

    [Parameter()]
    [string]
    $DBAdminPassword = "postgres-password",

    [Parameter()]
    [switch]
    $DryRun
)

Push-Location $PSScriptRoot/../infrastructure

$args = "upgrade",
    "--install",
    "almost-automated-release",
    (Resolve-Path "almost-automated"),
    "--namespace",
    "almost-automated",
    "--set",
    "global.postgresql.auth.postgresPassword=$DBAdminPassword",
    "--set",
    "global.postgresql.auth.password=$DBPassword"

if ($DryRun)
{
    $args += "--dry-run"
}

helm $args

Pop-Location
