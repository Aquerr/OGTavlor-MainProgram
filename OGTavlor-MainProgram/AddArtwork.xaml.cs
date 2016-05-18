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

        public AddArtwork()
        {
            InitializeComponent();
            IArtworkService service = new ArtworkService();
            IArtworkLogic logic = new ArtworkLogic(service);
            _artworkLogic = logic;

        }
        string _imagePath;


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

                //To save the image to "Images" folder inside the solution.

                //string name = System.IO.Path.GetFileName(_imagePath);
                //string destinationPath = GetDestinationPath(name, "");
                //File.Copy(_imagePath, destinationPath, true);

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

        //To save the file
        private static string GetDestinationPath(string filename, string foldername)
        {
            string appStartPath = System.IO.Path.GetDirectoryName(@"C:\Users\Admin\Desktop\OGTavlor\OGTavlor-MainProgram\OGTavlor-MainProgram\Images\");

            appStartPath = String.Format(appStartPath + "\\{0}\\" + filename, foldername);
            return appStartPath;
        }

        private void BackMainWindow_Click(object sender, RoutedEventArgs e)
        {
            var main = new MainWindow();
            this.Close();
            main.Show();
        }

        
    }
}
