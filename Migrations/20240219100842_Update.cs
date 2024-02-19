using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeBankingMinHub.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "Loans");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "Loans",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
