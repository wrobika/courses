using Microsoft.EntityFrameworkCore.Migrations;

namespace WebCourses.Data.Migrations
{
    public partial class UserTestResult_and_OpenQuestionAnswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OpenQuestionAnswers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    QuestionId = table.Column<string>(nullable: true),
                    Checked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenQuestionAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenQuestionAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OpenQuestionAnswers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserTestResults",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    TestId = table.Column<string>(nullable: true),
                    PointsCount = table.Column<int>(nullable: false),
                    Checked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTestResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTestResults_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTestResults_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OpenQuestionAnswers_QuestionId",
                table: "OpenQuestionAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenQuestionAnswers_UserId",
                table: "OpenQuestionAnswers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTestResults_TestId",
                table: "UserTestResults",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTestResults_UserId",
                table: "UserTestResults",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OpenQuestionAnswers");

            migrationBuilder.DropTable(
                name: "UserTestResults");
        }
    }
}
