using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wiwi.ScheduleCenter.Api.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ScheduleModel",
                columns: table => new
                {
                    schedule_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, comment: "主键")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    schedule_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    remark = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cron_expression = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<int>(type: "int", maxLength: 5, nullable: false),
                    last_run_time = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    next_run_time = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    start_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    end_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    deleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    request_url = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    body = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    create_time = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间"),
                    creater = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "", comment: "创建人")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    create_id = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "", comment: "创建人Id")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    last_modify_time = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "最后修改时间"),
                    last_modifier = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "", comment: "最后修改人")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    last_modifier_id = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "", comment: "最后修改人Id")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleModel", x => x.schedule_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduleModel");
        }
    }
}
