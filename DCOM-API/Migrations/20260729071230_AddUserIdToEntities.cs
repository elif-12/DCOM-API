using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DCOM_API.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Studies_StudyInstanceUid",
                table: "Studies");

            migrationBuilder.DropIndex(
                name: "IX_Series_SeriesInstanceUid",
                table: "Series");

            migrationBuilder.DropIndex(
                name: "IX_Patients_PatientId",
                table: "Patients");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Studies",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Series",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Patients",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "DicomFiles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Studies_UserId_StudyInstanceUid",
                table: "Studies",
                columns: new[] { "UserId", "StudyInstanceUid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Series_UserId_SeriesInstanceUid",
                table: "Series",
                columns: new[] { "UserId", "SeriesInstanceUid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_UserId_PatientId",
                table: "Patients",
                columns: new[] { "UserId", "PatientId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Studies_UserId_StudyInstanceUid",
                table: "Studies");

            migrationBuilder.DropIndex(
                name: "IX_Series_UserId_SeriesInstanceUid",
                table: "Series");

            migrationBuilder.DropIndex(
                name: "IX_Patients_UserId_PatientId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Studies");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DicomFiles");

            migrationBuilder.CreateIndex(
                name: "IX_Studies_StudyInstanceUid",
                table: "Studies",
                column: "StudyInstanceUid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Series_SeriesInstanceUid",
                table: "Series",
                column: "SeriesInstanceUid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PatientId",
                table: "Patients",
                column: "PatientId",
                unique: true);
        }
    }
}
