module AlmostAutomated.Web.Client.Main2

open System
open System.Net.Http
open System.Net.Http.Json
open Microsoft.AspNetCore.Components
open Elmish
open Bolero
open Bolero.Html
open Routes


type Message = SetPage of Page

type Model =
    { Page: Page
      ListTemplatesState: ListTemplates.Model }

let router = Router.infer SetPage (fun model -> model.Page)

let init _ =
    { Page = Home
      ListTemplatesState = ListTemplates.initModel () },
    Cmd.none

let update message model =
    match message with
    | SetPage page -> { model with Page = page }, Cmd.none

let view model dispatch =
    match model.Page with
    | Home -> h1 { text "Home" }
    | ListTemplates -> ListTemplates.view router model.ListTemplatesState dispatch

type RouteComponent() =
    inherit ProgramComponent<Model, Message>()

    [<Inject>]
    member val HttpClient = Unchecked.defaultof<HttpClient> with get, set

    override _.Program = Program.mkProgram init update view |> Program.withRouter router
