using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstroBlog.Migrations
{
    /// <inheritdoc />
    public partial class person : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PersonId",
                table: "Tags",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "BlogPosts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PersonId",
                table: "BlogPosts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_PersonId",
                table: "Tags",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_PersonId",
                table: "BlogPosts",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_People_PersonId",
                table: "BlogPosts",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_People_PersonId",
                table: "Tags",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_People_PersonId",
                table: "BlogPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_People_PersonId",
                table: "Tags");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropIndex(
                name: "IX_Tags_PersonId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_BlogPosts_PersonId",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "BlogPosts");
        }
    }
}
