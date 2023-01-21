module AlmostAutomated.Core.DTO

open AlmostAutomated.Core.Entities

type TemplateDTO =
    { Id: int64
      Title: string
      Description: string }

let toTemplateAndDetails (templateAndDetails: Template.Select * TemplateDetails.Select) =
    let (template, details) = templateAndDetails

    { Id = template.Id
      Title = details.Title
      Description = details.Description }
