module AlmostAutomated.Web.Client.Templates

open System.Net.Http
open System.Net.Http.Json
open Microsoft.AspNetCore.Components
open Elmish
open Bolero
open Bolero.Html
open AlmostAutomated.Core.DTO

type Page =
    | [<EndPoint "/">] Home
    | [<EndPoint "/templates">] Templates
    | [<EndPoint "/runs">] Runs

type Model =
    {
        Page: Page
        Templates: TemplateDTO[]
        ErrorMessage: string
    }

type Message =
    | SetPage of Page
    | GetTemplates
    | GotTemplates of templates: TemplateDTO[]
    | Error of exn

let router = Router.infer SetPage (fun model -> model.Page)

let init _ = { Page=Home; Templates=Array.empty; ErrorMessage="" }, Cmd.ofMsg GetTemplates

let update (httpClient: HttpClient) message model =
    match message with
    | SetPage page -> { model with Page=page }, Cmd.none
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
        Program.mkProgram init update view |> Program.withRouter router
