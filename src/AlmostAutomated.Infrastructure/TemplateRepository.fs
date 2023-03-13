module AlmostAutomated.Infrastructure.TemplateRepository

open AlmostAutomated.Core.Entities
open Npgsql.FSharp
open System.Threading.Tasks

let listTemplates connectionString : Task<Templates list> =
    connectionString
    |> Sql.connect
    |> Sql.query 
        """select * from "templates"
        where "templates"."deleted" is null
        order by "templates"."id"
        """
    |> Sql.executeAsync (fun read -> 
        {
            Id = read.int64 "id";
            Title = read.string "title";
            Description = read.string "description";
            Created = read.dateTime "created";
            Deleted = read.dateTimeOrNone "deleted";
        })

let listDeletedTemplates connectionString : Task<Templates list> =
    connectionString
    |> Sql.connect
    |> Sql.query 
        """select * from "templates"
        where "templates"."deleted" is not null
        order by "templates"."id"
        """
    |> Sql.executeAsync (fun read -> 
        {
            Id = read.int64 "id";
            Title = read.string "title";
            Description = read.string "description";
            Created = read.dateTime "created";
            Deleted = read.dateTimeOrNone "deleted";
        })

let getTemplateById connectionString id : Task<Templates> =
    connectionString
    |> Sql.connect
    |> Sql.query
        """select * from "templates"
        where
            "templates"."id" = @id and
            "templates"."deleted" is null
        """
    |> Sql.parameters [ "@id", Sql.int64 id ]
    |> Sql.executeRowAsync (fun read ->
        {
            Id = read.int64 "id";
            Title = read.string "title";
            Description = read.string "description";
            Created = read.dateTime "created";
            Deleted = read.dateTimeOrNone "deleted";
        })

let getDeletedTemplateById connectionString id : Task<Templates> =
    connectionString
    |> Sql.connect
    |> Sql.query
        """select * from "templates"
        where
            "templates"."id" = @id and
            "templates"."deleted" is not null
        """
    |> Sql.parameters [ "@id", Sql.int64 id ]
    |> Sql.executeRowAsync (fun read ->
        {
            Id = read.int64 "id";
            Title = read.string "title";
            Description = read.string "description";
            Created = read.dateTime "created";
            Deleted = read.dateTimeOrNone "deleted";
        })

let createTemplate connectionString title description : Task<Templates> =
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
        "@title", Sql.string title;
        "@description", Sql.string description
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

let deleteTemplate connectionString id : Task<Templates> =
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
