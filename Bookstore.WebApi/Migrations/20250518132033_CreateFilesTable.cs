using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class CreateFilesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "file_id",
                table: "books");

            migrationBuilder.DropColumn(
                name: "file_name",
                table: "books");

            migrationBuilder.CreateTable(
                name: "files",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    file_name = table.Column<string>(type: "text", nullable: false),
                    file_hash = table.Column<string>(type: "text", nullable: false),
                    file_size = table.Column<int>(type: "integer", nullable: false),
                    file_type = table.Column<string>(type: "text", nullable: false),
                    s3_url = table.Column<string>(type: "text", nullable: false),
                    uploaded_at = table.Column<DateTime>(type: "date", nullable: false),
                    book_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("files_pkey", x => x.id);
                    table.ForeignKey(
                        name: "FK_files_books_book_id",
                        column: x => x.book_id,
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "files");

            migrationBuilder.AddColumn<string>(
                name: "file_id",
                table: "books",
                type: "character varying",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "file_name",
                table: "books",
                type: "character varying",
                maxLength: 100,
                nullable: true);
        }
    }
}
