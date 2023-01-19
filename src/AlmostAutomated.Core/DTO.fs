module AlmostAutomated.Core.DTO

open AlmostAutomated.Core.Entities

type TemplateDTO =
    { Id: int64
      Title: string
      Description: string }

let toTemplateAndDetails (template: Template.Select) (details: TemplateDetails.Select) =
    { Id = template.Id
      Title = details.Title
      Description = details.Description }

let toTemplateAndDetails' templateAndDetails =
    let (template, details) = templateAndDetails
    toTemplateAndDetails template details
