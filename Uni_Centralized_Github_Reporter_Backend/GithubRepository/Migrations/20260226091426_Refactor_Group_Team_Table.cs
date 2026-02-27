using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GithubReporterRepository.Migrations
{
    /// <inheritdoc />
    public partial class Refactor_Group_Team_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Group_Supervisor",
                table: "GroupTeam");

            migrationBuilder.DropPrimaryKey(
                name: "PK__GroupTea__149AF30A46B8DFB5",
                table: "GroupTeam");

            migrationBuilder.DropColumn(
                name: "GroupID",
                table: "GroupTeam");

            migrationBuilder.DropColumn(
                name: "GroupCode",
                table: "GroupTeam");

            migrationBuilder.DropColumn(
                name: "GroupName",
                table: "GroupTeam");

            migrationBuilder.RenameColumn(
                name: "SupervisorID",
                table: "GroupTeam",
                newName: "SupervisorId");

		

			migrationBuilder.AlterColumn<Guid>(
                name: "SupervisorId",
                table: "GroupTeam",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupTeam",
                table: "GroupTeam",
                columns: new[] { "AccountID", "ProjectID" });

            migrationBuilder.AddForeignKey(
                name: "FK_GroupTeam_Supervisor_SupervisorId",
                table: "GroupTeam",
                column: "SupervisorId",
                principalTable: "Supervisor",
                principalColumn: "SupervisorID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupTeam_Supervisor_SupervisorId",
                table: "GroupTeam");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupTeam",
                table: "GroupTeam");

            migrationBuilder.RenameColumn(
                name: "SupervisorId",
                table: "GroupTeam",
                newName: "SupervisorID");

            migrationBuilder.RenameIndex(
                name: "IX_GroupTeam_SupervisorId",
                table: "GroupTeam",
                newName: "IX_GroupTeam_SupervisorID");

            migrationBuilder.AlterColumn<Guid>(
                name: "SupervisorID",
                table: "GroupTeam",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GroupID",
                table: "GroupTeam",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "GroupCode",
                table: "GroupTeam",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GroupName",
                table: "GroupTeam",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK__GroupTea__149AF30A46B8DFB5",
                table: "GroupTeam",
                column: "GroupID");

            migrationBuilder.AddForeignKey(
                name: "FK_Group_Supervisor",
                table: "GroupTeam",
                column: "SupervisorID",
                principalTable: "Supervisor",
                principalColumn: "SupervisorID");
        }
    }
}
