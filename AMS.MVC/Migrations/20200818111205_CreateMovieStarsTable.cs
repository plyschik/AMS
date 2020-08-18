using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AMS.MVC.Migrations
{
    public partial class CreateMovieStarsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovieStars",
                columns: table => new
                {
                    MovieId = table.Column<Guid>(nullable: false),
                    PersonId = table.Column<Guid>(nullable: false),
                    Character = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieStars", x => new { x.MovieId, x.PersonId });
                    
                    table.ForeignKey(
                        name: "FK_MovieStars_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    
                    table.ForeignKey(
                        name: "FK_MovieStars_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_MovieStars_PersonId",
                table: "MovieStars",
                column: "PersonId"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "MovieStars");
        }
    }
}
