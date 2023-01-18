module AlmostAutomated.Core.DTO

open Entities

type TemplateDTO =
    { Id: int
      Title: string
      Description: string }

let toTemplateAndDetails (template: Template) (details: TemplateDetails) =
    { Id = template.Id
      Title = details.Title
      Description = details.Description }

let toTemplateAndDetails' templateAndDetails =
    let (template, details) = templateAndDetails
    toTemplateAndDetails template details
