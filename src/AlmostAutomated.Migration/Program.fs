open AlmostAutomated.Migration.MigrationRunner
open Microsoft.Extensions.Configuration
open System.IO

[<EntryPoint>]
let main args =
    let config =
        ConfigurationBuilder()
            .AddCommandLine(args)
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile("appsettings.Development.json", true, true)
            .AddJsonFile("appsettings.Production.json", true, true)
            .AddEnvironmentVariables()
            .Build()

    let host = config.GetValue "DB_HOST"
    let port = config.GetValue "DB_PORT"
    let database = config.GetValue "DB_NAME"
    let username = config.GetValue "DB_USERNAME"
    let password = config.GetValue "DB_PASSWORD"

    printfn $"Connecting to PostgreSQL: Host={host}; Port={port}; Database={database}; User={username};"

    let connectionString =
        $"Include Error Detail=True;Host={host};Port={port};Database={database};Username={username};Password={password}"

    migrate connectionString
    0
