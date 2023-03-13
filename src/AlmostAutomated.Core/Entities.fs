namespace AlmostAutomated.Core.Entities

open System

type AuditAction =
    | Create
    | Insert
    | Delete

type Templates =
    { Id: int64
      Created: DateTime
      Deleted: DateTime option
      Title: string
      Description: string }
      
type TemplatesAudit =
    { Id: int64
      Action: AuditAction
      AuditCreated: DateTime
      TemplateId: int64
      Created: DateTime option
      Deleted: DateTime option
      Title: string option
      Description: string option }
