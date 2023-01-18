namespace AlmostAutomated.Api

open Microsoft.AspNetCore.Http
open System.Data
open AlmostAutomated.Api.TemplateService

module TemplateHandler =
    open Giraffe
    open AlmostAutomated.Infrastructure.TemplateRepository

    let listTemplates: HttpHandler =
        fun next ctx ->
            task {
                let dbConn = ctx.RequestServices.GetService(typeof<IDbConnection>) :?> IDbConnection

                let! result = listTemplates dbConn
                return! json result next ctx
            }
