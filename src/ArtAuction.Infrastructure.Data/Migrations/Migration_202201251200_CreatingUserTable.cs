using FluentMigrator;

namespace ArtAuction.Infrastructure.Persistence.Migrations
{
    [Migration(202201251200)]
    public class Migration_202201251200_CreatingUserTable : Migration
    {
        public override void Up()
        {
            Create.Table("user")
                .WithColumn("user_id").AsGuid().NotNullable().PrimaryKey().WithDefaultValue(SystemMethods.NewGuid)
                .WithColumn("login").AsString(70).NotNullable()
                .WithColumn("email").AsString(70).NotNullable()
                .WithColumn("password").AsFixedLengthString(64).NotNullable()
                .WithColumn("role").AsByte().NotNullable()
                .WithColumn("first_name").AsString(100).NotNullable()
                .WithColumn("last_name").AsString(100).NotNullable()
                .WithColumn("patronymic").AsString(100).NotNullable()
                .WithColumn("birth_date").AsDate().NotNullable()
                .WithColumn("address").AsString(150).NotNullable()
                .WithColumn("is_vip").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("is_blocked").AsBoolean().NotNullable().WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Table("user");
        }
    }
}