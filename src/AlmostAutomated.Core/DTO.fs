module AlmostAutomated.Core.DTO

open Entities

type TemplateDTO = { Title: string; Description: string }

let toTemplateAndDetails (template: Template) (details: TemplateDetails) =
    (template.Id,
     { Title = details.Title
       Description = details.Description })

let toTemplateAndDetails' templateAndDetails =
    let (template, details) = templateAndDetails
    toTemplateAndDetails template details
