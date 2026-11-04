using AgriShare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AgriShare.Infrastructure.Persistence.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20261104071000_RefactorReviewsForServiceExecutions")]
public sealed class RefactorReviewsForServiceExecutions : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameTable(
            name: "Reviews",
            newName: "LegacyReviews");
        migrationBuilder.Sql("ALTER INDEX \"PK_Reviews\" RENAME TO \"PK_LegacyReviews\"");

        migrationBuilder.CreateTable(
            name: "Reviews",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ServiceExecutionId = table.Column<int>(type: "integer", nullable: false),
                Rating = table.Column<int>(type: "integer", nullable: false),
                Comment = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Reviews", x => x.Id);
                table.CheckConstraint("CK_Reviews_Rating_Range", "\"Rating\" BETWEEN 1 AND 5");
                table.ForeignKey(
                    name: "FK_Reviews_ServiceExecutions_ServiceExecutionId",
                    column: x => x.ServiceExecutionId,
                    principalTable: "ServiceExecutions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Reviews_ServiceExecutionId",
            table: "Reviews",
            column: "ServiceExecutionId",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Reviews");
        migrationBuilder.Sql("ALTER INDEX \"PK_LegacyReviews\" RENAME TO \"PK_Reviews\"");

        migrationBuilder.RenameTable(
            name: "LegacyReviews",
            newName: "Reviews");
    }
}
