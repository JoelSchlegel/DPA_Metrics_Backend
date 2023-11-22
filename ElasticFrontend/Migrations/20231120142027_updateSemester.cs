﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElasticFrontend.Migrations
{
    public partial class updateSemester : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Semester",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Semester");
        }
    }
}
