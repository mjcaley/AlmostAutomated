module Sidebar

open Feliz
open Feliz.Bulma
open Feliz.Router

[<ReactComponent>]
let Sidebar () =
    Bulma.navbarMenu [
        Bulma.navbarItem.a [
            prop.text "Home"
            prop.href <| Router.format "/"
        ]
        Bulma.navbarItem.a [
            prop.text "Templates"
            prop.href <| Router.format "/templates"
        ]
    ]
