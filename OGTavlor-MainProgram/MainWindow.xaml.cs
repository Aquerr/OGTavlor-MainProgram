using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OGTavlor_MainProgram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Artwork> _allItems;
        private readonly IArtworkLogic _artworkLogic;

        public MainWindow()
        {
            InitializeComponent();
            IArtworkService service = new ArtworkService();
            IArtworkLogic logic = new ArtworkLogic(service);
            _artworkLogic = logic;
            listBoxPlace.SelectedValue = "Alla Områden";
            FillList();
            LoadPlaceList();
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
                var lookFor = TxtbxSearchBox.Text.ToLower();

                var arts = await GetItemsAsync();
                
                IEnumerable<Artwork> filteredArtworks;

                string places = listBoxPlace.SelectedValue.ToString();
                if (lookFor == "")
                {
                    if (places != "Alla Områden")
                    {
                        if (SignedCheck.IsChecked.Equals(true))
                        {
                            filteredArtworks = arts.Where(x => x.Signed.Equals(true));
                            filteredArtworks = filteredArtworks.Where(x => x.Place == places);
                        }
                        else
                        {
                            filteredArtworks = arts;
                            filteredArtworks = filteredArtworks.Where(x => x.Place == places);
                        }
                    }
                    else
                    {
                        if (SignedCheck.IsChecked.Equals(true))
                        {
                            filteredArtworks = arts.Where(x => x.Signed.Equals(true));             
                        }
                        else
                        {
                            filteredArtworks = arts;
                        }      
                    }
                    AllItems = new ObservableCollection<Artwork>(filteredArtworks);
                }
                else
                {
                    if (places != "Alla Områden")
                    {
                        if (SignedCheck.IsChecked.Equals(true))
                        {
                            filteredArtworks = arts.Where(x => x.Signed.Equals(true));
                            filteredArtworks = filteredArtworks.Where(x => x.Place == places);
                            filteredArtworks = filteredArtworks.Where(str => str.RowKey.ToLower().Contains(lookFor) || str.PartitionKey.ToLower().Contains(lookFor) || str.Room.ToLower().Contains(lookFor) || str.Height.ToString().Contains(lookFor) || str.Width.ToString().Contains(lookFor));
                        }
                        else
                        {
                            filteredArtworks = arts.Where(x => x.Place == places);
                            filteredArtworks = filteredArtworks.Where(str => str.RowKey.ToLower().Contains(lookFor) || str.PartitionKey.ToLower().Contains(lookFor) || str.Room.ToLower().Contains(lookFor) || str.Height.ToString().Contains(lookFor) || str.Width.ToString().Contains(lookFor));
                        }
                    }
                    else
                    {
                        if (SignedCheck.IsChecked.Equals(true))
                        {
                            filteredArtworks = arts.Where(x => x.Signed.Equals(true));
                            filteredArtworks = filteredArtworks.Where(str => str.RowKey.ToLower().Contains(lookFor) || str.PartitionKey.ToLower().Contains(lookFor) || str.Room.ToLower().Contains(lookFor));
                        }
                        else
                        {
                            filteredArtworks = arts.Where(x => x.Place == places);
                            filteredArtworks = arts.Where(str => str.RowKey.ToLower().Contains(lookFor) || str.PartitionKey.ToLower().Contains(lookFor) || str.Room.ToLower().Contains(lookFor));
                        }
                        
                    }
                    
                    AllItems = new ObservableCollection<Artwork>(filteredArtworks);

                }
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
            var item = (sender as FrameworkElement).DataContext;
            var id = ((Artwork)item).RowKey;
            var showPicture = new ShowPicture(id);
            this.Close();
            showPicture.Show();
        }

        //Function for searching artworks inside the listview in Main Window.
        private void SearchArts(object sender, RoutedEventArgs e)
        {
            FillList();
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

        private void SignedCheck_OnChecked(object sender, RoutedEventArgs e)
        {
            FillList();
        }

        private async void LoadPlaceList()
        {
            var arts = await _artworkLogic.GetArtworksAsync();

            listBoxPlace.Items.Add("Alla Områden");

            foreach (var item in arts)
            {
                var entity = item.Place;
                if (!listBoxPlace.Items.Contains(entity))
                {
                    listBoxPlace.Items.Add(entity);
                }
            }
        }

        private void listBoxPlace_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           FillList();
        }
    }
}
