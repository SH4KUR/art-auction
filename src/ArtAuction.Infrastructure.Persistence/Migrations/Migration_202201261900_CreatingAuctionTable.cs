using System;
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
                .WithColumn("auction_number").AsInt32().NotNullable().Identity()
                .WithColumn("lot_id").AsGuid().NotNullable()
                .WithColumn("seller_id").AsGuid().NotNullable()
                .WithColumn("creation_datetime").AsDateTime().NotNullable().WithDefaultValue(DateTime.Now)
                .WithColumn("start_billing_datetime").AsDateTime().NotNullable()
                .WithColumn("end_billing_datetime").AsDateTime().NotNullable()
                .WithColumn("start_price").AsDecimal().NotNullable()
                .WithColumn("current_price").AsDecimal().NotNullable()
                .WithColumn("full_price").AsDecimal().Nullable().WithDefaultValue(null)
                .WithColumn("bid_step").AsDecimal().NotNullable()
                .WithColumn("is_closed").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("customer_id").AsGuid().Nullable().WithDefaultValue(null);

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
            Create.UniqueConstraint()
                .OnTable("auction")
                .Column("auction_number");
        }

        public override void Down()
        {
            Delete.Table("auction");
        }
    }
}