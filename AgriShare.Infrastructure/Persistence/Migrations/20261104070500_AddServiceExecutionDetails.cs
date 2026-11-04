using AgriShare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgriShare.Infrastructure.Persistence.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20261104070500_AddServiceExecutionDetails")]
public sealed class AddServiceExecutionDetails : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<decimal>(
            name: "ActualSize",
            table: "ServiceExecutions",
            type: "numeric(18,4)",
            precision: 18,
            scale: 4,
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "OperatorId",
            table: "ServiceExecutions",
            type: "integer",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "Status",
            table: "ServiceExecutions",
            type: "integer",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.Sql("""
            UPDATE "ServiceExecutions"
            SET "Status" = CASE
                WHEN "CompletedAt" IS NOT NULL THEN 2
                WHEN "StartedAt" IS NOT NULL THEN 1
                ELSE 0
            END
            """);

        migrationBuilder.CreateIndex(
            name: "IX_ServiceExecutions_OperatorId",
            table: "ServiceExecutions",
            column: "OperatorId");

        migrationBuilder.AddForeignKey(
            name: "FK_ServiceExecutions_Operators_OperatorId",
            table: "ServiceExecutions",
            column: "OperatorId",
            principalTable: "Operators",
            principalColumn: "Id",
            onDelete: ReferentialAction.SetNull);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ServiceExecutions_Operators_OperatorId",
            table: "ServiceExecutions");

        migrationBuilder.DropIndex(
            name: "IX_ServiceExecutions_OperatorId",
            table: "ServiceExecutions");

        migrationBuilder.DropColumn(
            name: "ActualSize",
            table: "ServiceExecutions");

        migrationBuilder.DropColumn(
            name: "OperatorId",
            table: "ServiceExecutions");

        migrationBuilder.DropColumn(
            name: "Status",
            table: "ServiceExecutions");
    }
}
