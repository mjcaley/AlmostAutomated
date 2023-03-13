module AlmostAutomated.Core.DTO

open AlmostAutomated.Core.Entities

type TemplateDTO =
    { Id: int64
      Title: string
      Description: string }

let toTemplateDTO (entity: Templates) =
    { Id = entity.Id
      Title = entity.Title
      Description = entity.Description }
