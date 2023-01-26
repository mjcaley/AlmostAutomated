module Page

open Feliz
open Feliz.Bulma
open Header
open Sidebar

[<ReactComponent>]
let Page (content) =
    Html.div [
        Header ()

        Sidebar ()

        content ()
    ]
