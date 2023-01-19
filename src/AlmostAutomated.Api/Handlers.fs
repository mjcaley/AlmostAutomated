﻿module AlmostAutomated.Api.Handlers

open Services
open Falco
open System.Data

let listTemplatesHandler: HttpHandler =
    Services.inject<IDbConnection> (fun dbConn ctx ->
        task {
            let! result = listTemplatesService dbConn
            return Response.ofJson result ctx
        })

let internal taskResponse a' : HttpHandler =
    fun ctx -> task { return Response.withStatusCode a' ctx }



let getTemplateHandler: HttpHandler =
    Services.inject<IDbConnection> (fun dbConn ctx ->
        task {
            let route = Request.getRoute ctx
            let id = route.GetInt64 "id"
            let! result = getTemplateService dbConn id

            return!
                match result with
                | Some r -> Response.ofJson result ctx
                | None -> taskResponse 404 ctx
        })

let createTemplateHandler: HttpHandler =
    Services.inject<IDbConnection> (fun dbConn ctx ->
        let createTemplateHandler' details : HttpHandler =
            fun ctx ->
                task {
                    let! result = createTemplateService dbConn details
                    return Response.ofJson result ctx
                }

        Request.mapJson createTemplateHandler' ctx)

let deleteTemplateHandler: HttpHandler =
    Services.inject<IDbConnection> (fun dbConn ctx ->
        task {
            let route = Request.getRoute ctx
            let id = route.GetInt64 "id"
            let! result = deleteTemplateService dbConn id
            
            return
                match result with
                | Some r -> Response.withStatusCode 200
                | None -> Response.withStatusCode 404
        })
