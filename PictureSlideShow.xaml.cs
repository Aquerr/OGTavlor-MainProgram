using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OGTavlor_MainProgram
{
    /// <summary>
    /// Interaction logic for PictureSlideShow.xaml
    /// </summary>
    public partial class PictureSlideShow : Window
    {
        public PictureSlideShow()
        {
            InitializeComponent();
        }

        private void btnMainWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }
    }
}
