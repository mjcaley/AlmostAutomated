module AlmostAutomated.Migration.Migrations

open FluentMigrator
open FluentMigrator.Postgres
open AlmostAutomated.Core.Entities

[<Migration(20230117L, "Initial database schema")>]
type Initial =
    inherit Migration
    new() = { }

    override this.Up () =
        this
            .Create
            .Table("templates")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("created").AsDateTime().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("deleted").AsDateTime().Nullable()
            .WithColumn("title").AsString()
            .WithColumn("description").AsString()
        |> ignore

    override this.Down() =
        this.Delete.Table("template") |> ignore

[<Migration(20230313L, "Add template audit table")>]
type TemplateAudit =
    inherit Migration
    new() = { }

    override this.Up () =
        this
            .Create
            .Table("templates_audit")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("action").AsInt32()
            .WithColumn("audit_created").AsDateTime().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("templates_id").AsInt64().ForeignKey("templates", "id")
            .WithColumn("created").AsDateTime().Nullable()
            .WithColumn("deleted").AsDateTime().Nullable()
            .WithColumn("title").AsString().Nullable()
            .WithColumn("description").AsString().Nullable()
        |> ignore

    override this.Down () =
        this.Delete.Table("templates_audit") |> ignore
