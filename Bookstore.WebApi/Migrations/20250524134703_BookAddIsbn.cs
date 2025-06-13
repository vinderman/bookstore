using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class BookAddIsbn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Isbn",
                table: "books",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Isbn",
                table: "books");
        }
    }
}
