module AlmostAutomated.Api.Handlers

open AlmostAutomated.Core.Entities
open Giraffe
open System.Threading.Tasks
open System.Data
open Services

let internal jsonHandler service dbFactory : HttpHandler =
    fun next ctx ->
        task {
            let! dbConn = dbFactory ()
            let! result = service dbConn
            return! json result next ctx
        }

let listTemplatesHandler dbFactory : HttpHandler =
    jsonHandler listTemplatesService dbFactory

let getTemplateHandler dbFactory id : HttpHandler =
    fun next ctx ->
        task {
            let! dbConn = dbFactory ()
            let! result = getTemplateService dbConn id
            return! json result next ctx
        }

let createTemplateHandler dbFactory : HttpHandler =
    fun next ctx ->
        task {
            let! details = ctx.BindJsonAsync<TemplateDetails.Insert'> ()
            let! dbConn = dbFactory ()
            let! result = createTemplateService dbConn details
            return! json result next ctx
        }

let deleteTemplateHandler dbFactory id : HttpHandler =
    fun next ctx ->
        task {
            let! dbConn = dbFactory ()
            let! result = deleteTemplateService dbConn id
            return! json result next ctx
        }
