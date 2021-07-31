using Microsoft.EntityFrameworkCore.Migrations;

namespace mysql_distributedcache.Migrations
{
    public partial class AddUserSubscriptionActiveProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<sbyte>(
                name: "SubscriptionActive",
                table: "User",
                type: "tinyint",
                nullable: false,
                defaultValue: (sbyte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscriptionActive",
                table: "User");
        }
    }
}
