using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sending.API.Migrations
{
    public partial class PlayerSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "Birthday", "Email", "FirstName", "LastName" },
                values: new object[] { 1L, new DateTime(2010, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Email1@www.com", "FirstName_Test1", "LastName_Test1" });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "Birthday", "Email", "FirstName", "LastName" },
                values: new object[] { 2L, new DateTime(2000, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Email2@www.com", "FirstName_Test2", "LastName_Test2" });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "Birthday", "Email", "FirstName", "LastName" },
                values: new object[] { 3L, new DateTime(1990, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Email3@www.com", "FirstName_Test3", "LastName_Test3" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 3L);
        }
    }
}
