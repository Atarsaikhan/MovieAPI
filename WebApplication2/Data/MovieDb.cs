using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.Data
{
    public class MovieDb
    {
        // movies
        private static Movie[] movies = new Movie[]
        {
            new Movie { Id = 1, Title = "Interstellar", Genre = "Science fiction", Year = 2015, RunningTime = 120 },
            new Movie { Id = 2, Title = "The Matrix", Genre = "Triller", Year = 1999, RunningTime = 110 },
            new Movie { Id = 3, Title = "Titanic", Genre = "Drama", Year = 1997, RunningTime = 100 },
            new Movie { Id = 4, Title = "Indian Jones", Genre = "Action", Year = 2015, RunningTime = 90 },
            new Movie { Id = 5, Title = "Pretty women", Genre = "Drama", Year = 1999, RunningTime = 105 },
            new Movie { Id = 6, Title = "The Lier", Genre = "Comedy", Year = 1997, RunningTime = 100 }
        };

        // user ratings
        private static MovieRating[] ratings = new MovieRating[]
        {
            new MovieRating { MovieId = 1, UserId = 1, UserRating = 4 },
            new MovieRating { MovieId = 1, UserId = 2, UserRating = 5 },
            new MovieRating { MovieId = 1, UserId = 3, UserRating = 5 },
            new MovieRating { MovieId = 2, UserId = 1, UserRating = 5 },
            new MovieRating { MovieId = 2, UserId = 2, UserRating = 3 },
            new MovieRating { MovieId = 3, UserId = 1, UserRating = 4 },
            new MovieRating { MovieId = 3, UserId = 2, UserRating = 5 },
            new MovieRating { MovieId = 3, UserId = 3, UserRating = 5 },
            new MovieRating { MovieId = 3, UserId = 4, UserRating = 5 },
            new MovieRating { MovieId = 4, UserId = 1, UserRating = 4 },
            new MovieRating { MovieId = 4, UserId = 2, UserRating = 5 },
            new MovieRating { MovieId = 4, UserId = 3, UserRating = 5 },
            new MovieRating { MovieId = 5, UserId = 1, UserRating = 3 },
            new MovieRating { MovieId = 5, UserId = 2, UserRating = 4 },
            new MovieRating { MovieId = 6, UserId = 1, UserRating = 5 },
            new MovieRating { MovieId = 6, UserId = 2, UserRating = 4 },
            new MovieRating { MovieId = 6, UserId = 3, UserRating = 3 },
            new MovieRating { MovieId = 6, UserId = 4, UserRating = 4 }
        };

        public static void UpdateMovieRating(int mid, int uid, double rate)
        {
            MovieRating mr = new MovieRating { MovieId = mid, UserId = uid, UserRating = rate };
            bool insert = true;
            for(int i = 0; i < ratings.Length; i++)            
                if (ratings[i].MovieId == mid && ratings[i].UserId == uid)
                {
                    ratings[i] = mr;
                    insert = false;
                    break;
                }
            if (insert)
            {
                Array.Resize(ref ratings, ratings.Length + 1);
                ratings[ratings.GetUpperBound(0)] = mr;
            }
        }
        
        public static IQueryable<MoviePresentation> GetMoviePresentations(int TopN, int Id)
        {
            var MoviePresentations =
                from rating in ratings
                join movie in movies on rating.MovieId equals movie.Id
                where rating.UserId == Id || Id == -1 //filtering by user id
                group rating by movie into g
                select new MoviePresentation
                {
                    Id = g.Key.Id,
                    Title = g.Key.Title,
                    Year = g.Key.Year,
                    Genre = g.Key.Genre,
                    RunningTime = g.Key.RunningTime,
                    Rating = Math.Round(g.Average(x => x.UserRating) * 2) / 2
                };
            
            //Returning top 5
            return MoviePresentations.OrderByDescending(x => x.Rating).Take(TopN).AsQueryable();
        }

        public static IQueryable<MoviePresentation> GetMoviePresentations(string criteria, string value)
        {
            // filtering movies by criteria
            var TempMovies = from movie in movies select movie;
            if (criteria.Equals("title"))
                TempMovies = TempMovies.Where(x => x.Title.ToLower().Contains(value));
            else if (criteria.Equals("year"))
                TempMovies = TempMovies.Where(x => x.Year == Convert.ToInt32(value));
            else if (criteria.Equals("genre"))
                TempMovies = TempMovies.Where(x => x.Genre.ToLower().Contains(value));

            //prefaring movies to present
            var MoviePresentations =
                from rating in ratings
                join movie in TempMovies on rating.MovieId equals movie.Id
                group rating by movie into g
                select new MoviePresentation
                {
                    Id = g.Key.Id,
                    Title = g.Key.Title,
                    Year = g.Key.Year,
                    Genre = g.Key.Genre,
                    RunningTime = g.Key.RunningTime,
                    Rating = Math.Round(g.Average(x => x.UserRating) * 2) / 2
                };

            return MoviePresentations.AsQueryable();
        }

    }
}