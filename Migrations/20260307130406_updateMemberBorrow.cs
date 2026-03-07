using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheLibraryApi.Migrations
{
    /// <inheritdoc />
    public partial class updateMemberBorrow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Bio", "DateOfBirth", "Name" },
                values: new object[] { 5, " developer", new DateTime(1999, 1, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Agatsya" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
