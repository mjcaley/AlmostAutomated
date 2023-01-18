namespace AlmostAutomated.Api

open Giraffe
open AlmostAutomated.Infrastructure.DataAccess
open System.Data
open Handlers
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging

module Program =
    let getDbConnection (dataSource: Npgsql.NpgsqlDataSource) =
        fun () ->
            task {
                let! dbConn = dataSource.OpenConnectionAsync()
                return dbConn :> IDbConnection
            }

    let webApp dataSource =
        subRoute
            "/api"
            (choose
                [ GET
                  >=> choose
                          [ route "/templates" >=> (listTemplatesHandler <| getDbConnection dataSource)
                            routef "/template/%i" (getTemplateHandler <| getDbConnection dataSource) ] ])

    let exitCode = 0

    [<EntryPoint>]
    let main args =
        let builder = WebApplication.CreateBuilder(args)

        builder.Configuration.AddEnvironmentVariables("ALMOST_")

        let dbConnectionString = builder.Configuration.GetConnectionString("Database")
        use dataSource = openDataSource dbConnectionString

        builder
            .Services
            .AddTransient<IDbConnection>(fun _ -> dataSource.OpenConnection())
            .AddGiraffe()
            .AddSwaggerGen()
            .AddControllers()
        |> ignore

        let app = builder.Build()

        app.UseHttpsRedirection()

        app.UseGiraffe <| webApp dataSource

        app.UseAuthorization()
        app.MapControllers()

        app.UseSwagger()
        app.UseSwaggerUI()

        app.Run()

        exitCode
