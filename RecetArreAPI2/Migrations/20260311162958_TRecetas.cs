using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecetArreAPI2.Migrations
{
    /// <inheritdoc />
    public partial class TRecetas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecetaId",
                table: "Ingredientes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecetaId",
                table: "Categorias",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Recetas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    instrucciones = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TiempoCoccionMinutos = table.Column<int>(type: "int", nullable: false),
                    TiempoPreparacionMinutos = table.Column<int>(type: "int", nullable: false),
                    Porciones = table.Column<int>(type: "int", nullable: false),
                    EstaPublicado = table.Column<bool>(type: "bit", nullable: false),
                    CreadoUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificadoUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreadoPorUsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recetas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recetas_AspNetUsers_CreadoPorUsuarioId",
                        column: x => x.CreadoPorUsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ingredientes_RecetaId",
                table: "Ingredientes",
                column: "RecetaId");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_RecetaId",
                table: "Categorias",
                column: "RecetaId");

            migrationBuilder.CreateIndex(
                name: "IX_Recetas_CreadoPorUsuarioId",
                table: "Recetas",
                column: "CreadoPorUsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categorias_Recetas_RecetaId",
                table: "Categorias",
                column: "RecetaId",
                principalTable: "Recetas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredientes_Recetas_RecetaId",
                table: "Ingredientes",
                column: "RecetaId",
                principalTable: "Recetas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categorias_Recetas_RecetaId",
                table: "Categorias");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingredientes_Recetas_RecetaId",
                table: "Ingredientes");

            migrationBuilder.DropTable(
                name: "Recetas");

            migrationBuilder.DropIndex(
                name: "IX_Ingredientes_RecetaId",
                table: "Ingredientes");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_RecetaId",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "RecetaId",
                table: "Ingredientes");

            migrationBuilder.DropColumn(
                name: "RecetaId",
                table: "Categorias");
        }
    }
}
