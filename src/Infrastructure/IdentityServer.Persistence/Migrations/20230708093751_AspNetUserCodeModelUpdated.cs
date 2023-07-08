using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityServer.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AspNetUserCodeModelUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "AspNetUserCodes");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "AspNetUserCodes",
                type: "character varying(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "AspNetUserCodes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(6)",
                oldMaxLength: 6);

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "AspNetUserCodes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
