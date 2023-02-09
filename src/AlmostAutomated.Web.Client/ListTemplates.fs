module AlmostAutomated.Web.Client.ListTemplates

open System.Net.Http
open System.Net.Http.Json
open Elmish
open Bolero
open Bolero.Html
open AlmostAutomated.Core.DTO
open Routes

type Model =
    { Templates: TemplateDTO[]
      ErrorMessage: string }

let initModel () =
    { Templates = Array.empty
      ErrorMessage = "" }

type Message =
    | Init
    | GetTemplates
    | GotTemplates of templates: TemplateDTO[]
    | Error of exn


let init _ =
    { Templates = Array.empty
      ErrorMessage = "" },
    Cmd.none

let update (httpClient: HttpClient) message model =
    match message with
    | Init -> model, Cmd.ofMsg GetTemplates
    | GetTemplates ->
        let getTemplates () =
            httpClient.GetFromJsonAsync<TemplateDTO[]>("http://localhost:5268/api/templates")

        let cmd = Cmd.OfTask.either getTemplates () GotTemplates Error
        model, cmd
    | GotTemplates templates -> { model with Templates = templates }, Cmd.none
    | Error exn -> 
        eprintfn "Error: %s" exn.Message
        model, Cmd.none

let view (router: Router<Page, 'model, 'msg>) model _ =
    div {
        attr.style "padding: 1;"

        h1 { "Templates" }
        p { text $"Error: {model.ErrorMessage}" }

        ul {
            for template in model.Templates do
                div {
                    p { text $"Id: {string template.Id}" }
                    p { text $"Title: {template.Title}" }
                    p { text $"Description: {template.Description}" }

                    a {
                        router.HRef <| EditTemplate template.Id
                        "Edit"
                    }
                }
        }
    }
