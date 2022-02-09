using FluentMigrator;

namespace ArtAuction.Infrastructure.Persistence.Migrations
{
    [Migration(202201262200)]
    public class Migration_202201262200_CreatingTrackedTable : Migration
    {
        public override void Up()
        {
            Create.Table("tracked")
                .WithColumn("user_id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("auction_id").AsGuid().PrimaryKey().NotNullable();

            Create.ForeignKey()
                .FromTable("tracked").ForeignColumn("auction_id")
                .ToTable("auction").PrimaryColumn("auction_id");

            Create.ForeignKey()
                .FromTable("tracked").ForeignColumn("user_id")
                .ToTable("user").PrimaryColumn("user_id");
        }

        public override void Down()
        {
            Delete.Table("tracked");
        }
    }
}