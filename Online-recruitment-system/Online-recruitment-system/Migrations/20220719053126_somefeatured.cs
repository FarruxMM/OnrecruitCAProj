using Microsoft.EntityFrameworkCore.Migrations;

namespace Online_recruitment_system.Migrations
{
    public partial class somefeatured : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobFollowers_Jobs_JobId",
                table: "JobFollowers");

            migrationBuilder.DropIndex(
                name: "IX_JobFollowers_JobId",
                table: "JobFollowers");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "JobFollowers");

            migrationBuilder.AddColumn<int>(
                name: "Usertype",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Usertype",
                table: "Candidates",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Usertype",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Usertype",
                table: "Candidates");

            migrationBuilder.AddColumn<int>(
                name: "JobId",
                table: "JobFollowers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobFollowers_JobId",
                table: "JobFollowers",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobFollowers_Jobs_JobId",
                table: "JobFollowers",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
