using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace RandomCoffee.Database.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "meeting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_meeting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "person",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Department = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "topic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topic", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "personmeeting",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    MeetingId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personmeeting", x => new { x.PersonId, x.MeetingId });
                    table.ForeignKey(
                        name: "FK_personmeeting_meeting_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "meeting",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_personmeeting_person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "persontopic",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    TopicId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persontopic", x => new { x.PersonId, x.TopicId });
                    table.ForeignKey(
                        name: "FK_persontopic_person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_persontopic_topic_TopicId",
                        column: x => x.TopicId,
                        principalTable: "topic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_personmeeting_MeetingId",
                table: "personmeeting",
                column: "MeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_persontopic_TopicId",
                table: "persontopic",
                column: "TopicId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "personmeeting");

            migrationBuilder.DropTable(
                name: "persontopic");

            migrationBuilder.DropTable(
                name: "meeting");

            migrationBuilder.DropTable(
                name: "person");

            migrationBuilder.DropTable(
                name: "topic");
        }
    }
}
