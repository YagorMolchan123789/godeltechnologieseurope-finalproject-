using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalCenter.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotosToDoctor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "DoctorInfos",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "DoctorInfos");
        }
    }
}
