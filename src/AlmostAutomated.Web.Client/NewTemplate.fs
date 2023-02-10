module NewTemplate

open System.Net.Http
open System.Net.Http.Json
open Elmish
open Bolero
open Bolero.Html
open AlmostAutomated.Core.DTO
open Routes

type Model = TemplateDTO

let initModel () =
    { Id = -1;
      Title = "";
      Description = ""}

type Message =
    | New
    | Save
    | Edit
    | Error of exn
    | SetTitle of string
    | SetDescription of string


let init _ = initModel (), Cmd.none

let update (httpClient: HttpClient) message model =
    match message with
    | New -> model, Cmd.none
    | Edit -> model, Cmd.none
    | Save ->
        let postTemplate () =
            httpClient.PostAsJsonAsync<TemplateDTO>($"http://localhost:5268/api/template", model)

        let updateId _ =
            Edit

        let cmd = Cmd.OfTask.either postTemplate () updateId Error
        model, Cmd.none
    | Error exn ->
        eprintfn "Error: %s" exn.Message
        model, Cmd.none
    | SetTitle t -> { model with Title = t }, Cmd.ofMsg Edit
    | SetDescription d -> { model with Description = d }, Cmd.ofMsg Edit

let view model dispatch =
    div {
        attr.style "padding: 1;"

        h1 { $"Template {model.Id}" }

        input {
            attr.``type`` "input"
            bind.input.string model.Title (dispatch << SetTitle)
        }
        input {
            attr.``type`` "input"
            bind.input.string model.Description (dispatch << SetDescription)
        }
        button {
            
            text "Save"
        }
    }
