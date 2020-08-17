// project=Data, file=20200724085954_InitialMigration.cs, create=10:59 Copyright (c) 2020
// tuke productions. All rights reserved.
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Object");

            migrationBuilder.DropTable(
                name: "Process");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Library");

            migrationBuilder.DropTable(
                name: "Target");

            migrationBuilder.DropTable(
                name: "Workspace");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    DarkMode = table.Column<bool>(nullable: false),
                    PrimaryColor = table.Column<string>(nullable: false),
                    SecondaryColor = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workspace",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Directory = table.Column<string>(nullable: true),
                    File = table.Column<string>(nullable: true),
                    PBVersion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workspace", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Target",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Directory = table.Column<string>(nullable: true),
                    File = table.Column<string>(nullable: true),
                    WorkspaceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Target", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Target_Workspace_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Library",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Directory = table.Column<string>(nullable: true),
                    File = table.Column<string>(nullable: true),
                    Build = table.Column<bool>(nullable: false),
                    TargetId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Library", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Library_Target_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Target",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Process",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    TargetId = table.Column<int>(nullable: true),
                    Error = table.Column<int>(nullable: true),
                    Sucess = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Process", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Process_Target_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Target",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Object",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Regenerate = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ObjectType = table.Column<string>(nullable: true),
                    LibraryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Object", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Object_Library_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Library",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "CreatedDate", "DarkMode", "PrimaryColor", "SecondaryColor", "UpdatedDate" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "teal", "cyan", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_Library_TargetId",
                table: "Library",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_Object_LibraryId",
                table: "Object",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Process_TargetId",
                table: "Process",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_Target_WorkspaceId",
                table: "Target",
                column: "WorkspaceId");
        }
    }
}