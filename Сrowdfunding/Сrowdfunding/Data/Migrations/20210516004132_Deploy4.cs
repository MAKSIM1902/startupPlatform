using Microsoft.EntityFrameworkCore.Migrations;

namespace Сrowdfunding.Data.Migrations
{
    public partial class Deploy4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Campaigns_CampaignId",
                table: "Ratings");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Campaigns_CampaignId",
                table: "Ratings",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Campaigns_CampaignId",
                table: "Ratings");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Campaigns_CampaignId",
                table: "Ratings",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id");
        }
    }
}
