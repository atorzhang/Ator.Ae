using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Ator.Entity.Migrations
{
    public partial class commentChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ToCommentId",
                table: "Sys_Cms_InfoComment",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InfoLable",
                table: "Sys_Cms_Info",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToCommentId",
                table: "Sys_Cms_InfoComment");

            migrationBuilder.AlterColumn<string>(
                name: "InfoLable",
                table: "Sys_Cms_Info",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true);
        }
    }
}
