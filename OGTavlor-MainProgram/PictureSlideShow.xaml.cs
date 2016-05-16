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

        private System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();
        private readonly IArtworkLogic _artworkLogic;
        private int _id;
        private int _listCount;

        public PictureSlideShow()
        {
            InitializeComponent();
            IArtworkService service = new ArtworkService();
            IArtworkLogic logic = new ArtworkLogic(service);
            _artworkLogic = logic;
            StopSlide.IsEnabled = false;
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
            _id = 0;
            _listCount = _artworkLogic.GetArtworksAsync().Result.Count;
            GetPicutre();
        }

        private void NextPicture(object sender, RoutedEventArgs e)
        {
            if (_id != _listCount - 1)
            {
                _id += 1;
                GetPicutre();
            }
            else
            {
                _id = 0;
                GetPicutre();
            }
        }

        private void PreviousPicture(object sender, RoutedEventArgs e)
        {
            if (_id != 0)
            {
                _id -= 1;
                GetPicutre();
            }
            else
            {
                _id = _listCount - 1;
                GetPicutre();
            }
        }

        private void GetPicutre()
        {
            if (_artworkLogic.GetArtworksAsync().Result[_id].ImagePath != null)
            {
                var uripath = new Uri((_artworkLogic.GetArtworksAsync().Result[_id].ImagePath), UriKind.RelativeOrAbsolute);
                ImgSlideShow.Source = new BitmapImage(uripath);
            }
        }

        private void TimerStart()
        {
            Timer.Tick += NextPictureByTimer;
            Timer.Interval = new TimeSpan(0, 0, 2);
            Timer.Start();
        }

        private void NextPictureByTimer(object sender, EventArgs e)
        {
            if (_id != _listCount - 1)
            {
                _id += 1;
                GetPicutre();
            }
            else
            {
                _id = 0;
                GetPicutre();
            }
        }

        private void StopSlideShow(object sender, RoutedEventArgs e)
        {
            Timer.Stop();
            StartSlide.IsEnabled = true;
            StopSlide.IsEnabled = false;
        }

        private void StartSlideShow(object sender, RoutedEventArgs e)
        {
            TimerStart();
            StartSlide.IsEnabled = false;
            StopSlide.IsEnabled = true;
        }
    }
}
