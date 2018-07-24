using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoRecTester.Engine
{
    class UserRating
    {
        /// <summary>
        /// Unique user ID from dataset
        /// </summary>
        public uint UserID { get; private set; }

        /// <summary>
        /// Dictionary with assignment Movie-ID => Numerical rating
        /// </summary>
        private Dictionary<uint, double> MovieRatings { get;  set; }

        public UserRating(uint userID)
        {
            UserID = userID;
            MovieRatings = new Dictionary<uint, double>();
        }

        /// <summary>
        /// Add new rating
        /// </summary>
        /// <param name="movieID">id for movie</param>
        /// <param name="value">rating value</param>
        public void AddRating(uint movieID, double value)
        {
            MovieRatings.Add(movieID, value);
        }

        public List<uint> GetRatedMovies()
        {
            return MovieRatings.Keys.ToList<uint>();
        }

        public bool HasRating(uint movieID)
        {
            return MovieRatings.ContainsKey(movieID);
        }

        public double GetRating(uint movieID)
        {
            if(MovieRatings.ContainsKey(movieID))
            {
                return MovieRatings[movieID];
            }

            throw new KeyNotFoundException($"Die ID {movieID} ist unbekannt");
        }
    }
}
