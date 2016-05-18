using Microsoft.Win32;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// Interaction logic for AddArtwork.xaml
    /// </summary>
    public partial class AddArtwork : Window
    {
        private readonly IArtworkLogic _artworkLogic;
        private Artwork _artwork;
        private string _imagePath;

        public AddArtwork()
        {
            InitializeComponent();
            IArtworkService service = new ArtworkService();
            IArtworkLogic logic = new ArtworkLogic(service);
            _artworkLogic = logic;
        }

        //To save the artwork if there is an artist, title and an imagepath included
        private void SaveArtwork_Click(object sender, RoutedEventArgs e)
        {
            if ((ArtArtist.Text != "") && (ArtName.Text != "") && (_imagePath != null))
            {
                _artwork = new Artwork(ArtArtist.Text, ArtName.Text);
                _artwork.ImagePath = _imagePath;
                _artwork.Description = ArtDescription.Text;
                _artwork.Room = ArtRoom.Text;
                _artwork.Signed = CheckBoxSigned.IsChecked;
                _artwork.Place = ArtPlace.Text;
                int height;
                int width;
                int.TryParse(ArtHeight.Text, out height);
                int.TryParse(ArtWidth.Text, out width);
                _artwork.Height = height;
                _artwork.Width = width;
                _artworkLogic.SaveArtworkAsync(_artwork);

                var main = new MainWindow();
                this.Close();
                main.Show();
            }
            else
            {
                MessageBox.Show("Du måste ange tavlans titel, konstnär och bifoga bild.");
            }
        }
        //Open the file browser
        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Välj en bild";
            openFileDialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                _imagePath = openFileDialog.FileName;
                ImageSource imgSource = new BitmapImage(new Uri(_imagePath));
                ArtImage.Source = imgSource;              
            }
        }

        //Back to Main Window.
        private void BackMainWindow_Click(object sender, RoutedEventArgs e)
        {
            var main = new MainWindow();
            this.Close();
            main.Show();
        }      
    }
}
