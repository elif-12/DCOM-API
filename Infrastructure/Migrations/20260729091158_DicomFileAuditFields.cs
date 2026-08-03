using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DCOM_API.Migrations
{
    /// <inheritdoc />
    public partial class DicomFileAuditFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "DicomFiles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorUserId",
                table: "DicomFiles",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterUserId",
                table: "DicomFiles",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicomFiles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "DicomFiles",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "DicomFiles");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "DicomFiles");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "DicomFiles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicomFiles");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "DicomFiles");
        }
    }
}
