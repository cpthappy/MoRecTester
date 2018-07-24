using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoRecTester.UI
{
    public partial class MainWindow : Form
    {
        private MainWindowController _Controller;
        public MainWindow(MainWindowController controller)
        {
            InitializeComponent();
            _Controller = controller;
        }
    }
}
