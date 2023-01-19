module AlmostAutomated.Core.Entities.TemplateDetails

open System

[<CLIMutable>]
type Select =
    { Id: int64

      Title: string
      Description: string
      ValidFrom: DateTime
      ValidTo: DateTime option

      TemplateId: int64 }

type Insert =
    { Title: string
      Description: string
      TemplateId: int64 }

type Insert' = { Title: string; Description: string }
