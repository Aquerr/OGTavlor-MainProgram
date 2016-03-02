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
        string PassId;
        string ImagePath = "";

        public EditArtwork(string _id)
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
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "ogtavlor" table.
            CloudTable table = tableClient.GetTableReference("ogtavlor");

            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<CustomerEntity>("Smith", "Ben");

            // Execute the operation.
            TableResult retrievedResult = table.Execute(retrieveOperation);

            // Assign the result to a CustomerEntity object.
            CustomerEntity updateEntity = (CustomerEntity)retrievedResult.Result;

            if (updateEntity != null)
            {
                // Change the phone number.
                updateEntity.RowKey = ArtName.Text;
                updateEntity.ImagePath = ImagePath;
                updateEntity.PartitionKey = ArtArtist.Text;

                // Create the InsertOrReplace TableOperation.
                TableOperation updateOperation = TableOperation.Replace(updateEntity);

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

            TableQuery<CustomerEntity> query = new TableQuery<CustomerEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Brutus"));

            ArtName.Text = (table.ExecuteQuery(query).Where(x => x.RowKey == PassId).Select(y => y.RowKey).FirstOrDefault());
            ArtArtist.Text = (table.ExecuteQuery(query).Where(x => x.RowKey == PassId).Select(y => y.PartitionKey).FirstOrDefault());

            var uripath = new Uri((table.ExecuteQuery(query).Where(x => x.RowKey == PassId).Select(y => y.ImagePath).FirstOrDefault()).ToString(), UriKind.RelativeOrAbsolute);
            ArtImage.Source = new BitmapImage(uripath);
            ImagePath = uripath.ToString();
        }
    }
}
