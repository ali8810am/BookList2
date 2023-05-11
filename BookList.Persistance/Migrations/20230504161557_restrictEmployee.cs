using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class restrictEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorrowAllocations_Books_BookId",
                table: "BorrowAllocations");

            migrationBuilder.DropForeignKey(
                name: "FK_BorrowAllocations_Employee_EmployeeId",
                table: "BorrowAllocations");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "BorrowAllocations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowAllocations_Books_BookId",
                table: "BorrowAllocations",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowAllocations_Employee_EmployeeId",
                table: "BorrowAllocations",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorrowAllocations_Books_BookId",
                table: "BorrowAllocations");

            migrationBuilder.DropForeignKey(
                name: "FK_BorrowAllocations_Employee_EmployeeId",
                table: "BorrowAllocations");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "BorrowAllocations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowAllocations_Books_BookId",
                table: "BorrowAllocations",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowAllocations_Employee_EmployeeId",
                table: "BorrowAllocations",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id");
        }
    }
}
