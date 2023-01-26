module Header

open Feliz
open Feliz.Bulma

[<ReactComponent>]
let Header () =
    Bulma.hero [
        Bulma.heroBody [
            Bulma.title [
                prop.text "Almost Automated"
                color.isPrimary
                ]
        ]
    ]
