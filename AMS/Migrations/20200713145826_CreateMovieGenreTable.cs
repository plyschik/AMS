using Microsoft.EntityFrameworkCore.Migrations;

namespace AMS.Migrations
{
    public partial class CreateMovieGenreTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "movie_genre",
                columns: table => new
                {
                    movie_id = table.Column<int>(nullable: false),
                    genre_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_movie_genre", x => new { x.movie_id, x.genre_id });
                    
                    table.ForeignKey(
                        name: "fk_movie_genre_genres_genre_id",
                        column: x => x.genre_id,
                        principalTable: "genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                    
                    table.ForeignKey(
                        name: "fk_movie_genre_movies_movie_id",
                        column: x => x.movie_id,
                        principalTable: "movies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                });

            migrationBuilder.CreateIndex(
                name: "ix_movie_genre_genre_id",
                table: "movie_genre",
                column: "genre_id"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "movie_genre");
        }
    }
}
