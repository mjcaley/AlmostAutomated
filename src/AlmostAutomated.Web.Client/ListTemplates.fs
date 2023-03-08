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
            httpClient.GetFromJsonAsync<TemplateDTO[]>("/api/templates")

        let successTemplatesCmd templates = List.ofArray templates |> GotTemplates

        let cmd = Cmd.OfTask.either getTemplates () successTemplatesCmd Error
        model, cmd
    | GotTemplates templates -> { model with Templates = templates }, Cmd.none
    | DeleteTemplate id ->
        let deleteTemplate () =
            httpClient.DeleteAsync($"/api/templates/{id}")

        let successDeleteCmd _ =
            model.Templates |> List.filter (fun t -> t.Id <> id) |> GotTemplates

        let cmd = Cmd.OfTask.either deleteTemplate () successDeleteCmd Error
        model, cmd
    | Error exn ->
        eprintfn "Error: %s" exn.Message
        model, Cmd.none

let view (router: Router<Page, 'model, 'msg>) model dispatch =
    div {
        div {
            attr.``class`` "flex justify-between"

            h2 {
                attr.``class`` "text-xl"
                "Templates"
            }

            a {
                attr.``class`` "bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
                router.HRef NewTemplate
                "New"
            }
        }

        div {
            //attr.``class`` "section"

            div {
                attr.``class`` "grid gap-4 grid-flow-col"

                for template in model.Templates do
                    div {
                        attr.id $"template-id-{template.Id}"
                        attr.``class`` "w-60 p-4 shadow rounded"

                        div {
                            attr.``class`` "m-2"

                            p {
                                attr.``class`` "font-bold truncate"
                                template.Title
                            }

                            p {
                                attr.``class`` "h-16 truncate"
                                template.Description
                            }
                        }

                        div {
                            attr.``class`` "bottom-0"

                            a {
                                attr.``class`` "bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 mx-1 rounded"
                                router.HRef <| EditTemplate template.Id
                                "Edit"
                            }

                            a {
                                attr.``class`` "bg-red-500 hover:bg-red-700 text-white font-bold py-2 px-4 mx-1 rounded"
                                on.click (fun _ -> dispatch (DeleteTemplate template.Id))
                                text "Delete"
                            }
                        }
                    }
            }
        }
    }
