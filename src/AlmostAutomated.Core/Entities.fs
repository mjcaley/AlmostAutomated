﻿namespace AlmostAutomated.Core
module Entities =

    open System
    open System.ComponentModel.DataAnnotations
    
    type AuditType = 
        | Select
        | Update
        | Insert
        | Delete
        
     [<CLIMutable>]
     type TemplateDetails =
        {
            [<Key>]
            Id : int

            [<Required>]
            [<MaxLength(500)>]
            Title : string
            [<Required>]
            [<MaxLength(2000)>]
            Description : string
            [<Required>]
            ValidFrom : DateTime
            ValidTo : DateTime option
        }

    [<CLIMutable>]
    type Template =
        {
            [<Key>]
            Id : int
        
            Created : DateTime
            Deleted : DateTime option

            TemplateDetailsId : int
            TemplateDetails : TemplateDetails
        }

    [<CLIMutable>]
    type TemplateDetailsAudit = {
        [<Key>]
        Id : int

        TemplateDetailsId : TemplateDetails
        Action : AuditType
        Title : string option
        Description : string option
        ValidFrom : DateTime option
        ValidTo : DateTime option
        TemplateId : Template option
    }
