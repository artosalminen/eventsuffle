using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eventsuffle.Infrastructure.Migrations.MsSqlMigrations
{
    public partial class CreateTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EVENT",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENT", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SUGGESTED_DATE",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EventId = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SUGGESTED_DATE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SUGGESTED_DATE_EVENT_EventId",
                        column: x => x.EventId,
                        principalTable: "EVENT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VOTE",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PersonName = table.Column<string>(maxLength: 500, nullable: false),
                    EventId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VOTE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VOTE_EVENT_EventId",
                        column: x => x.EventId,
                        principalTable: "EVENT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VOTE_SUGGESTED_DATE",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    VoteId = table.Column<Guid>(nullable: false),
                    SuggestedDateId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VOTE_SUGGESTED_DATE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VOTE_SUGGESTED_DATE_SUGGESTED_DATE_SuggestedDateId",
                        column: x => x.SuggestedDateId,
                        principalTable: "SUGGESTED_DATE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VOTE_SUGGESTED_DATE_VOTE_VoteId",
                        column: x => x.VoteId,
                        principalTable: "VOTE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EVENT_Id",
                table: "EVENT",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SUGGESTED_DATE_Id",
                table: "SUGGESTED_DATE",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SUGGESTED_DATE_EventId_Date",
                table: "SUGGESTED_DATE",
                columns: new[] { "EventId", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VOTE_EventId",
                table: "VOTE",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_VOTE_Id",
                table: "VOTE",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VOTE_SUGGESTED_DATE_Id",
                table: "VOTE_SUGGESTED_DATE",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VOTE_SUGGESTED_DATE_SuggestedDateId",
                table: "VOTE_SUGGESTED_DATE",
                column: "SuggestedDateId");

            migrationBuilder.CreateIndex(
                name: "IX_VOTE_SUGGESTED_DATE_VoteId_SuggestedDateId",
                table: "VOTE_SUGGESTED_DATE",
                columns: new[] { "VoteId", "SuggestedDateId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VOTE_SUGGESTED_DATE");

            migrationBuilder.DropTable(
                name: "SUGGESTED_DATE");

            migrationBuilder.DropTable(
                name: "VOTE");

            migrationBuilder.DropTable(
                name: "EVENT");
        }
    }
}
