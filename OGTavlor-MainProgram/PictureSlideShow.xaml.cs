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
    /// Interaction logic for PictureSlideShow.xaml
    /// </summary>
    public partial class PictureSlideShow : Window
    {
        private readonly IArtworkLogic _artworkLogic;
        string _artworkName;

        public PictureSlideShow()
        {
            InitializeComponent();
            IArtworkService service = new ArtworkService();
            IArtworkLogic logic = new ArtworkLogic(service);
            _artworkLogic = logic;
        }

        private void btnMainWindow_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }

        private void PictureSlideShow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadImage();
        }

        public void LoadImage()
        {
            var uripath = new Uri((_artworkLogic.GetArtworksAsync().Result[0].ImagePath), UriKind.RelativeOrAbsolute);
            _artworkName = _artworkLogic.GetArtworksAsync().Result[0].Title;
            ImgSlideShow.Source = new BitmapImage(uripath);
        }

        private void NextPicure()
        {
            var uripath = new Uri((_artworkLogic.GetArtworksAsync().Result.FirstOrDefault().ImagePath), UriKind.RelativeOrAbsolute);
            ImgSlideShow.Source = new BitmapImage(uripath);
        }

        private void PreviousPicture()
        {
            var uripath = new Uri((_artworkLogic.GetArtworksAsync().Result.FirstOrDefault().ImagePath), UriKind.RelativeOrAbsolute);
            ImgSlideShow.Source = new BitmapImage(uripath);
        }
    }
}
