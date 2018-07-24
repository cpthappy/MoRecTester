using System.Collections.Generic;
using System.Globalization;

namespace MoRecTester.IO
{
    static class DataReader
    {
        private const string RATING_PATH = "data\\ratings.csv";
        private const string MOVIES_PATH = "data\\movies.csv";
        private const string LINKS_PATH = "data\\links.csv";


        private static Dictionary<uint, uint> ReadLinks()
        {
            var reader = new CsvReader(LINKS_PATH, true);
            var links = new Dictionary<uint,uint>();
            foreach (var entry in reader.ParseFile())
            {
                links.Add(uint.Parse(entry[0]), uint.Parse(entry[1]));
            }

            return links;
        }
        public static Dictionary<uint, Engine.Movie> ReadMovies()
        {
            var links = ReadLinks();

            var reader = new CsvReader(MOVIES_PATH, true);
            var movies = new Dictionary<uint, Engine.Movie>();
            foreach (var entry in reader.ParseFile())
            {
                var id = uint.Parse(entry[0]);
                var movie = new Engine.Movie(id, entry[1], entry[2], links[id]);
                movies.Add(movie.ID, movie);
            }

            return movies;
        }

        public static Dictionary<uint, Engine.UserRating> ReadRatings()
        {
            var reader = new CsvReader(RATING_PATH, true);
            var ratings = new Dictionary<uint, Engine.UserRating>();
            var lastId = 0u;
            foreach (var entry in reader.ParseFile())
            {
                var userId = uint.Parse(entry[0]);
                if (userId != lastId)
                {
                    ratings.Add(userId, new Engine.UserRating(userId));
                    lastId = userId;
                }
                ratings[userId].AddRating(uint.Parse(entry[1]), double.Parse(entry[2], CultureInfo.InvariantCulture));
            }

            return ratings;
        }
    }
}
