using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlotMachineAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddModifiedDateTimeToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "Users",
                type: "TEXT",
                nullable: true);
            
            // Update all existing records with current timestamp
            migrationBuilder.Sql("UPDATE Users SET ModifiedDateTime = datetime('now')");
            
            // Now make it non-nullable
            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: DateTime.UtcNow);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "Users");
        }
    }
}
