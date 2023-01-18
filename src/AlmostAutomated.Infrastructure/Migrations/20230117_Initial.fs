namespace AlmostAutomated.Infrastructure

open FluentMigrator

module Migrations =
    [<Migration(20230117L, "Initial database schema")>]
    type Initial =
        inherit Migration
        new() = {  }

        override this.Up() =
            this
                .Create
                .Table("Template")
                .WithColumn("Id")
                .AsInt64()
                .PrimaryKey()
                .Identity()
                .WithColumn("Created")
                .AsDateTime()
                .WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("Deleted")
                .AsDateTime()
                .Nullable()
            |> ignore

            this
                .Create
                .Table("TemplateDetails")
                .WithColumn("Id")
                .AsInt64()
                .PrimaryKey()
                .Identity()
                .WithColumn("TemplateId")
                .AsInt64()
                .ForeignKey("Template", "Id")
                .WithColumn("Title")
                .AsString()
                .WithColumn("Description")
                .AsString()
                .WithColumn("ValidFrom")
                .AsDateTime()
                .WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("ValidTo")
                .AsDateTime()
                .Nullable()
            |> ignore

        override this.Down() =
            this.Delete.Table("TemplateDetails") |> ignore
            this.Delete.Table("Template") |> ignore
