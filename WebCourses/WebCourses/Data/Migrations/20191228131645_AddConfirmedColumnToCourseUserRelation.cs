using Microsoft.EntityFrameworkCore.Migrations;

namespace WebCourses.Data.Migrations
{
    public partial class AddConfirmedColumnToCourseUserRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Confirmed",
                table: "CourseUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Confirmed",
                table: "CourseUsers");
        }
    }
}
