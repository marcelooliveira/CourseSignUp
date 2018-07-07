using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CourseSignUp.Migrations
{
    public partial class CourseCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Teachers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Enrollments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Courses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Courses");
        }
    }
}
