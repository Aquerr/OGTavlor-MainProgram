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
        private int _interval = 2;

        public PictureSlideShow()
        {
            InitializeComponent();
            IArtworkService service = new ArtworkService();
            IArtworkLogic logic = new ArtworkLogic(service);
            _artworkLogic = logic;
            StopSlide.IsEnabled = false;
            Interval2.IsChecked = true;
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
            if (Interval1.IsChecked) _interval = 1;
            else if (Interval2.IsChecked) _interval = 2;
            else if (Interval3.IsChecked) _interval = 3;
            Timer.Tick += NextPictureByTimer;
            Timer.Interval = new TimeSpan(0, 0, _interval);
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

        private void EditArtwork(object sender, RoutedEventArgs e)
        {
            var editArtwork = new EditArtwork(_artworkLogic.GetArtworksAsync().Result[_id].Title);
            editArtwork.Show();
            this.Close();
        }

        private void LäggTillKonstverk_Click(object sender, RoutedEventArgs e)
        {
            var addArt = new AddArtwork();
            this.Close();
            addArt.Show();
        }

        //TODO: Refactor this code. It is not good to have three methods like this.

        private void Interval3_OnClick(object sender, RoutedEventArgs e)
        {
            if (Interval3.IsChecked)
            {
                Interval1.IsChecked = false;
                Interval2.IsChecked = false;
            }
        }

        private void Interval2_OnClick(object sender, RoutedEventArgs e)
        {
            if (Interval2.IsChecked)
            {
                Interval1.IsChecked = false;
                Interval3.IsChecked = false;
            }
        }

        private void Interval1_OnClick(object sender, RoutedEventArgs e)
        {
            if (Interval1.IsChecked)
            {
                Interval2.IsChecked = false;
                Interval3.IsChecked = false;
            }
        }
    }
}
