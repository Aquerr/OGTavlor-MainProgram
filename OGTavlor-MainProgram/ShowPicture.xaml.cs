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
        string _artworkName;
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

        private async void LoadArtwork()
        {
            var art = await _artworkLogic.GetArtworkAsync(_artworkName);


            //TODO: Make here anticrashing system. Program shall not crash when it will not find imagepath for an image.
            var uripath = new Uri((art.ImagePath), UriKind.RelativeOrAbsolute);
            image.Source = new BitmapImage(uripath);

            TextInfo.Text = art.Description;
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var editArtwork = new EditArtwork(_artworkName);
            editArtwork.Show();
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
                _artworkLogic.DeleteArtworkAsync(_artworkName);

             //   MessageBox.Show("Du har nu tagit bort detta konstverk", "Statusmeddelande");

                var mainWindow = new MainWindow();
                this.Close();
                mainWindow.Show();
            }


        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();

        }
    }
}
