using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoRecTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var ratings = IO.DataReader.ReadRatings();
            var movies = IO.DataReader.ReadMovies();

            var recommender = new Engine.Recommender(ratings);
            uint testUser = 2;

            foreach (var movie in ratings[testUser].GetRatedMovies())
            {
                Console.WriteLine($"{movies[movie].Name,-50} {ratings[testUser].GetRating(movie)}");
            }

            foreach (var result in recommender.GetRecommendations(ratings[testUser]).GetRange(0, 10))
            {
                Console.WriteLine($"{result.Item1}: {movies[result.Item2]}");

            }
        }
    }
}
