using FluentMigrator;

namespace ArtAuction.Infrastructure.Persistence.Migrations
{
    [Migration(202201261700)]
    public class Migration_202201261700_CreatingCategoryTable : Migration
    {
        public override void Up()
        {
            Create.Table("category")
                .WithColumn("category_id").AsGuid().NotNullable().PrimaryKey().WithDefaultValue(SystemMethods.NewGuid)
                .WithColumn("name").AsString(100).NotNullable();

            Create.UniqueConstraint()
                .OnTable("category")
                .Column("name");
        }

        public override void Down()
        {
            Delete.Table("category");
        }
    }
}