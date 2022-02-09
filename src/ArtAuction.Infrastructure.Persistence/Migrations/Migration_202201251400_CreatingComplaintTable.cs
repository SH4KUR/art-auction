using FluentMigrator;

namespace ArtAuction.Infrastructure.Persistence.Migrations
{
    [Migration(202201251400)]
    public class Migration_202201251400_CreatingComplaintTable : Migration
    {
        public override void Up()
        {
            Create.Table("complaint")
                .WithColumn("complaint_id").AsGuid().NotNullable().PrimaryKey().WithDefaultValue(SystemMethods.NewGuid)
                .WithColumn("user_id_from").AsGuid().NotNullable()
                .WithColumn("user_id_on").AsGuid().NotNullable()
                .WithColumn("date_time").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime)
                .WithColumn("description").AsString(1000).NotNullable()
                .WithColumn("is_processed").AsBoolean().NotNullable().WithDefaultValue(false);

            Create.ForeignKey()
                .FromTable("complaint").ForeignColumn("user_id_from")
                .ToTable("user").PrimaryColumn("user_id");

            Create.ForeignKey()
                .FromTable("complaint").ForeignColumn("user_id_on")
                .ToTable("user").PrimaryColumn("user_id");
        }

        public override void Down()
        {
            Delete.Table("complaint");
        }
    }
}