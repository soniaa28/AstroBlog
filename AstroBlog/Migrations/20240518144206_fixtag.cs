using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstroBlog.Migrations
{
    /// <inheritdoc />
    public partial class fixtag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_People_PersonId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_PersonId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Tags");

            migrationBuilder.CreateTable(
                name: "PersonTag",
                columns: table => new
                {
                    PersonsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonTag", x => new { x.PersonsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_PersonTag_People_PersonsId",
                        column: x => x.PersonsId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonTag_TagsId",
                table: "PersonTag",
                column: "TagsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonTag");

            migrationBuilder.AddColumn<Guid>(
                name: "PersonId",
                table: "Tags",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_PersonId",
                table: "Tags",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_People_PersonId",
                table: "Tags",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id");
        }
    }
}
