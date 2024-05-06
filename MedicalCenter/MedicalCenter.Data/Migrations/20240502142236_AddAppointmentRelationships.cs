using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalCenter.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAppointmentRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorInfos",
                table: "DoctorInfos");

            migrationBuilder.DropIndex(
                name: "IX_DoctorInfos_AppUserId",
                table: "DoctorInfos");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "DoctorInfos",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "PatientId",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorId",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorInfos",
                table: "DoctorInfos",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorInfos_AppUserId",
                table: "DoctorInfos",
                column: "AppUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_TimeSlotId",
                table: "Appointments",
                column: "TimeSlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AspNetUsers_DoctorId",
                table: "Appointments",
                column: "DoctorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AspNetUsers_PatientId",
                table: "Appointments",
                column: "PatientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_TimeSlots_TimeSlotId",
                table: "Appointments",
                column: "TimeSlotId",
                principalTable: "TimeSlots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AspNetUsers_DoctorId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AspNetUsers_PatientId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_TimeSlots_TimeSlotId",
                table: "Appointments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorInfos",
                table: "DoctorInfos");

            migrationBuilder.DropIndex(
                name: "IX_DoctorInfos_AppUserId",
                table: "DoctorInfos");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_TimeSlotId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DoctorInfos");

            migrationBuilder.AlterColumn<string>(
                name: "PatientId",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorId",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorInfos",
                table: "DoctorInfos",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorInfos_AppUserId",
                table: "DoctorInfos",
                column: "AppUserId");
        }
    }
}
