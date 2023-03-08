module NewTemplate

open System.Net.Http
open System.Net.Http.Json
open Elmish
open Bolero
open Bolero.Html

type Model =
    { Id: int64 option
      Title: string
      Description: string }

let initModel () =
    { Id = None
      Title = ""
      Description = "" }

type Message =
    | Update of Model
    | Edit
    | SaveNew
    | SaveExisting
    | Error of exn
    | SetTitle of string
    | SetDescription of string


let init _ = initModel (), Cmd.none


let update (httpClient: HttpClient) message (model: Model) =
    match message with
    | Edit -> model, Cmd.none
    | SaveNew ->
        let postTemplate () =
            task {
                let! response =
                    httpClient.PostAsJsonAsync(
                        $"/api/templates",
                        {| Title = model.Title
                           Description = model.Description |}
                    )

                if response.IsSuccessStatusCode then
                    let! body = response.Content.ReadFromJsonAsync<int64>()
                    return { model with Id = Some body }
                else
                    return model
            }

        let cmd =
            Cmd.OfTask.either postTemplate () (fun newModel -> Update <| newModel) Error

        model, cmd
    | SaveExisting -> model, Cmd.none
    | Update template -> template, Cmd.ofMsg Edit
    | Error exn ->
        eprintfn "Error: %s" exn.Message
        model, Cmd.none
    | SetTitle t -> { model with Title = t }, Cmd.ofMsg Edit
    | SetDescription d -> { model with Description = d }, Cmd.ofMsg Edit

let view model dispatch =
    form {
        attr.``class`` "justify-center"

        div {
            label {
                attr.``for`` "title"
                "Title"
            }
            input {
                attr.id "title"
                attr.``class`` "w-1/3 border border-gray-300 rounded block"
                bind.input.string model.Title (dispatch << SetTitle)
            }
        }
        
        div {
            label {
                attr.``for`` "description"
                "Description"
            }
            input {
                attr.id "description"
                attr.``class`` "w-1/3 border border-gray-300 rounded block"
                bind.input.string model.Description (dispatch << SetDescription)
            }
        }

        match model.Id with
        | Some _ ->
            button {
                attr.``class`` "bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 m-2 rounded"
                on.click (fun _ -> dispatch SaveExisting)
                "Save"
            }
        | None ->
            button {
                attr.``class`` "bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 my-2 rounded"
                on.click (fun _ -> dispatch SaveNew)
                "Save"
            }
    }
