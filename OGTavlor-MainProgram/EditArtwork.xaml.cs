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
        string ImagePath = "";

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
                ImagePath = (ArtImage.Source as BitmapImage).UriSource.AbsolutePath;
            }
        }

        private void SaveArtwork_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "ogtavlor" table.
            CloudTable table = tableClient.GetTableReference("ogtavlor");

            var art = _artworkLogic.GetArtworkAsync(_artName).Result;

            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<Artwork>(art.Artist, art.Title);

            // Execute the operation.
            TableResult retrievedResult = table.Execute(retrieveOperation);

            // Assign the result to a CustomerEntity object.
            Artwork artwork = (Artwork)retrievedResult.Result;

            if (artwork != null)
            {
                //TODO: If RowKey or PartitionKey is changed, delete an entity and insert a new one with new properties.

                // Update Entity
                artwork.RowKey = ArtName.Text;
                artwork.ImagePath = "";
                //updateEntity.ImagePath = ImagePath;
                artwork.PartitionKey = ArtArtist.Text;

                // Create the Replace TableOperation.
                TableOperation updateOperation = TableOperation.Replace(artwork);

                // Execute the operation.
                table.Execute(updateOperation);

                MessageBox.Show("Entity updated.");
            }

            else
                MessageBox.Show("Entity could not be retrieved.");

            //  Artworks.Invnetory[PassId-1].Title = ArtName.Text;
            //  Artworks.Invnetory[PassId-1].Artist = ArtArtist.Text;
            //  Artworks.Invnetory[PassId-1].ImagePath = ImagePath;

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
            ImagePath = uripath.ToString();
        }
    }
}
