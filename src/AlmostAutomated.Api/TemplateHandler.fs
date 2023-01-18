namespace AlmostAutomated.Api

open Microsoft.AspNetCore.Http
open System.Data

module TemplateHandler =
    open Giraffe
    open AlmostAutomated.Infrastructure.TemplateRepository

    let listTemplates : HttpHandler =
        fun next ctx ->
           task {
                let dbConn = ctx.RequestServices.GetService(typeof<IDbConnection>) :?> IDbConnection
                
                let! templates = getAll dbConn
                let templatesList = List.ofSeq templates
                return! json templatesList next ctx
           } 
