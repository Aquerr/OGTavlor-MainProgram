using Microsoft.Win32;
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
    /// Interaction logic for EditArtwork.xaml
    /// </summary>
    public partial class EditArtwork : Window
    {
        int PassId;
        string ImagePath = "";

        public EditArtwork(int _id)
        {
            InitializeComponent();
            PassId = _id;
            FillInfo();
        }

        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                ArtImage.Source = new BitmapImage(new Uri(op.FileName));
                ImagePath = (ArtImage.Source as BitmapImage).UriSource.AbsolutePath;
            }
        }

        private void SaveArtwork_Click(object sender, RoutedEventArgs e)
        {
            Artworks.Invnetory[PassId-1].Title = ArtName.Text;
            Artworks.Invnetory[PassId-1].Artist = ArtArtist.Text;
            Artworks.Invnetory[PassId-1].ImagePath = ImagePath;

            MainWindow Main = new MainWindow();
            this.Close();
            Main.Show();
        }

        private void FillInfo()
        {

            ArtName.Text = (Artworks.Invnetory.Where(x => x.ArtworkId == PassId).Select(y => y.Title).FirstOrDefault());
            ArtArtist.Text = (Artworks.Invnetory.Where(x => x.ArtworkId == PassId).Select(y => y.Artist).FirstOrDefault());

            var uripath = new Uri((Artworks.Invnetory.Where(x => x.ArtworkId == PassId).Select(y => y.ImagePath).FirstOrDefault()).ToString(), UriKind.RelativeOrAbsolute);
            ArtImage.Source = new BitmapImage(uripath);
            ImagePath = uripath.ToString();
        }
    }
}
