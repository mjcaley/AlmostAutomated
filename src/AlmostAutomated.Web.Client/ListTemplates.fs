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
            attr.``class`` "button is-primary"
            router.HRef NewTemplate
            "New"
        }

        section {
            attr.``class`` "section"

            div {
                attr.``class`` "container"

                for template in model.Templates do
                    div {
                        attr.id $"template-id-{template.Id}"
                        attr.``class`` "card"

                        div {
                            attr.``class`` "card-content"

                            div {
                                attr.``class`` "title"
                                template.Title
                            }

                            div {
                                attr.``class`` "content"
                                template.Description
                            }
                        }

                        div {
                            attr.``class`` "card-footer"

                            div {
                                attr.``class`` "field is-grouped"

                                div {
                                    attr.``class`` "card-footer-item control"

                                    a {
                                        attr.``class`` "button is-primary"
                                        router.HRef <| EditTemplate template.Id
                                        "Edit"
                                    }
                                }

                                div {
                                    attr.``class`` "card-footer-item control"

                                    button {
                                        attr.``class`` "button is-danger"
                                        on.click (fun _ -> dispatch (DeleteTemplate template.Id))
                                        text "Delete"
                                    }
                                }
                            }
                        }
                    }
            }
        }

    }
