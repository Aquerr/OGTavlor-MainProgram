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
            LoadComboBox();

        }
        string _imagePath;


        private void SaveArtwork_Click(object sender, RoutedEventArgs e)
        {
            if ((ArtArtist.Text != "") && (ArtName.Text != ""))
            {
                _artwork = new Artwork(ArtArtist.Text, ArtName.Text);
                _artwork.ImagePath = _imagePath;
                _artwork.Description = ArtDescription.Text;
                _artwork.Place = ArtPlace.Text;
                _artworkLogic.SaveArtworkAsync(_artwork);

                //To add the image to \OGTavlor-MainProgram\OGTavlor-MainProgram\bin\Debug\Images map
                string name = System.IO.Path.GetFileName(_imagePath);
                string destinationPath = GetDestinationPath(name, "Images");
                File.Copy(_imagePath, destinationPath, true);
                //

                var main = new MainWindow();
                this.Close();
                main.Show();
            }
            else
            {
                MessageBox.Show("Du måste skriva in tavlans namn och konstnär.");
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
        private static String GetDestinationPath(string filename, string foldername)
        {
            string appStartPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            appStartPath = String.Format(appStartPath + "\\{0}\\" + filename, foldername);
            return appStartPath;
        }


        private void btnAddArtist_Click(object sender, RoutedEventArgs e)
        {
            //var myArtist = new AddArtist();
            //this.Close();
            //myArtist.Show();
        }

        private void LoadComboBox()
        {


            //CmBxArtistName.ItemsSource = Artworks.Invnetory.Distinct();
            //CmBxArtistName.DisplayMemberPath = "Artist";


            //for (int i = 0; i < CmBxArtistName.Items.Count; i++)
            //{
            //    for (int y = 0; y < CmBxArtistName.Items.Count; y++)
            //    {
            //        if (y != i && CmBxArtistName.Items[i] == CmBxArtistName.Items[y])
            //        {
            //            CmBxArtistName.Items.RemoveAt(i);
            //            break;
            //        }
            //    }
            //}

            //var listWithoutDuplicates = CmBxArtistName.Distinct().ToList();
        }

        private void CmBxArtistName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //CmBxArtistName.DisplayMemberPath = "Artist";
            //CmBxArtistName.SelectedValuePath = "ArtistId";
        }

        private void BackMainWindow_Click(object sender, RoutedEventArgs e)
        {
            var main = new MainWindow();
            this.Close();
            main.Show();
        }
    }
}
