using FluentMigrator;

namespace ArtAuction.Infrastructure.Persistence.Migrations
{
    [Migration(202201261800)]
    public class Migration_202201261800_CreatingLotTable : Migration
    {
        public override void Up()
        {
            Create.Table("lot")
                .WithColumn("lot_id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("category_id").AsGuid().NotNullable()
                .WithColumn("name").AsString(200).NotNullable()
                .WithColumn("painting_date").AsString(50).NotNullable()
                .WithColumn("photo").AsCustom("VARBINARY(MAX)").NotNullable()
                .WithColumn("description").AsString(1000).NotNullable();

            Create.ForeignKey()
                .FromTable("lot").ForeignColumn("category_id")
                .ToTable("category").PrimaryColumn("category_id");
        }

        public override void Down()
        {
            Delete.Table("lot");
        }
    }
}