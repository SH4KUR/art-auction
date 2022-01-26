using FluentMigrator;

namespace ArtAuction.Infrastructure.Persistence.Migrations
{
    [Migration(202201261600)]
    public class Migration_202201261600_CreatingVipTable : Migration
    {
        public override void Up()
        {
            Create.Table("vip")
                .WithColumn("vip_id").AsGuid().NotNullable().PrimaryKey().WithDefaultValue(SystemMethods.NewGuid)
                .WithColumn("operation_id").AsGuid().NotNullable()
                .WithColumn("user_id").AsGuid().NotNullable()
                .WithColumn("date_from").AsDate().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime)
                .WithColumn("date_until").AsDate().NotNullable();

            Create.ForeignKey()
                .FromTable("vip").ForeignColumn("user_id")
                .ToTable("user").PrimaryColumn("user_id");

            Create.ForeignKey()
                .FromTable("vip").ForeignColumn("operation_id")
                .ToTable("operation").PrimaryColumn("operation_id");

            Create.UniqueConstraint()
                .OnTable("vip")
                .Column("operation_id");
        }

        public override void Down()
        {
            Delete.Table("vip");
        }
    }
}