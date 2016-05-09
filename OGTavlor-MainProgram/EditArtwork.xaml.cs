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
        string _artName;
        string _imagePath = "";

        public EditArtwork(string artName)
        {
            InitializeComponent();
            IArtworkService service = new ArtworkService();
            IArtworkLogic logic = new ArtworkLogic(service);
            _artworkLogic = logic;

            _artName = artName;
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
            _artworkLogic.ReplaceArtwork(ArtArtist.Text, ArtName.Text, _imagePath, _artName);

            MainWindow Main = new MainWindow();
            this.Close();
            Main.Show();
        }

        private void FillInfo()
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "ogtavlor" table.
            CloudTable table = tableClient.GetTableReference("ogtavlor");

            TableQuery<Artwork> query = new TableQuery<Artwork>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Brutus"));

            ArtName.Text = (table.ExecuteQuery(query).Where(x => x.RowKey == _artName).Select(y => y.RowKey).FirstOrDefault());
            ArtArtist.Text = (table.ExecuteQuery(query).Where(x => x.RowKey == _artName).Select(y => y.PartitionKey).FirstOrDefault());

            //TODO: Make here anticrashing system. Program shall not crash when it will not find imagepath for an image.
            var uripath = new Uri((table.ExecuteQuery(query).Where(x => x.RowKey == _artName).Select(y => y.ImagePath).FirstOrDefault()), UriKind.RelativeOrAbsolute);
            ArtImage.Source = new BitmapImage(uripath);
            _imagePath = uripath.ToString();
        }
    }
}
