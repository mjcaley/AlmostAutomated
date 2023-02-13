module AlmostAutomated.Api.Handlers

open Services
open Falco
open System.Data

let healthCheck : HttpHandler = Response.withStatusCode 200 >> Response.ofEmpty

let listTemplatesHandler repo : HttpHandler =
    Services.inject<IDbConnection>
    <| fun dbConn ctx ->
        task {
            let q = Request.getQuery ctx
            let deleted = q.GetBoolean("deleted", false)
            let! result = listTemplatesService <| repo dbConn deleted
            return Response.ofJson result ctx
        }

let getTemplateHandler repo : HttpHandler =
    Services.inject<IDbConnection>
    <| fun dbConn ctx ->
        task {
            let q = Request.getQuery ctx
            let deleted = q.GetBoolean("deleted", false)
            let route = Request.getRoute ctx
            let id = route.GetInt64 "id"
            let! result = getTemplateService <| repo dbConn id deleted

            return
                match result with
                | Ok r -> Response.ofJson r ctx
                | NotFound -> Response.withStatusCode 404 >> Response.ofEmpty <| ctx
        }

let createTemplateHandler repo : HttpHandler =
    Services.inject<IDbConnection>
    <| fun dbConn ctx ->
        let createTemplateHandler' details : HttpHandler =
            fun ctx ->
                task {

                    let! result = createTemplateService <| repo dbConn details
                    return Response.ofJson result ctx
                }

        Request.mapJson createTemplateHandler' ctx

let deleteTemplateHandler repo : HttpHandler =
    Services.inject<IDbConnection>
    <| fun dbConn ctx ->
        task {
            let route = Request.getRoute ctx
            let id = route.GetInt64 "id"
            let! result = deleteTemplateService <| repo dbConn id

            return
                match result with
                | Some r -> Response.withStatusCode 200 >> Response.ofEmpty <| ctx
                | None -> Response.withStatusCode 404 >> Response.ofEmpty <| ctx
        }
