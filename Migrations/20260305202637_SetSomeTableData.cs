using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TheLibraryApi.Migrations
{
    /// <inheritdoc />
    public partial class SetSomeTableData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Authors_AuthorId",
                table: "Books");

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Bio", "DateOfBirth", "Name" },
                values: new object[,]
                {
                    { 1, " Software Engineer", new DateTime(1980, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "J.K. Rowling" },
                    { 2, "C# developer", new DateTime(1990, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "George R.R. Martin" },
                    { 3, "Java developer", new DateTime(1999, 1, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Agatha Christie" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "AvailableCopies", "ISBN", "PublishedYear", "Title", "TotalCopies" },
                values: new object[,]
                {
                    { 1, 2, 12, "978-0439708180", 1998, "Harry Potter and the Sorcerer's Stone", 12 },
                    { 2, 1, 34, "978-0553103540", 1996, "A Game of Thrones", 34 },
                    { 3, 3, 4, "978-0553103540", 2023, "The Slient Patient ", 4 },
                    { 4, 1, 4, "978-0553103540", 2023, "The Girl On The Train", 4 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Authors_AuthorId",
                table: "Books",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Authors_AuthorId",
                table: "Books");

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Authors_AuthorId",
                table: "Books",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
