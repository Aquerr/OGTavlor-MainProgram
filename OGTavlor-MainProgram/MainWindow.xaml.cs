using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<Artwork> _allItems;
        private readonly IArtworkLogic _artworkLogic;
        private string _searchText = string.Empty;
        public int Ful = 0;

        public MainWindow()
        {
            InitializeComponent();
            IArtworkService service = new ArtworkService();
            IArtworkLogic logic = new ArtworkLogic(service);
            _artworkLogic = logic;
            FillList();
        }

        //Button to redirect user to Add Artwork Window.
        private void LäggTillKonstverk_Click(object sender, RoutedEventArgs e)
        {
            var addArt = new AddArtwork();
            this.Close();
            addArt.Show();
        }

        //Fills the Listview in Main Window with all the artworks.
        private async void FillList()
        {
            try
            {
                var list = await GetItemsAsync();
                AllItems = new ObservableCollection<Artwork>(list);
            }
            catch (Exception)
            {

            }
            finally
            {
                ArtworkListView.ItemsSource = AllItems;
            }
        }


        //List of all the artworks.
        private async Task<List<Artwork>> GetItemsAsync()
        {
            var list = await _artworkLogic.GetArtworksAsync();
            return list;
        }

        //Button to redirect user to Slideshow Window.
        private void BtnSlideShow_Click(object sender, RoutedEventArgs e)
        {
            var pictureSlideShow = new PictureSlideShow();
            this.Close();
            pictureSlideShow.Show();
        }

        //Function to choose an item in the listview thru an double-click.
        private void ButtonArtwork_Click(object sender, RoutedEventArgs e)
        {

            Ful++;
            if (Ful == 2)
            {
                var item = (sender as FrameworkElement).DataContext;
                var id = ((Artwork)item).RowKey;
                var showPicture = new ShowPicture(id);
                this.Close();
                showPicture.Show();
                Ful = 0;
            }

        }

        //Function for searching artworks inside the listview in Main Window.
        private void SearchArts(object sender, RoutedEventArgs e)
        {
            var lookFor = TxtbxSearchBox.Text.ToLower();

            var arts = _artworkLogic.GetArtworksAsync().Result;

            IEnumerable<Artwork> filteredArtworks;

            if (SignedCheck.IsChecked.Value)
            {
                filteredArtworks = arts.Where(x => x.Signed.Equals(true));
                filteredArtworks = filteredArtworks.Where(str => str.RowKey.ToLower().Contains(lookFor) || str.PartitionKey.ToLower().Contains(lookFor) || str.Room.ToLower().Contains(lookFor));
            }
            else
            {
                filteredArtworks = arts.Where(x => x.Signed.Equals(false));
                filteredArtworks = filteredArtworks.Where(str => str.RowKey.ToLower().Contains(lookFor) || str.PartitionKey.ToLower().Contains(lookFor) || str.Room.ToLower().Contains(lookFor));
            }

            AllItems = new ObservableCollection<Artwork>(filteredArtworks);

            ArtworkListView.ItemsSource = AllItems;

            if (lookFor == "")
            {
                FillList();
            }
        }

        //Sets focus on the Searchbox/Textbox inside Main Window.
        private void ArtworkList_OnLoaded(object sender, RoutedEventArgs e)
        {
            TxtbxSearchBox.Focus();
        }

        public ObservableCollection<Artwork> AllItems
        {
            get { return _allItems; }
            set
            {
                if (_allItems != value)
                {
                    _allItems = value;
                }
            }
        }

        //Sets the focus on the item that the mouse is hoovering over.
        private void ArtworkListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var items = ArtworkListView.Items;
        }

        private void SignedCheck_OnChecked(object sender, RoutedEventArgs e)
        {
            var arts = _artworkLogic.GetArtworksAsync().Result;

            IEnumerable<Artwork> filteredArtworks;

            if (SignedCheck.IsChecked.Value)
            {
                filteredArtworks = arts.Where(x => x.Signed.Equals(true));
            }
            else
            {
                filteredArtworks = arts;
            }

            AllItems = new ObservableCollection<Artwork>(filteredArtworks);

            ArtworkListView.ItemsSource = AllItems;
        }
    }
}
