using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteEdgesOnNodeRemoval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Edges_Nodes_SourceNodeId",
                table: "Edges");

            migrationBuilder.DropForeignKey(
                name: "FK_Edges_Nodes_TargetNodeId",
                table: "Edges");

            migrationBuilder.AddForeignKey(
                name: "FK_Edges_Nodes_SourceNodeId",
                table: "Edges",
                column: "SourceNodeId",
                principalTable: "Nodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Edges_Nodes_TargetNodeId",
                table: "Edges",
                column: "TargetNodeId",
                principalTable: "Nodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Edges_Nodes_SourceNodeId",
                table: "Edges");

            migrationBuilder.DropForeignKey(
                name: "FK_Edges_Nodes_TargetNodeId",
                table: "Edges");

            migrationBuilder.AddForeignKey(
                name: "FK_Edges_Nodes_SourceNodeId",
                table: "Edges",
                column: "SourceNodeId",
                principalTable: "Nodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Edges_Nodes_TargetNodeId",
                table: "Edges",
                column: "TargetNodeId",
                principalTable: "Nodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
