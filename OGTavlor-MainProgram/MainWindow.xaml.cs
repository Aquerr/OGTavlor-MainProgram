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

        private void LäggTillKonstverk_Click(object sender, RoutedEventArgs e)
        {
            var addArt = new AddArtwork();
            this.Close();
            addArt.Show();
        }

        private async void FillList()
        {
            try
            {
                var list = await GetItemsAsync();
                AllItems = new ObservableCollection<Artwork>(list);
            }
            catch(Exception exception)
            {

            }
            finally
            {
                ArtworkListView.ItemsSource = AllItems;
            }
        }



        private async Task<List<Artwork>> GetItemsAsync()
        {
            var list = await _artworkLogic.GetArtworksAsync();
            return list;
        }

        private void BtnSlideShow_Click(object sender, RoutedEventArgs e)
        {
            var pictureSlideShow = new PictureSlideShow();
            this.Close();
            pictureSlideShow.Show();
        }
        
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

        private void SearchArts(object sender, RoutedEventArgs e)
        {
            var lookFor = TxtbxSearchBox.Text.ToLower();

            var arts = _artworkLogic.GetArtworksAsync().Result;

            var filteredArtworks = arts.Where(str => str.RowKey.ToLower().Contains(lookFor) || str.PartitionKey.ToLower().Contains(lookFor));

            AllItems = new ObservableCollection<Artwork>(filteredArtworks);

            ArtworkListView.ItemsSource = AllItems;

            if (lookFor == "")
            {
                FillList();
            }
        }

        private void ArtworkList_OnLoaded(object sender, RoutedEventArgs e)
        {
            TxtbxSearchBox.Focus();
        }

        public ObservableCollection<Artwork> AllItems
        {
            get { return _allItems; }
            set
            {
                if(_allItems != value)
                {
                    _allItems = value;
                }
            }
        }

        private void ArtworkListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var items = ArtworkListView.Items;
        }
    }
}
