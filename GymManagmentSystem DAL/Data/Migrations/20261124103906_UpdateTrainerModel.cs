using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagmentSystem_DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTrainerModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Specialties",
                table: "Trainers",
                newName: "Specialization");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Specialization",
                table: "Trainers",
                newName: "Specialties");
        }
    }
}
