using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AMS.MVC.Migrations
{
    public partial class CreateMovieWritersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovieWriters",
                columns: table => new
                {
                    MovieId = table.Column<Guid>(nullable: false),
                    PersonId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieWriters", x => new {x.MovieId, x.PersonId });
                    
                    table.ForeignKey(
                        name: "FK_MovieWriters_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    
                    table.ForeignKey(
                        name: "FK_MovieWriters_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_MovieWriters_PersonId",
                table: "MovieWriters",
                column: "PersonId"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "MovieWriters");
        }
    }
}
