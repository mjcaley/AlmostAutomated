module AlmostAutomated.Web.Client.Routes

open Bolero

type Page =
    | [<EndPoint "/">] Home
    | [<EndPoint "/templates">] ListTemplates
    | [<EndPoint "/templates/edit/{id}">] EditTemplate of id: int64
    | [<EndPoint "/templates/new">] NewTemplate
    | [<EndPoint "/runs">] Runs
