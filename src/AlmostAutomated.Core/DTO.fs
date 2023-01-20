module AlmostAutomated.Core.DTO

open AlmostAutomated.Core.Entities

type TemplateDTO =
    { Id: int64
      Title: string
      Description: string }

let toTemplateAndDetails templateAndDetails =
    let (template, details) = templateAndDetails

    { Id = template.Id
      Title = details.Title
      Description = details.Description }
