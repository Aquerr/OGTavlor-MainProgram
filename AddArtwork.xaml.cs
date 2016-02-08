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
        }

        List<Artwork> Inventory = new List<Artwork>();
        
        private void SaveArtwork_Click(object sender, RoutedEventArgs e)
        {           
            Inventory.Add(new Artwork() { Title = ArtName.Text, Artist = ArtistName.Text });

            MainWindow Main = new MainWindow();
            this.Close();
            Main.Show();
        }

        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            Artwork art = new Artwork();

            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                ArtImage.Source = new BitmapImage(new Uri(op.FileName));
                art.ImagePath = (ArtImage.Source as BitmapImage).UriSource.AbsolutePath;
                //  MessageBox.Show(ImagePath);
            }
        }
    }
}
