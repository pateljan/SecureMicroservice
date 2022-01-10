using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Models;

namespace Web.Api.Data
{
    public class MoviesContextSeed
    {
        public static void SeedAsync(MovieContext moviesContext)
        {
            if (!moviesContext.Movie.Any())
            {
                var movies = new List<Movie>
                {
                    new Movie
                    {
                        Id=1,
                        Genre="Drama",
                        Title="The Shawshank Redemption",
                        Rating="9.3",
                        ImageUrl="images/src",
                        ReleaseDate = new DateTime(1994,5,5),
                        owner="alice"
                    },
                    new Movie
                    {
                        Id=2,
                        Genre="Crime",
                        Title="The Godfather",
                        Rating="9.2",
                        ImageUrl="images/src",
                        ReleaseDate = new DateTime(1972,5,3),
                        owner="alice"
                    },
                    new Movie
                    {
                        Id=3,
                        Genre="Action",
                        Title="Die Hard",
                        Rating="9.7",
                        ImageUrl="images/src",
                        ReleaseDate = new DateTime(1988,2,5),
                        owner="alice"
                    },
                    new Movie
                    {
                        Id=4,
                        Genre="Action",
                        Title="The Dark Knight",
                        Rating="9.3",
                        ImageUrl="images/src",
                        ReleaseDate = new DateTime(2001,4,1),
                        owner="bob"
                    },
                    new Movie
                    {
                        Id=5,
                        Genre="Biography",
                        Title="Schidler's List",
                        Rating="8.9",
                        ImageUrl="images/src",
                        ReleaseDate = new DateTime(1993,5,5),
                        owner="alice"
                    },
                    new Movie
                    {
                        Id=6,
                        Genre="Drama",
                        Title="Pulp Fiction",
                        Rating="8.8",
                        ImageUrl="images/src",
                        ReleaseDate = new DateTime(1999,4,3),
                        owner="alice"
                    },
                    new Movie
                    {
                        Id=7,
                        Genre="Drama",
                        Title="Fight Club",
                        Rating="8.3",
                        ImageUrl="images/src",
                        ReleaseDate = new DateTime(1998,4,2),
                        owner="bob"
                    },
                    new Movie
                    {
                        Id=8,
                        Genre="Romance",
                        Title="Forrest Gump",
                        Rating="8.8",
                        ImageUrl="images/src",
                        ReleaseDate = new DateTime(1994,3,7),
                        owner="bob"
                    },
                };

                moviesContext.Movie.AddRange(movies);
                moviesContext.SaveChangesAsync();
            }
        }
    }
}
