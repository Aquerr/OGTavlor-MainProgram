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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System.Configuration;

namespace OGTavlor_MainProgram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FillList();
        }

        private void LäggTillKonstverk_Click(object sender, RoutedEventArgs e)
        {
            AddArtwork AddArt = new AddArtwork();
            this.Close();
            AddArt.Show();
        }

        private void FillList()
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "ogtavlor" table.
            CloudTable table = tableClient.GetTableReference("ogtavlor");

            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<CustomerEntity> query = new TableQuery<CustomerEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Brutus"));

            // Print the fields for each customer.

          //  ArtworkListView.ItemsSource = table.ExecuteQuery(query);

            //    TableOperation selectOperation = TableOperation.Retrieve;

            //    ArtworkListView.ItemsSource = Artworks.Invnetory;

        }

        private void BtnSlideShow_Click(object sender, RoutedEventArgs e)
        {
            PictureSlideShow SlideShow = new PictureSlideShow();
            this.Close();
            SlideShow.Show();
        }

        private void ButtonArtwork_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext;
            var id = ((CustomerEntity)item).RowKey;
            ShowPicture Sp = new ShowPicture(id);
            this.Close();
            Sp.Show();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ArtworkList_OnLoaded(object sender, RoutedEventArgs e)
        {
            txtbxSearchBox.Focus();
        }
    }
}
