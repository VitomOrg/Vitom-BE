using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixModelMaterialsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModelMaterial_Products_ProductId",
                table: "ModelMaterial");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ModelMaterial",
                table: "ModelMaterial");

            migrationBuilder.RenameTable(
                name: "ModelMaterial",
                newName: "ModelMaterials");

            migrationBuilder.RenameIndex(
                name: "IX_ModelMaterial_ProductId",
                table: "ModelMaterials",
                newName: "IX_ModelMaterials_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ModelMaterials",
                table: "ModelMaterials",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelMaterials_Products_ProductId",
                table: "ModelMaterials",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModelMaterials_Products_ProductId",
                table: "ModelMaterials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ModelMaterials",
                table: "ModelMaterials");

            migrationBuilder.RenameTable(
                name: "ModelMaterials",
                newName: "ModelMaterial");

            migrationBuilder.RenameIndex(
                name: "IX_ModelMaterials_ProductId",
                table: "ModelMaterial",
                newName: "IX_ModelMaterial_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ModelMaterial",
                table: "ModelMaterial",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelMaterial_Products_ProductId",
                table: "ModelMaterial",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
