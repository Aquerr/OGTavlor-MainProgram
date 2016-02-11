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
    /// Interaction logic for ShowPicture.xaml
    /// </summary>
    public partial class ShowPicture : Window
    {
        int PassId;
        public ShowPicture(int _id)
        {
            InitializeComponent();
            PassId = _id;
            LoadArtwork();
            //Artworks artworks = new Artworks();

            //BtnRemove.Click += new EventHandler((x, y) => Artworks.Invnetory.Remove(x => x = PassId));
        }

        private void LoadArtwork()
        {
            var uripath = new Uri((Artworks.Invnetory.Where(x => x.ArtworkId == PassId).Select(y => y.ImagePath).FirstOrDefault()).ToString(), UriKind.RelativeOrAbsolute);
            image.Source = new BitmapImage(uripath);

            TextInfo.Text = (Artworks.Invnetory.Where(x => x.ArtworkId == PassId).Select(y => y.Comment).FirstOrDefault());
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            EditArtwork ea = new EditArtwork(PassId);
            ea.Show();
            this.Close();
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Vill du ta bort detta konstverk?", "Ta bort konstverk",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {

            }
            else
            {
                Artworks.Invnetory.Remove(Artworks.Invnetory.Where(x => x.ArtworkId == PassId).FirstOrDefault());
                MessageBox.Show("Du har nu tagit bort detta konstverk", "Statusmeddelande");
                MainWindow mainWindow = new MainWindow();
                this.Close();
                mainWindow.Show();
            }


        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();

        }
    }
}
