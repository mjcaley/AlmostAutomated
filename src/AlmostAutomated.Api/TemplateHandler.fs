module AlmostAutomated.Api.TemplateHandler

open Giraffe
open System.Data
open AlmostAutomated.Api.TemplateService


let listTemplates: HttpHandler =
    fun next ctx ->
        task {
            let dbConn = ctx.RequestServices.GetService(typeof<IDbConnection>) :?> IDbConnection

            let! result = listTemplates dbConn
            return! json result next ctx
        }
