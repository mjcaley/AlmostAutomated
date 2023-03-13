namespace AlmostAutomated.Core.Entities.Template

open System

type Select =
    { Id: int64
      Created: DateTime
      Deleted: DateTime option
      Title: string
      Description: string }

type Insert =
    { Title: string
      Description: string }
