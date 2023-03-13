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
open Services


[<EntryPoint>]
let main args =
    let config =
        configuration args {
            required_json "appsettings.json"
            optional_json "appsettings.Development.json"
            add_env
        }

    let connectionString =
        let host = config.GetValue("DB_HOST")
        let port = config.GetValue("DB_PORT")
        let database = config.GetValue("DB_NAME")
        let username = config.GetValue("DB_USERNAME")
        let password = config.GetValue("DB_PASSWORD")

        $"Include Error Detail=True;Host={host};Port={port};Database={database};Username={username};Password={password}"

    let exitCode = 0

    webHost [||] {
        logging (fun logging ->
            logging
                .ClearProviders()
                .AddSimpleConsole()
                .AddConfiguration(config)
                .SetMinimumLevel(LogLevel.Debug))

        add_service (fun svc ->
            svc.AddCors(fun opt ->
                opt.AddDefaultPolicy(fun policy -> policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader() |> ignore)))

        use_middleware (fun app -> app.UseCors())

        endpoints
            [ get "/" healthCheck
              get "/api/templates" <| listTemplatesHandler listTemplatesService (listTemplates connectionString) (listDeletedTemplates connectionString)
              get "/api/templates/{id:long}" <| getTemplateHandler getTemplateService (getTemplateById connectionString) (getDeletedTemplateById connectionString)
              post "/api/templates" <| createTemplateHandler createTemplateService (createTemplate connectionString)
              delete "/api/templates/{id:long}" <| deleteTemplateHandler deleteTemplateService (deleteTemplate connectionString) ]
    }

    exitCode
