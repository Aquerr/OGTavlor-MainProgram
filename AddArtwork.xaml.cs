using Microsoft.Win32;
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
    /// Interaction logic for AddArtwork.xaml
    /// </summary>
    public partial class AddArtwork : Window
    {
        public AddArtwork()
        {
            InitializeComponent();
            LoadComboBox();

        }
        string ImagePath = "";
        

        private void SaveArtwork_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "people" table.
            CloudTable table = tableClient.GetTableReference("ogtavlor");

            // Create a new customer entity.
            CustomerEntity customer1 = new CustomerEntity(ArtArtist.Text, ArtName.Text);
            customer1.ImagePath = ImagePath.ToString();

            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(customer1);

            // Execute the insert operation.
            table.Execute(insertOperation);

         //   Artworks.Invnetory.Add(new Artwork() { Title = ArtName.Text, Artist = ArtArtist.Text, ImagePath = ImagePath.ToString() });
            
            MainWindow Main = new MainWindow();
            this.Close();
            Main.Show();
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

        private void btnAddArtist_Click(object sender, RoutedEventArgs e)
        {
            AddArtist MyArtist = new AddArtist();
            this.Close();
            MyArtist.Show();
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


    }
}
