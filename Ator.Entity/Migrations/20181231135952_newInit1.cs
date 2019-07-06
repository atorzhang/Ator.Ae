using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Ator.Entity.Migrations
{
    public partial class newInit1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Sys_Cms_InfoComment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SysCmsColumnId",
                table: "Sys_Cms_InfoComment",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Sys_Cms_InfoComment");

            migrationBuilder.DropColumn(
                name: "SysCmsColumnId",
                table: "Sys_Cms_InfoComment");
        }
    }
}
