module AlmostAutomated.Core.Entities.Template

open System

[<CLIMutable>]
type Select =
    { Id: int64
      Created: DateTime
      Deleted: DateTime option }

type Insert = { Created: DateTime }
