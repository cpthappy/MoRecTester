using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace MoRecTester
{
    class Program
    {
        static void Main()
        {
            Application.EnableVisualStyles();
            var ratings = IO.DataReader.ReadRatings();
            var movies = IO.DataReader.ReadMovies();
            var recommender = new Engine.Recommender(ratings);
            var controller = new UI.MainWindowController(movies, recommender);
            var mainWindow = new UI.MainWindow(controller);
            Application.Run(mainWindow);

        }
    }
}
