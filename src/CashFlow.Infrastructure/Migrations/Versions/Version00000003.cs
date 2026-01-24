using FluentMigrator;
using System.Data;

namespace CashFlow.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.ADD_NEW_TABLE_TAG, "Add new table Tag")]
public class Version00000003 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Tags")
            .WithColumn("Id").AsInt64().NotNullable()
                .PrimaryKey().Identity()
            .WithColumn("Value").AsInt16().NotNullable()
            .WithColumn("ExpenseId").AsInt64().NotNullable()
                .ForeignKey("FK_Tags_ExpenseId", "Expenses", "Id").OnDelete(Rule.Cascade);
    }
}
