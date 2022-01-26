using FluentMigrator;

namespace ArtAuction.Infrastructure.Persistence.Migrations
{
    [Migration(202201261900)]
    public class Migration_202201261900_CreatingAuctionTable : Migration
    {
        public override void Up()
        {
            Create.Table("auction")
                .WithColumn("auction_id").AsGuid().NotNullable().PrimaryKey().WithDefaultValue(SystemMethods.NewGuid)
                .WithColumn("lot_id").AsGuid().NotNullable()
                .WithColumn("seller_id").AsGuid().NotNullable()
                .WithColumn("start_billing_date").AsDateTime().NotNullable()
                .WithColumn("end_billing_date").AsDateTime().NotNullable()
                .WithColumn("start_price").AsDecimal().NotNullable()
                .WithColumn("current_price").AsDecimal().NotNullable()
                .WithColumn("full_price").AsDecimal().WithDefaultValue(null)
                .WithColumn("bid_step").AsDecimal().NotNullable()
                .WithColumn("is_closed").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("customer_id").AsGuid().WithDefaultValue(null);

            Create.ForeignKey()
                .FromTable("auction").ForeignColumn("lot_id")
                .ToTable("lot").PrimaryColumn("lot_id");

            Create.ForeignKey()
                .FromTable("auction").ForeignColumn("seller_id")
                .ToTable("user").PrimaryColumn("user_id");

            Create.ForeignKey()
                .FromTable("auction").ForeignColumn("customer_id")
                .ToTable("user").PrimaryColumn("user_id");

            Create.UniqueConstraint()
                .OnTable("auction")
                .Column("lot_id");
        }

        public override void Down()
        {
            Delete.Table("auction");
        }
    }
}