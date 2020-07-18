using Microsoft.EntityFrameworkCore.Migrations;

namespace AMS.Migrations
{
    public partial class CreateMoviePersonStarTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "movie_person_stars",
                columns: table => new
                {
                    movie_id = table.Column<int>(nullable: false),
                    person_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_movie_person_stars", x => new { x.movie_id, x.person_id });
                    
                    table.ForeignKey(
                        name: "fk_movie_person_stars_movies_movie_id",
                        column: x => x.movie_id,
                        principalTable: "movies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                    
                    table.ForeignKey(
                        name: "fk_movie_person_stars_persons_person_id",
                        column: x => x.person_id,
                        principalTable: "persons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_movie_person_stars_person_id",
                table: "movie_person_stars",
                column: "person_id"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "movie_person_stars");
        }
    }
}
