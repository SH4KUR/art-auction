using FluentMigrator;

namespace ArtAuction.Infrastructure.Persistence.Migrations
{
    [Migration(202201262100)]
    public class Migration_202201262100_CreatingMessageTable : Migration
    {
        public override void Up()
        {
            Create.Table("message")
                .WithColumn("message_id").AsGuid().NotNullable().PrimaryKey().WithDefaultValue(SystemMethods.NewGuid)
                .WithColumn("user_id").AsGuid().NotNullable()
                .WithColumn("auction_id").AsGuid().NotNullable()
                .WithColumn("date_time").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime)
                .WithColumn("message_text").AsString(300).NotNullable()
                .WithColumn("is_admin").AsBoolean().NotNullable().WithDefaultValue(false);

            Create.ForeignKey()
                .FromTable("message").ForeignColumn("auction_id")
                .ToTable("auction").PrimaryColumn("auction_id");
            Create.ForeignKey()
                .FromTable("message").ForeignColumn("user_id")
                .ToTable("user").PrimaryColumn("user_id");
        }

        public override void Down()
        {
            Delete.Table("message");
        }
    }
}