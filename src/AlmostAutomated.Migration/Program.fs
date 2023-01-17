open AlmostAutomated.Migration.MigrationRunner
open Microsoft.Extensions.Configuration

[<EntryPoint>]
let main args =
    let config = 
        ConfigurationBuilder()
            .AddCommandLine(args)
            .AddEnvironmentVariables("ALMOST_")
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.Development.json", true, true)
            .AddJsonFile($"appsettings.Production.json", true, true)
            .Build()
    let connectionString = config.GetConnectionString "Database"

    migrate connectionString
    0
