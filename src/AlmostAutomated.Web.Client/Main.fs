module Main

open System.Net.Http
open Microsoft.AspNetCore.Components
open Elmish
open Bolero
open Bolero.Html
open Routes


type Message =
    | SetPage of Page
    | ListTemplatesMsg of ListTemplates.Message
    | NewTemplateMsg of NewTemplate.Message

type Model =
    { Page: Page
      ListTemplatesState: ListTemplates.Model
      NewTemplateState: NewTemplate.Model }

let router = Router.infer SetPage (fun model -> model.Page)

let init _ =
    { Page = Home
      ListTemplatesState = ListTemplates.initModel ()
      NewTemplateState = NewTemplate.initModel () },
    Cmd.none

let update (httpClient: HttpClient) message model =
    match message with
    | SetPage page ->
        match page with
        | Home -> { model with Page = page }, Cmd.none
        | ListTemplates -> { model with Page = page }, Cmd.map ListTemplatesMsg <| Cmd.ofMsg ListTemplates.Init
        | NewTemplate -> { model with Page = page }, Cmd.map NewTemplateMsg <| Cmd.ofMsg NewTemplate.Edit
    | ListTemplatesMsg msg ->
        let listState, listCmd =
            ListTemplates.update httpClient msg model.ListTemplatesState

        let appCmd = Cmd.map ListTemplatesMsg listCmd
        { model with ListTemplatesState = listState }, appCmd
    | NewTemplateMsg msg ->
        let newState, newCmd = NewTemplate.update httpClient msg model.NewTemplateState
        let appCmd = Cmd.map NewTemplateMsg newCmd
        { model with NewTemplateState = newState }, appCmd

let view model dispatch =
    div {
        div {
            id "sidebar"

            a {
                router.HRef Home
                "Home"
            }

            a {
                router.HRef ListTemplates
                "Templates"
            }
        }

        content {
            match model.Page with
            | Home -> h1 { text "Home" }
            | ListTemplates -> ListTemplates.view router model.ListTemplatesState (ListTemplatesMsg >> dispatch)
            | NewTemplate -> NewTemplate.view model.NewTemplateState (NewTemplateMsg >> dispatch)
        }
    }

type App() =
    inherit ProgramComponent<Model, Message>()

    [<Inject>]
    member val HttpClient = Unchecked.defaultof<HttpClient> with get, set

    override this.Program =
        let update = update this.HttpClient
        Program.mkProgram init update view |> Program.withRouter router
