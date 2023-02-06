module AlmostAutomated.Web.Client.Routes

open System
open System.Net.Http
open System.Net.Http.Json
open Microsoft.AspNetCore.Components
open Elmish
open Bolero
open Bolero.Html


//type Page =
//    | [<EndPoint "/">] Home
//    | [<EndPoint "/templates">] Templates
//    | [<EndPoint "/runs">] Runs

//type Model =
//    {
//        page: Page
//    }

//type Message =
//    | SetPage of Page
    

//let update (http: HttpClient) message model =
//    match message with
//    | SetPage page ->
//        { model with page = page }, Cmd.none

//    | Increment ->
//        { model with counter = model.counter + 1 }, Cmd.none
//    | Decrement ->
//        { model with counter = model.counter - 1 }, Cmd.none
//    | SetCounter value ->
//        { model with counter = value }, Cmd.none

//    | GetBooks ->
//        let getBooks() = http.GetFromJsonAsync<Book[]>("/books.json")
//        let cmd = Cmd.OfTask.either getBooks () GotBooks Error
//        { model with books = None }, cmd
//    | GotBooks books ->
//        { model with books = Some books }, Cmd.none

//    | Error exn ->
//        { model with error = Some exn.Message }, Cmd.none
//    | ClearError ->
//        { model with error = None }, Cmd.none

///// Connects the routing system to the Elmish application.
//let router = Router.infer SetPage (fun model -> model.page)

//type Main = Template<"wwwroot/main.html">

//let homePage model dispatch =
//    Main.Home().Elt()

//let counterPage model dispatch =
//    Main.Counter()
//        .Decrement(fun _ -> dispatch Decrement)
//        .Increment(fun _ -> dispatch Increment)
//        .Value(model.counter, fun v -> dispatch (SetCounter v))
//        .Elt()

//let dataPage model dispatch =
//    Main.Data()
//        .Reload(fun _ -> dispatch GetBooks)
//        .Rows(cond model.books <| function
//            | None ->
//                Main.EmptyData().Elt()
//            | Some books ->
//                forEach books <| fun book ->
//                    tr {
//                        td { book.title }
//                        td { book.author }
//                        td { book.publishDate.ToString("yyyy-MM-dd") }
//                        td { book.isbn }
//                    })
//        .Elt()


//let menuItem (model: Model) (page: Page) (text: string) =
//    Main.MenuItem()
//        .Active(if model.page = page then "is-active" else "")
//        .Url(router.Link page)
//        .Text(text)
//        .Elt()

//let view model dispatch =
//    Main()
//        .Menu(concat {
//            menuItem model Home "Home"
//            menuItem model Counter "Counter"
//            menuItem model Data "Download data"
//        })
//        .Body(
//            cond model.page <| function
//            | Home -> homePage model dispatch
//            | Counter -> counterPage model dispatch
//            | Data ->
//                dataPage model dispatch
//        )
//        .Error(
//            cond model.error <| function
//            | None -> empty()
//            | Some err ->
//                Main.ErrorNotification()
//                    .Text(err)
//                    .Hide(fun _ -> dispatch ClearError)
//                    .Elt()
//        )
//        .Elt()

//type TemplatesApp() =
//    inherit ProgramComponent<Model, Message>()

//    [<Inject>]
//    member val HttpClient = Unchecked.defaultof<HttpClient> with get, set

//    override this.Program =
//        let update = update this.HttpClient
//        Program.mkSimple (fun _ -> initModel, Cmd.ofMsg GetBooks) update view
//        |> Program.withRouter router
