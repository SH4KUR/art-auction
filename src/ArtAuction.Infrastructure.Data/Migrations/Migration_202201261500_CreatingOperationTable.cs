using FluentMigrator;

namespace ArtAuction.Infrastructure.Persistence.Migrations
{
    [Migration(202201261500)]
    public class Migration_202201261500_CreatingOperationTable : Migration
    {
        public override void Up()
        {
            Create.Table("operation")
                .WithColumn("operation_id").AsGuid().NotNullable().PrimaryKey().WithDefaultValue(SystemMethods.NewGuid)
                .WithColumn("account_id").AsGuid().NotNullable()
                .WithColumn("date_time").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime)
                .WithColumn("sum_before").AsDecimal().NotNullable()
                .WithColumn("sum_operation").AsDecimal().NotNullable()
                .WithColumn("sum_after").AsDecimal().NotNullable()
                .WithColumn("description").AsString(200).NotNullable();

            Create.ForeignKey()
                .FromTable("operation").ForeignColumn("account_id")
                .ToTable("account").PrimaryColumn("account_id");
        }

        public override void Down()
        {
            Delete.Table("operation");
        }
    }
}