using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Data.Migrations
{
    public partial class SamllFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Jurnals",
                table: "Jurnals");

            migrationBuilder.RenameTable(
                name: "Jurnals",
                newName: "Journals");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Journals",
                table: "Journals",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Journals",
                table: "Journals");

            migrationBuilder.RenameTable(
                name: "Journals",
                newName: "Jurnals");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Jurnals",
                table: "Jurnals",
                column: "Id");
        }
    }
}
