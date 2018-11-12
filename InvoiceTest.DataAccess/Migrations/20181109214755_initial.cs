using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace InvoiceTest.DataAccess.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateLastUpdated = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<Guid>(nullable: false),
                    LastUpdatedByUserId = table.Column<Guid>(nullable: true),
                    Identifier = table.Column<string>(maxLength: 400, nullable: true),
                    Amount = table.Column<decimal>(type: "Money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateLastUpdated = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<Guid>(nullable: false),
                    LastUpdatedByUserId = table.Column<Guid>(nullable: true),
                    ApiKey = table.Column<string>(maxLength: 400, nullable: true),
                    Role = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.Sql("INSERT INTO Users (ApiKey, Role, Id, DateCreated, CreatedByUserId) VALUES ('admin345', 20, newid(), getdate(), '00000000-0000-0000-0000-000000000000')");
            migrationBuilder.Sql("INSERT INTO Users (ApiKey, Role, Id, DateCreated, CreatedByUserId) VALUES ('user123',  10, newid(), getdate(), '00000000-0000-0000-0000-000000000000')");
            migrationBuilder.Sql("INSERT INTO Users (ApiKey, Role, Id, DateCreated, CreatedByUserId) VALUES ('admin123', 20, newid(), getdate(), '00000000-0000-0000-0000-000000000000')");
            migrationBuilder.Sql("INSERT INTO Users (ApiKey, Role, Id, DateCreated, CreatedByUserId) VALUES ('user345',  10, newid(), getdate(), '00000000-0000-0000-0000-000000000000')");

            migrationBuilder.CreateTable(
                name: "InvoiceNotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateLastUpdated = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<Guid>(nullable: false),
                    LastUpdatedByUserId = table.Column<Guid>(nullable: true),
                    InvoiceId = table.Column<Guid>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceNotes_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceNotes_InvoiceId",
                table: "InvoiceNotes",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_Identifier",
                table: "Invoices",
                column: "Identifier",
                unique: true,
                filter: "[Identifier] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ApiKey",
                table: "Users",
                column: "ApiKey",
                unique: true,
                filter: "[ApiKey] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceNotes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Invoices");
        }
    }
}
