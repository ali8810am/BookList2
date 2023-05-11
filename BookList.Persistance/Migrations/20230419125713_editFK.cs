using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class editFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorrowAllocations_Customer_CustomerId",
                table: "BorrowAllocations");

            migrationBuilder.DropForeignKey(
                name: "FK_BorrowRequests_Customer_CustomerId",
                table: "BorrowRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowAllocations_Customer_CustomerId",
                table: "BorrowAllocations",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowRequests_Customer_CustomerId",
                table: "BorrowRequests",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorrowAllocations_Customer_CustomerId",
                table: "BorrowAllocations");

            migrationBuilder.DropForeignKey(
                name: "FK_BorrowRequests_Customer_CustomerId",
                table: "BorrowRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowAllocations_Customer_CustomerId",
                table: "BorrowAllocations",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowRequests_Customer_CustomerId",
                table: "BorrowRequests",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
