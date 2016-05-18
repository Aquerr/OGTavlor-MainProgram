using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System.Configuration;
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
        private readonly string _artworkName;
        private readonly IArtworkLogic _artworkLogic;

        public ShowPicture(string artworkName)
        {
            InitializeComponent();
            _artworkName = artworkName;
            IArtworkService service = new ArtworkService();
            IArtworkLogic logic = new ArtworkLogic(service);
            _artworkLogic = logic;

            LoadArtwork();
        }
        
        //Loads the information into the Show Picture window.
        private async void LoadArtwork()
        {
            var art = await _artworkLogic.GetArtworkAsync(_artworkName);

            if (art.ImagePath != null)
            {
                var uripath = new Uri(art.ImagePath, UriKind.RelativeOrAbsolute);
                try
                {
                    Image.Source = new BitmapImage(uripath);
                }
                catch (Exception)
                {

                    MessageBox.Show("Bilden kunde inte hittas.");
                }                
            }

            TextTitle.Text = "Titel: " + art.Title;
            TextArtist.Text = "Konstnär: " + art.Artist;
            TextInfo.Text = "Beskrivning: "+ art.Description;
            TextRoom.Text = "Rum: " + art.Room;
            TextPlace.Text = "Plats: " + art.Place;
            TextSize.Text = "Bredd x Höjd: " + art.Width + " x " + art.Height + " cm";
        }

        //Button to redirect user to Edit Window.
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var editArtwork = new EditArtwork(_artworkName);
            editArtwork.Show();
            this.Close();
        }
        
        //Button to remove an artwork.
        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Vill du ta bort detta konstverk?", "Ta bort konstverk", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {

            }
            else
            {
                _artworkLogic.DeleteArtworkAsync(_artworkName);
                //   MessageBox.Show("Du har nu tagit bort detta konstverk", "Statusmeddelande");

                var mainWindow = new MainWindow();
                this.Close();
                mainWindow.Show();
            }
        }

        //Button to redirect user to Main Window.
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();

        }
    }
}
