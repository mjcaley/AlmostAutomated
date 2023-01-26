module Sidebar

open Feliz
open Feliz.Bulma
open Feliz.Router

[<ReactComponent>]
let Sidebar () =
    Bulma.menu [
        Bulma.menuList [
            Html.a [
                prop.text "Home"
                prop.href <| Router.format "/"
            ]
            Html.a [
                prop.text "Templates"
                prop.href <| Router.format "/templates"
            ]
        ]
    ]
