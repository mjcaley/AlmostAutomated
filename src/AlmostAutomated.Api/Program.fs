namespace AlmostAutomated.Api

open Giraffe
open Swashbuckle.AspNetCore
open AlmostAutomated.Infrastructure.DataAccess
open System.Data

#nowarn "20"
open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.HttpsPolicy
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging

module Program =
    let helloEndpoint : HttpHandler =
            fun  next ctx -> 
            task {
                return! json "Hello" next ctx
            }

    let webApp = 
        subRoute "/api" (
            choose [
                route "/templates" >=> GET >=> helloEndpoint])

    let exitCode = 0

    [<EntryPoint>]
    let main args =
        let builder = WebApplication.CreateBuilder(args)

        builder.Configuration.AddEnvironmentVariables("ALMOST_")

        let dbConnectionString = builder.Configuration.GetConnectionString("Database")
        use dataSource = openDataSource dbConnectionString
        builder.Services.AddTransient<IDbConnection>(fun _ -> dataSource.OpenConnection())

        builder.Services.AddControllers()
        
        let app = builder.Build()

        app.UseHttpsRedirection()

        app.UseGiraffe webApp

        app.UseAuthorization()
        app.MapControllers()
        
        app.UseSwagger()
        app.UseSwaggerUI()

        app.Run()

        exitCode
