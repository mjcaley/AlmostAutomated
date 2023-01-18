namespace AlmostAutomated.Api

module TemplateService =

    open AlmostAutomated.Infrastructure.TemplateRepository

    let listTemplates dbConn = task { return! getAll dbConn }
