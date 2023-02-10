module ListTemplates

open System.Net.Http
open System.Net.Http.Json
open Elmish
open Bolero
open Bolero.Html
open AlmostAutomated.Core.DTO
open Routes

type Model =
    { Templates: TemplateDTO list
      ErrorMessage: string }

let initModel () =
    { Templates = list.Empty
      ErrorMessage = "" }

type Message =
    | Init
    | GetTemplates
    | GotTemplates of templates: TemplateDTO list
    | DeleteTemplate of id: int64
    | Error of exn


let init _ =
    { Templates = list.Empty
      ErrorMessage = "" },
    Cmd.none

let update (httpClient: HttpClient) message model =
    match message with
    | Init -> model, Cmd.ofMsg GetTemplates
    | GetTemplates ->
        let getTemplates () =
            httpClient.GetFromJsonAsync<TemplateDTO[]>("http://localhost:5268/api/templates")

        let successTemplatesCmd templates = List.ofArray templates |> GotTemplates

        let cmd = Cmd.OfTask.either getTemplates () successTemplatesCmd Error
        model, cmd
    | GotTemplates templates -> { model with Templates = templates }, Cmd.none
    | DeleteTemplate id ->
        let deleteTemplate () =
            httpClient.DeleteAsync($"http://localhost:5268/api/templates/{id}")

        let successDeleteCmd _ =
            model.Templates |> List.filter (fun t -> t.Id <> id) |> GotTemplates

        let cmd = Cmd.OfTask.either deleteTemplate () successDeleteCmd Error
        model, cmd
    | Error exn ->
        eprintfn "Error: %s" exn.Message
        model, Cmd.none

let view (router: Router<Page, 'model, 'msg>) model dispatch =
    div {
        attr.style "padding: 1;"

        h1 { "Templates" }
        p { text $"Error: {model.ErrorMessage}" }

        a {
            router.HRef NewTemplate
            "New"
        }

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

                    button {
                        on.click (fun _ -> dispatch (DeleteTemplate template.Id))
                        text "Delete"
                    }
                }
        }
    }
