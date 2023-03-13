module AlmostAutomated.Api.Handlers

open Services
open Falco

let healthCheck: HttpHandler = Response.withStatusCode 200 >> Response.ofEmpty

let listTemplatesHandler service listRepo deletedRepo : HttpHandler =
    fun ctx ->
        task {
            let q = Request.getQuery ctx
            let deleted = q.GetBoolean("deleted", false)
            let! result = service listRepo deletedRepo deleted
            return Response.ofJson result ctx
        }

let getTemplateHandler service getRepo deletedRepo : HttpHandler =
    fun ctx ->
        task {
            let q = Request.getQuery ctx
            let deleted = q.GetBoolean("deleted", false)
            let route = Request.getRoute ctx
            let id = route.GetInt64 "id"
            let! result = service getRepo deletedRepo id deleted

            return
                match result with
                | Ok r -> Response.ofJson r ctx
                | NotFound -> Response.withStatusCode 404 >> Response.ofEmpty <| ctx
        }

let createTemplateHandler service repo : HttpHandler =
    fun ctx ->
        let createTemplateHandler' details : HttpHandler =
            fun ctx ->
                task {
                    let! result = service repo details
                    return Response.ofJson result ctx
                }

        Request.mapJson createTemplateHandler' ctx

let deleteTemplateHandler service repo : HttpHandler =
    fun ctx ->
        task {
            let route = Request.getRoute ctx
            let id = route.GetInt64 "id"
            let! result = service repo id

            return
                match result with
                | Ok r -> Response.withStatusCode 200 >> Response.ofEmpty <| ctx
                | NotFound -> Response.withStatusCode 404 >> Response.ofEmpty <| ctx
        }
