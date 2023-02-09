module AlmostAutomated.Web.Client.Main2

open System.Net.Http
open Microsoft.AspNetCore.Components
open Elmish
open Bolero
open Bolero.Html
open Routes


type Message =
    | SetPage of Page
    | ListTemplatesMsg of ListTemplates.Message

type Model =
    { Page: Page
      ListTemplatesState: ListTemplates.Model }

let router = Router.infer SetPage (fun model -> model.Page)

let init _ =
    { Page = Home
      ListTemplatesState = ListTemplates.initModel () },
    Cmd.none

let update (httpClient: HttpClient) message model =
    match message with
    | SetPage page ->
        match page with
        | Home -> { model with Page = page }, Cmd.none
        | ListTemplates -> { model with Page = page }, Cmd.map ListTemplatesMsg <| Cmd.ofMsg ListTemplates.Init
    | ListTemplatesMsg msg ->
        let listState, listCmd =
            ListTemplates.update httpClient msg model.ListTemplatesState

        let appCmd = Cmd.map ListTemplatesMsg listCmd
        { model with ListTemplatesState = listState }, appCmd

let view model dispatch =
    match model.Page with
    | Home -> h1 { text "Home" }
    | ListTemplates -> ListTemplates.view router model.ListTemplatesState dispatch

type App() =
    inherit ProgramComponent<Model, Message>()

    [<Inject>]
    member val HttpClient = Unchecked.defaultof<HttpClient> with get, set

    override this.Program =
        let update = update this.HttpClient
        Program.mkProgram init update view |> Program.withRouter router
