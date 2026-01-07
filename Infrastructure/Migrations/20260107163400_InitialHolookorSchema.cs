using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HolookorBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialHolookorSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "YearsOfExperience",
                table: "Tutors",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailableStatus",
                table: "Tutors",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailableStatus",
                table: "Tutors");

            migrationBuilder.AlterColumn<string>(
                name: "YearsOfExperience",
                table: "Tutors",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
