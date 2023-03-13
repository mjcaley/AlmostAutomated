module AlmostAutomated.Migration.Migrations

open FluentMigrator

[<Migration(20230117L, "Initial database schema")>]
type Initial =
    inherit Migration
    new() = {  }

    override this.Up() =
        this
            .Create
            .Table("templates").WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("created").AsDateTime().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("deleted").AsDateTime().Nullable()
            .WithColumn("title").AsString()
            .WithColumn("description").AsString()
            
        |> ignore

    override this.Down() =
        this.Delete.Table("template") |> ignore
