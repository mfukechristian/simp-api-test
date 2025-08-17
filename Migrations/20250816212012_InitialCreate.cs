using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sims.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UploadedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    MSISDN = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    MSISDN_Int = table.Column<long>(type: "bigint", nullable: false),
                    MSISDN_Num = table.Column<long>(type: "bigint", nullable: false),
                    IMEI = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    SIMNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SIMStatus = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Locked = table.Column<bool>(type: "bit", nullable: true),
                    LockDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CanLock = table.Column<bool>(type: "bit", nullable: false),
                    DataSetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sims_DataSets_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "DataSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sims_DataSetId",
                table: "Sims",
                column: "DataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Sims_MSISDN",
                table: "Sims",
                column: "MSISDN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sims_SIMNumber",
                table: "Sims",
                column: "SIMNumber",
                unique: true,
                filter: "[SIMNumber] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sims");

            migrationBuilder.DropTable(
                name: "DataSets");
        }
    }
}
