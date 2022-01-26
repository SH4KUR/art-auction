using FluentMigrator;

namespace ArtAuction.Infrastructure.Persistence.Migrations
{
    [Migration(202201251800)]
    public class Migration_202201251800_CreatingAccountTable : Migration
    {
        public override void Up()
        {
            Create.Table("account")
                .WithColumn("account_id").AsGuid().NotNullable().PrimaryKey().WithDefaultValue(SystemMethods.NewGuid)
                .WithColumn("user_id").AsGuid().Unique().NotNullable()
                .WithColumn("sum").AsDecimal().NotNullable().WithDefaultValue(decimal.Zero)
                .WithColumn("last_update").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime);

            Create.ForeignKey()
                .FromTable("account").ForeignColumn("user_id")
                .ToTable("user").PrimaryColumn("user_id");

            Create.UniqueConstraint()
                .OnTable("account")
                .Column("user_id");
        }

        public override void Down()
        {
            Delete.Table("account");
        }
    }
}