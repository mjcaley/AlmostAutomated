module TemplateComponent

open Feliz
open AlmostAutomated.Core.DTO

[<ReactComponent>]
let TemplateComponent(template: TemplateDTO) =
    Html.div [
        Html.p "Title"
        Html.p template.Title
        Html.p "Description"
        Html.p "Id"
        Html.p (int template.Id)
    ]