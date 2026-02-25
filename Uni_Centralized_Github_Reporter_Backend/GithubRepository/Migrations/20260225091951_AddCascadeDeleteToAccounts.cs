using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GithubReporterRepository.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDeleteToAccounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			// 1. Drop the existing foreign keys that don't have cascade delete
			migrationBuilder.DropForeignKey(
				name: "FK_Student_Account",
				table: "Student");

			migrationBuilder.DropForeignKey(
				name: "FK_Supervisor_Account",
				table: "Supervisor");

			// 2. Re-add them with OnDelete: ReferentialAction.Cascade
			migrationBuilder.AddForeignKey(
				name: "FK_Student_Account",
				table: "Student",
				column: "AccountID",
				principalTable: "Account",
				principalColumn: "AccountID",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_Supervisor_Account",
				table: "Supervisor",
				column: "AccountID",
				principalTable: "Account",
				principalColumn: "AccountID",
				onDelete: ReferentialAction.Cascade);
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
