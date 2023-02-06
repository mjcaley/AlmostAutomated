module AlmostAutomated.Web.Client.Templates

open System.Net.Http
open System.Net.Http.Json
open Microsoft.AspNetCore.Components
open Elmish
open Bolero
open Bolero.Html
open Bolero.Remoting
open AlmostAutomated.Core.DTO


type Model =
    {
        Templates: TemplateDTO[]
        ErrorMessage: string
    }

let initModel = { Templates=Array.empty; ErrorMessage="" }

type Message =
    | GetTemplates
    | GotTemplates of templates: TemplateDTO[]
    | Error of exn

let update (httpClient: HttpClient) message model =
    match message with
    | GetTemplates -> 
        let getTemplates () = httpClient.GetFromJsonAsync<TemplateDTO[]>("http://localhost:5268/api/templates")
        let cmd = Cmd.OfTask.either getTemplates () GotTemplates Error
        model, cmd
    | GotTemplates templates ->
        { model with Templates=templates }, Cmd.none
    | Error exn -> { model with ErrorMessage=exn.Message }, Cmd.none

let view model _ =
    div {
        attr.style "border: solid;"

        h1 { "Templates" }
        p { text $"Error: {model.ErrorMessage}" }
        ul {
            for template in model.Templates do
                li { 
                    concat {
                        "Id:"
                        string template.Id
                        ", "
                        "Title: "
                        template.Title
                        ", "
                        "Description: "
                        template.Description
                    }
                }
        }
    }

type TemplatesComponent() =
    inherit ProgramComponent<Model, Message>()
    
    [<Inject>]
    member val HttpClient = Unchecked.defaultof<HttpClient> with get, set

    override this.Program =
        let update = update this.HttpClient
        Program.mkProgram (fun _ -> initModel, Cmd.ofMsg GetTemplates) update view
