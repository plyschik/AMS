using Microsoft.EntityFrameworkCore.Migrations;

namespace AMS.Migrations
{
    public partial class CreateMoviePersonWriterTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "movie_person_writers",
                columns: table => new
                {
                    movie_id = table.Column<int>(nullable: false),
                    person_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_movie_person_writers", x => new { x.movie_id, x.person_id });
                    
                    table.ForeignKey(
                        name: "fk_movie_person_writers_movies_movie_id",
                        column: x => x.movie_id,
                        principalTable: "movies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                    
                    table.ForeignKey(
                        name: "fk_movie_person_writers_persons_person_id",
                        column: x => x.person_id,
                        principalTable: "persons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                });

            migrationBuilder.CreateIndex(
                name: "ix_movie_person_writers_person_id",
                table: "movie_person_writers",
                column: "person_id"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "movie_person_writers");
        }
    }
}
