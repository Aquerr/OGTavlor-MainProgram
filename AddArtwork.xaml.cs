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
            Artworks.Invnetory.Add(new Artwork() { Title = ArtName.Text, Artist = CmBxArtistName.SelectedValue.ToString(), ImagePath = ImagePath.ToString() });
            
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
            CmBxArtistName.ItemsSource = Artworks.Invnetory;
            CmBxArtistName.DisplayMemberPath = "Artist";
        }

        private void CmBxArtistName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            //CmBxArtistName.DisplayMemberPath = "Artist";
            //CmBxArtistName.SelectedValuePath = "ArtistId";
        }
    }
}
