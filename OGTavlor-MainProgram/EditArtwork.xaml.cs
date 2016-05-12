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
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System.Configuration;

namespace OGTavlor_MainProgram
{
    /// <summary>
    /// Interaction logic for EditArtwork.xaml
    /// </summary>
    public partial class EditArtwork : Window
    {
        private IArtworkLogic _artworkLogic;
        string _artworkName;
        string _imagePath = "";

        public EditArtwork(string artworkName)
        {
            InitializeComponent();
            IArtworkService service = new ArtworkService();
            IArtworkLogic logic = new ArtworkLogic(service);
            _artworkLogic = logic;

            _artworkName = artworkName;
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
                _imagePath = (ArtImage.Source as BitmapImage).UriSource.AbsolutePath;
            }
        }

        private void SaveArtwork_Click(object sender, RoutedEventArgs e)
        {
            if ((ArtArtist.Text != "") && (ArtName.Text != ""))
            {
                _artworkLogic.ReplaceArtwork(ArtArtist.Text, ArtName.Text, _imagePath, ArtDescription.Text, _artworkName);

                MainWindow Main = new MainWindow();
                this.Close();
                Main.Show();
            }
        }

        private async void FillInfo()
        {

            var art = await _artworkLogic.GetArtworkAsync(_artworkName);

            ArtName.Text = art.Title;
            ArtArtist.Text = art.Artist;
            ArtDescription.Text = art.Description;

            //TODO: Make here anticrashing system. Program shall not crash when it will not find imagepath for an image.
            var uripath = new Uri((art.ImagePath), UriKind.RelativeOrAbsolute);
            ArtImage.Source = new BitmapImage(uripath);
            _imagePath = uripath.ToString();
        }

        private void BackMainWindow_Click(object sender, RoutedEventArgs e)
        {
            var main = new MainWindow();
            this.Close();
            main.Show();
        }
    }
}
