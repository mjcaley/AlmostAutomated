module AlmostAutomated.Api.Program

open System.Data
open Handlers
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging
open Falco.Routing
open Falco.HostBuilder
open AlmostAutomated.Infrastructure.TemplateRepository

[<EntryPoint>]
let main args =

    let config =
        configuration args {
            required_json "appsettings.json"
            optional_json "appsettings.Development.json"
            add_env
        }

    let dbConnectionService (svc: IServiceCollection) =
        let host = config.GetValue("DB_HOST")
        let port = config.GetValue("DB_PORT")
        let database = config.GetValue("DB_NAME")
        let username = config.GetValue("DB_USERNAME")
        let password = config.GetValue("DB_PASSWORD")

        let connectionString =
            $"Include Error Detail=True;Host={host};Port={port};Database={database};Username={username};Password={password}"

        let dataSource = Npgsql.NpgsqlDataSource.Create(connectionString)
        svc.AddScoped<IDbConnection, Npgsql.NpgsqlConnection>(fun _ -> dataSource.OpenConnection())

    let exitCode = 0

    webHost [||] {
        logging (fun logging ->
            logging
                .ClearProviders()
                .AddSimpleConsole()
                .AddConfiguration(config)
                .SetMinimumLevel(LogLevel.Debug))

        add_service (fun service -> dbConnectionService service)

        add_service (fun svc ->
            svc.AddCors(fun opt ->
                opt.AddDefaultPolicy(fun policy -> policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader() |> ignore)))

        use_middleware (fun app -> app.UseCors())

        endpoints
            [ get "/" healthCheck
              get "/api/templates" <| listTemplatesHandler listTemplates
              get "/api/templates/{id:long}" <| getTemplateHandler getTemplateById
              post "/api/templates" <| createTemplateHandler createTemplate
              delete "/api/templates/{id:long}" <| deleteTemplateHandler deleteTemplate ]
    }

    exitCode
