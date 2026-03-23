using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecetArreAPI2.Migrations
{
    /// <inheritdoc />
    public partial class TIngredienteCCreadoCCreadopor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreadoPorUsuarioId",
                table: "Ingredientes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreadoUtc",
                table: "Ingredientes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredientes_CreadoPorUsuarioId",
                table: "Ingredientes",
                column: "CreadoPorUsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredientes_AspNetUsers_CreadoPorUsuarioId",
                table: "Ingredientes",
                column: "CreadoPorUsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredientes_AspNetUsers_CreadoPorUsuarioId",
                table: "Ingredientes");

            migrationBuilder.DropIndex(
                name: "IX_Ingredientes_CreadoPorUsuarioId",
                table: "Ingredientes");

            migrationBuilder.DropColumn(
                name: "CreadoPorUsuarioId",
                table: "Ingredientes");

            migrationBuilder.DropColumn(
                name: "CreadoUtc",
                table: "Ingredientes");
        }
    }
}
