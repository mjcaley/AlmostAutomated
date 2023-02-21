module AlmostAutomated.Migration.MigrationRunner

open FluentMigrator.Runner
open FluentMigrator.Runner.Initialization
open Microsoft.Extensions.DependencyInjection
open AlmostAutomated.Migration.Migrations

let migrate (connectionString: string) =
    use serviceProvider =
        ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(fun builder ->
                builder
                    .AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn((typeof<Initial>.Assembly))
                    .For.Migrations()
                |> ignore)
            .AddLogging(fun builder -> builder.AddFluentMigratorConsole() |> ignore)
            .BuildServiceProvider(false)

    use scope = serviceProvider.CreateScope()

    let runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>()
    runner.MigrateUp()
