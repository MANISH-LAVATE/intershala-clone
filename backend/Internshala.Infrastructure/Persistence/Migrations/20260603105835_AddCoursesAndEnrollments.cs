using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Internshala.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCoursesAndEnrollments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Instructor = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Level = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    DurationHours = table.Column<int>(type: "int", nullable: false),
                    IsFree = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    EnrolledCount = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Prerequisites = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WhatYouLearn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseModules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    IsPreview = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseModules_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    ProgressPercent = table.Column<byte>(type: "tinyint", nullable: false),
                    CompletedModules = table.Column<int>(type: "int", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastAccessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enrollments_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6446), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6447) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6450), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6450) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6451), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6452) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6452), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6453) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6453), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6453) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6454), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6454) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6455), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6455) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6456), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6456) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6457), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6457) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6457), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6458) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6458), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6458) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6459), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6459) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6460), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6460) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6460), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6461) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6461), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6461) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6462), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6462) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6463), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6463) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6464), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6464) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6464), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6464) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6465), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6465) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6563), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6563) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6564), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6565) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6565), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6566) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6566), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6566) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6567), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6567) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6568), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6568) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6569), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6569) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6569), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6570) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6570), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6570) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6571), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6571) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6572), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6572) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6573), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6573) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6573), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6573) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6574), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6574) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6575), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6575) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6576), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6576) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6576), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6577) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6577), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6577) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6578), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6578) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6579), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6579) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6603), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6603) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6604), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6604) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6604), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6604) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6605), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6605) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6606), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6606) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6634), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6634) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6637), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6637) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6638), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6638) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6639), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6639) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6640), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6640) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6641), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6641) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6642), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6642) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6643), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6643) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6644), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6644) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6645), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6645) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6645), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6646) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6646), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6647) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6647), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6648) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6648), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6648) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6649), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6649) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6650), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6650) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6651), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6651) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6652), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6652) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6653), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6653) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6654), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6654) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6655), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6655) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6656), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6656) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6657), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6657) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6658), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6658) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6658), new DateTime(2026, 6, 3, 10, 58, 33, 209, DateTimeKind.Utc).AddTicks(6659) });

            migrationBuilder.CreateIndex(
                name: "IX_CourseModules_CourseId_OrderIndex",
                table: "CourseModules",
                columns: new[] { "CourseId", "OrderIndex" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CategoryId",
                table: "Courses",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_IsDeleted_IsPublished_CategoryId",
                table: "Courses",
                columns: new[] { "IsDeleted", "IsPublished", "CategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_CourseId",
                table: "Enrollments",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_StudentId_CourseId",
                table: "Enrollments",
                columns: new[] { "StudentId", "CourseId" },
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseModules");

            migrationBuilder.DropTable(
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3197), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3206) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3215), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3216) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3217), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3218) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3219), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3219) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3221), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3221) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3222), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3223) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3224), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3224) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3226), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3226) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3227), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3228) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3229), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3229) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3230), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3231) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3232), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3232) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3234), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3234) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3235), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3236) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3239), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3250) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3251), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3252) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3253), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3253) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3255), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3255) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3256), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3256) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3258), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3258) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3909), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3909) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3914), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3914) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3916), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3916) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3918), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3918) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3919), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3920) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3921), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3921) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3923), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3923) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3926), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3927) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3928), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3928) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3929), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3930) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3931), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3931) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3933), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3933) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3934), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3935) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3936), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3936) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3937), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3938) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3939), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3939) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3941), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3941) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3942), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3942) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3944), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3944) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3945), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(3945) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4009), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4010) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4011), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4012) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4013), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4014) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4015), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4015) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4017), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4017) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4149), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4149) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4154), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4154) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4156), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4157) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4158), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4159) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4160), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4161) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4162), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4163) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4164), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4165) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4166), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4167) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4168), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4169) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4170), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4171) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4172), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4173) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4174), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4175) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4176), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4177) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4178), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4179) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4180), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4181) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4182), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4183) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4185), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4185) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4186), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4187) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4188), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4189) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4190), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4191) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4192), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4193) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4194), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4195) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4196), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4197) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4198), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4198) });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4200), new DateTime(2026, 5, 30, 6, 20, 22, 186, DateTimeKind.Utc).AddTicks(4200) });
        }
    }
}
