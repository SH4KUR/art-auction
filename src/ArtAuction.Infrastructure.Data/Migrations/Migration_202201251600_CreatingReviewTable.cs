using FluentMigrator;

namespace ArtAuction.Infrastructure.Persistence.Migrations
{
    [Migration(202201251600)]
    public class Migration_202201251600_CreatingReviewTable : Migration
    {
        public override void Up()
        {
            Create.Table("review")
                .WithColumn("review_id").AsGuid().NotNullable().PrimaryKey().WithDefaultValue(SystemMethods.NewGuid)
                .WithColumn("user_id_from").AsGuid().NotNullable()
                .WithColumn("user_id_on").AsGuid().NotNullable()
                .WithColumn("date_time").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime)
                .WithColumn("rate").AsByte().NotNullable()
                .WithColumn("description").AsString(1000).NotNullable();

            Create.ForeignKey()
                .FromTable("review").ForeignColumn("user_id_from")
                .ToTable("user").PrimaryColumn("user_id");

            Create.ForeignKey()
                .FromTable("review").ForeignColumn("user_id_on")
                .ToTable("user").PrimaryColumn("user_id");
        }

        public override void Down()
        {
            Delete.Table("review");
        }
    }
}