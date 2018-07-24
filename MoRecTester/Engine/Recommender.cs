
using System;
using System.Collections.Generic;

namespace MoRecTester.Engine
{
    public class Recommender
    {
        /// <summary>
        /// Dictionary with assignment user ID => Rating
        /// </summary>
        public Dictionary<uint, UserRating> UserRatings { get; private set; }

        public Recommender(Dictionary<uint, UserRating> userRatings)
        {
            UserRatings = userRatings;
        }

        /// <summary>
        /// Returns a distance-based similarity score for person1 and person2
        /// </summary>
        /// <param name="user1"></param>
        /// <param name="user2"></param>
        /// <returns></returns>
        public double CalculateSimilarity(UserRating user1, UserRating user2)
        {
            var sharedItems = new List<uint>();
            // Get the list of shared movies
            foreach (var movieId in user1.GetRatedMovies())
            {
                if (user2.HasRating(movieId))
                {
                    sharedItems.Add(movieId);
                }
            }

            // if they have no ratings in common, return 0
            if (sharedItems.Count == 0)
            {
                return 0;
            }

            // Add up the squares of all the differences
            var sumOfSquares = 0.0;
            foreach (var movieID in sharedItems)
            {
                sumOfSquares += Math.Pow(user1.GetRating(movieID) - user2.GetRating(movieID), 2);
            }

            return 1 / (1 + sumOfSquares);
        }

        /// <summary>
        /// Returns the best matches for person from the prefs dictionary.
        /// Number of results and similarity function are optional params.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<Tuple<double, uint>> GetTopMatches(UserRating user, int n)
        {
            var scores = new List<Tuple<double, uint>>();
            foreach (var other in UserRatings.Values)
            {
                if (other.UserID != user.UserID)
                {
                    scores.Add(new Tuple<double, uint>(CalculateSimilarity(user, other), other.UserID));
                }
            }
            // Sort the list so the highest scores appear at the top

            scores.Sort();
            scores.Reverse();
            return scores.GetRange(0, n);
        }

        /// <summary>
        /// Gets recommendations for a person by using a weighted average
        /// of every other user's rankings
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public List<Tuple<double, uint>> GetRecommendations(UserRating person)
        {
            var totals = new Dictionary<uint, double>();
            var simSums = new Dictionary<uint, double>();

            foreach (var other in UserRatings.Values)
            {
                if (other.UserID == person.UserID)
                {
                    continue;
                }
                var similarity = CalculateSimilarity(person, other);
                //# ignore scores of zero or lower
                if (similarity <= 0) { continue; }

                foreach (var item in other.GetRatedMovies())
                {
                    // only score movies I haven't seen yet
                    if (!person.HasRating(item) || person.GetRating(item) == 0)
                    {
                        // Similarity * Score
                        if (totals.ContainsKey(item))
                        {
                            totals[item] += other.GetRating(item) * similarity;
                            simSums[item] += similarity;
                        }
                        else
                        {
                            totals.Add(item, other.GetRating(item) * similarity);
                            simSums.Add(item, similarity);
                        }
                    }
                }
            }

            var rankings = new List<Tuple<double, uint>>();

            // Create the normalized list
            foreach (var item in totals.Keys)
            {
                rankings.Add(new Tuple<double, uint>(totals[item] / simSums[item], item));
            }

            // Return the sorted list
            rankings.Sort();
            rankings.Reverse();
            return rankings;
        }


    }
}