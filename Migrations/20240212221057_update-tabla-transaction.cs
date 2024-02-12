using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeBankingMinHub.Migrations
{
    /// <inheritdoc />
    public partial class updatetablatransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ammount",
                table: "Transactions",
                newName: "Amount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Transactions",
                newName: "Ammount");
        }
    }
}
