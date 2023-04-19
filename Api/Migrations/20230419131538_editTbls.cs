using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class editTbls : Migration
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

            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "BorrowRequests");

            migrationBuilder.DropColumn(
                name: "AllocationId",
                table: "BorrowAllocations");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorrowAllocations_Customer_CustomerId",
                table: "BorrowAllocations");

            migrationBuilder.DropForeignKey(
                name: "FK_BorrowRequests_Customer_CustomerId",
                table: "BorrowRequests");

            migrationBuilder.AddColumn<int>(
                name: "RequestId",
                table: "BorrowRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AllocationId",
                table: "BorrowAllocations",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
    }
}
