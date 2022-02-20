using FluentMigrator;

namespace ArtAuction.Infrastructure.Persistence.Migrations
{
    [Migration(202201262000)]
    public class Migration_202201262000_CreatingBidTable : Migration
    {
        public override void Up()
        {
            Create.Table("bid")
                .WithColumn("bid_id").AsGuid().NotNullable().PrimaryKey().WithDefaultValue(SystemMethods.NewGuid)
                .WithColumn("user_id").AsGuid().NotNullable()
                .WithColumn("auction_id").AsGuid().NotNullable()
                .WithColumn("date_time").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime)
                .WithColumn("sum").AsDecimal().NotNullable();

            Create.ForeignKey()
                .FromTable("bid").ForeignColumn("auction_id")
                .ToTable("auction").PrimaryColumn("auction_id");
            Create.ForeignKey()
                .FromTable("bid").ForeignColumn("user_id")
                .ToTable("user").PrimaryColumn("user_id");
        }

        public override void Down()
        {
            Delete.Table("bid");
        }
    }
}