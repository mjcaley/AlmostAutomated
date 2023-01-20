namespace AlmostAutomated.Api

open System.Data
open Handlers
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging
open Falco.Routing
open Falco.HostBuilder
open AlmostAutomated.Infrastructure.TemplateRepository

module Program =
    let config =
        configuration [||] {
            required_json "appsettings.json"
            optional_json "appsettings.Development.json"
        }

    let dbConnectionService (connectionString: string) (svc: IServiceCollection) =
        let dataSource = Npgsql.NpgsqlDataSource.Create(connectionString)
        svc.AddScoped<IDbConnection, Npgsql.NpgsqlConnection>(fun _ -> dataSource.OpenConnection())


    [<EntryPoint>]
    let main _ =
        let exitCode = 0

        webHost [||] {
            logging (fun logging -> logging.ClearProviders().AddSimpleConsole().AddConfiguration(config))

            add_service (dbConnectionService <| config.GetConnectionString "Database")

            endpoints
                [ get "/api/templates" <| listTemplatesHandler listTemplates
                  get "/api/template/{id:long}" <| getTemplateHandler getTemplateById
                  post "/api/template" <| createTemplateHandler createTemplate
                  delete "/api/template/{id:long}" <| deleteTemplateHandler deleteTemplate ]
        }

        exitCode
