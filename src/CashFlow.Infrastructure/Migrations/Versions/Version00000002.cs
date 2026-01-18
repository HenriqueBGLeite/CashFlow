using FluentMigrator;

namespace CashFlow.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.ALTER_COLUMNS_DESCRIPTION_AND_AMOUNT_IN_EXPENSE, "Alter Column in Database")]
public class Version00000002 : ForwardOnlyMigration
{
    public override void Up()
    {
        Alter.Column("Description").OnTable("Expenses").AsString(2000).Nullable();
        Alter.Column("Amount").OnTable("Expenses").AsDecimal(65, 2).NotNullable();
    }
}
