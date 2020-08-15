using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AMS.MVC.Migrations
{
    public partial class CreateMovieDirectorsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovieDirectors",
                columns: table => new
                {
                    MovieId = table.Column<Guid>(nullable: false),
                    PersonId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieDirectors", x => new { x.MovieId, x.PersonId });
                    
                    table.ForeignKey(
                        name: "FK_MovieDirectors_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    
                    table.ForeignKey(
                        name: "FK_MovieDirectors_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_MovieDirectors_PersonId",
                table: "MovieDirectors",
                column: "PersonId"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "MovieDirectors");
        }
    }
}
