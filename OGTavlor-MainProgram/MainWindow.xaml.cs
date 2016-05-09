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
        private IArtworkLogic _artworkLogic;
        private string _searchText = string.Empty;

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
            AddArtwork AddArt = new AddArtwork();
            this.Close();
            AddArt.Show();
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
            PictureSlideShow SlideShow = new PictureSlideShow();
            this.Close();
            SlideShow.Show();
        }

        private void ButtonArtwork_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext;
            var id = ((Artwork)item).RowKey;
            ShowPicture Sp = new ShowPicture(id);
            this.Close();
            Sp.Show();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            
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
    }
}
