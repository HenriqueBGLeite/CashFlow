using FluentMigrator;

namespace CashFlow.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.INITIAL_MIGRATION, "Initial Migration")]
public class Version00000001 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Users")
            .WithColumn("Id").AsInt64().NotNullable()
                .PrimaryKey().Identity()
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("Email").AsString(255).NotNullable()
            .WithColumn("Password").AsString(500).NotNullable()
            .WithColumn("UserIdentifier").AsGuid().NotNullable()
            .WithColumn("Role").AsString(1000).NotNullable();

        Create.Table("Expenses")
            .WithColumn("Id").AsInt64().NotNullable()
                .PrimaryKey().Identity()
            .WithColumn("Title").AsString(300).NotNullable()
            .WithColumn("Description").AsString(2000)
            .WithColumn("Date").AsDateTime().NotNullable()
            .WithColumn("Amount").AsDecimal(65, 30).NotNullable()
            .WithColumn("PaymentType").AsInt16().NotNullable()
            .WithColumn("UserId").AsInt64().NotNullable()
                .ForeignKey("FK_Expenses_UserId", "Users", "Id");
    }
}
