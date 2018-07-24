using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoRecTester.Engine
{
    public class Movie
    {
        /// <summary>
        /// Unique ID from Dataset
        /// </summary>
        public uint ID { get; private set; }

        /// <summary>
        /// Movie name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// List with genres for this movie
        /// </summary>
        public List<string> Genres { get; private set; }

        public string ImdbLink
        {
            get
            {
                return $"http://www.imdb.com/title/tt{ImdbID}";
            }
        }

        public string ApiLink
        {
            get
            {
                return $"http://sg.media-imdb.com/suggests/t/tt{ImdbID}.json";
            }
        }

        public uint ImdbID { get; private set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="id">Unique ID from Dataset</param>
        /// <param name="name">Movie name</param>
        /// <param name="genre">String with genres separated by | </param>
        public Movie(uint id, string name, string genre, uint imdbID)
        {
            ID = id;
            Name = name;
            Genres = genre.Split('|').ToList<string>();
            ImdbID = imdbID;
        }

        public override string ToString()
        {
            return $"{Name}\t {ImdbLink}";
        }
    }
}
