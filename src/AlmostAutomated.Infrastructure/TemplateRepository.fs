module AlmostAutomated.Infrastructure.TemplateRepository

open AlmostAutomated.Core.Entities
open Npgsql.FSharp
open System.Threading.Tasks

let listTemplates connectionString isDeleted : Task<Template.Select list> =
    connectionString
    |> Sql.connect
    |> Sql.query 
        """select * from "templates"
        where ("templates"."deleted" is null) != @deleted
        order by "templates"."id"
        """
    |> Sql.parameters [ "@deleted", Sql.bool isDeleted ]
    |> Sql.executeAsync (fun read -> 
        {
            Id = read.int64 "id";
            Title = read.string "title";
            Description = read.string "description";
            Created = read.dateTime "created";
            Deleted = read.dateTimeOrNone "deleted";
        })

let getTemplateById connectionString id isDeleted : Task<Template.Select> =
    connectionString
    |> Sql.connect
    |> Sql.query
        """select * from "templates"
        where
            "templates"."id" = @id and
            ("templates"."deleted" is null) != @deleted
        """
    |> Sql.parameters [ "@id", Sql.int64 id; "@deleted", Sql.bool isDeleted ]
    |> Sql.executeRowAsync (fun read ->
        {
            Id = read.int64 "id";
            Title = read.string "title";
            Description = read.string "description";
            Created = read.dateTime "created";
            Deleted = read.dateTimeOrNone "deleted";
        })

let createTemplate connectionString (details: Template.Insert) : Task<Template.Select> =
    connectionString
    |> Sql.connect
    |> Sql.query
        """insert into "templates"
        ("created", "title", "description")
        values
        (now() at time zone 'utc', @title, @description)
        returning *
        """
    |> Sql.parameters [
        "@title", Sql.string details.Title;
        "@description", Sql.string details.Description
    ]
    |> Sql.executeRowAsync (fun read ->
        {
            Id = read.int64 "id";
            Title = read.string "title";
            Description = read.string "description";
            Created = read.dateTime "created";
            Deleted = read.dateTimeOrNone "deleted";
        }
    )

let deleteTemplate connectionString id : Task<Template.Select> =
    connectionString
    |> Sql.connect
    |> Sql.query 
        """update "templates"
        set "deleted" = (now() at time zone 'utc')
        where
            "id" = @id and
            "templates"."deleted" is null
        returning *
        """
    |> Sql.parameters ["@id", Sql.int64 id ]
    |> Sql.executeRowAsync (fun read ->
        {
            Id = read.int64 "id";
            Title = read.string "title";
            Description = read.string "description";
            Created = read.dateTime "created";
            Deleted = read.dateTimeOrNone "deleted";
        }
    )
