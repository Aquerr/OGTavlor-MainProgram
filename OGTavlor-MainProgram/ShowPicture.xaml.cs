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
        string PassId;
        public ShowPicture(string _id)
        {
            InitializeComponent();
            PassId = _id;
            LoadArtwork();
            //Artworks artworks = new Artworks();

            //BtnRemove.Click += new EventHandler((x, y) => Artworks.Invnetory.Remove(x => x = PassId));
        }

        private void LoadArtwork()
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "ogtavlor" table.
            CloudTable table = tableClient.GetTableReference("ogtavlor");

            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<CustomerEntity> query = new TableQuery<CustomerEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Brutus"));


            //TODO: Make here anticrashing system. Program shall not crash when it will not find imagepath for an image.
            var uripath = new Uri((table.ExecuteQuery(query).Where(x => x.RowKey == PassId).Select(y => y.ImagePath).FirstOrDefault()).ToString(), UriKind.RelativeOrAbsolute);
            image.Source = new BitmapImage(uripath);

            TextInfo.Text = (table.ExecuteQuery(query).Where(x => x.RowKey == PassId).Select(y => y.Description).FirstOrDefault());
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            EditArtwork ea = new EditArtwork(PassId);
            ea.Show();
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
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                CloudTable table = tableClient.GetTableReference("ogtavlor");

                TableQuery<CustomerEntity> query = new TableQuery<CustomerEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Brutus"));

                TableOperation retrieveOperation = TableOperation.Retrieve<CustomerEntity>("Brutus", "Pingviner");

                TableResult retrievedResult = table.Execute(retrieveOperation);

                // Artworks.Invnetory.Remove(Artworks.Invnetory.Where(x => x.ArtworkId == PassId).FirstOrDefault());
                MessageBox.Show("Du har nu tagit bort detta konstverk", "Statusmeddelande");
                MainWindow mainWindow = new MainWindow();
                this.Close();
                mainWindow.Show();
            }


        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();

        }
    }
}
