using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlotMachineAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCreatedDateTimeFromUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");
        }
    }
}
