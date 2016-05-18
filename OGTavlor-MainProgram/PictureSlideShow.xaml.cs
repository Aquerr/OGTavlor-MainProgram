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

        //Redirects user to Main Window.
        private void btnMainWindow_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }
        
        //Loads the image when the window is loaded.
        private void PictureSlideShow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadImage();
        }

        //Gets the image.
        public void LoadImage()
        {
            _id = 0;
            _listCount = _artworkLogic.GetArtworksAsync().Result.Count;
            GetPicture();
        }

        //Function to load the next image.
        private void NextPicture(object sender, RoutedEventArgs e)
        {
            if (_id != _listCount - 1)
            {
                _id += 1;
                GetPicture();
            }
            else
            {
                _id = 0;
                GetPicture();
            }
        }

        //Function to load the previous image.
        private void PreviousPicture(object sender, RoutedEventArgs e)
        {
            if (_id != 0)
            {
                _id -= 1;
                GetPicture();
            }
            else
            {
                _id = _listCount - 1;
                GetPicture();
            }
        }

        //Function to get the image.
        private void GetPicture()
        {
            if (_artworkLogic.GetArtworksAsync().Result[_id].ImagePath != null)
            {
                var uripath = new Uri(_artworkLogic.GetArtworksAsync().Result[_id].ImagePath, UriKind.RelativeOrAbsolute);
                try
                {
                    ImgSlideShow.Source = new BitmapImage(uripath);

                }
                catch (Exception)
                {
                    MessageBox.Show("Bilderna kan inte laddas.", "Felmeddelande");
                    var main = new MainWindow();
                    this.Close();
                    main.Show();
                }
            }
        }

        //function to start the automatic slideshow.
        private void TimerStart()
        {
            if (Interval1.IsChecked) _interval = 1;
            else if (Interval2.IsChecked) _interval = 2;
            else if (Interval3.IsChecked) _interval = 3;
            Timer.Tick += NextPictureByTimer;
            Timer.Interval = new TimeSpan(0, 0, _interval);
            Timer.Start();
        }

        //Function for the next picture in the automatic slideshow by a timer.
        private void NextPictureByTimer(object sender, EventArgs e)
        {
            if (_id != _listCount - 1)
            {
                _id += 1;
                GetPicture();
            }
            else
            {
                _id = 0;
                GetPicture();
            }
        }

        //Stops the automatic slideshow.
        private void StopSlideShow(object sender, RoutedEventArgs e)
        {
            Timer.Stop();
            StartSlide.IsEnabled = true;
            StopSlide.IsEnabled = false;
        }

        //Starts the automatic slideshow.
        private void StartSlideShow(object sender, RoutedEventArgs e)
        {
            TimerStart();
            StartSlide.IsEnabled = false;
            StopSlide.IsEnabled = true;
        }

        //Button to redirect the user to edit the current artwork.
        private void EditArtwork(object sender, RoutedEventArgs e)
        {
            var editArtwork = new EditArtwork(_artworkLogic.GetArtworksAsync().Result[_id].Title);
            editArtwork.Show();
            this.Close();
        }

        //Button to redirect the user to Add Artwork.
        private void LäggTillKonstverk_Click(object sender, RoutedEventArgs e)
        {
            var addArt = new AddArtwork();
            this.Close();
            addArt.Show();
        }

        //Function for the intervals that can be selected.
        private void IntervalsCombined()
        {
            if (Interval3.IsPressed)
            {
                if (Interval3.IsChecked)
                {
                    Interval1.IsChecked = false;
                    Interval2.IsChecked = false;
                }
            }
            if (Interval2.IsPressed)
            {
                if (Interval2.IsChecked)
                {
                    Interval1.IsChecked = false;
                    Interval3.IsChecked = false;
                }
            }
            if (Interval1.IsPressed)
            {
                if (Interval1.IsChecked)
                {
                    Interval2.IsChecked = false;
                    Interval3.IsChecked = false;
                }
            }

        }

        //Interval setting 3.
        private void Interval3_OnClick(object sender, RoutedEventArgs e)
        {
            IntervalsCombined();
        }

        //Interval setting 2.
        private void Interval2_OnClick(object sender, RoutedEventArgs e)
        {
            IntervalsCombined();
        }

        //Interval setting 1.
        private void Interval1_OnClick(object sender, RoutedEventArgs e)
        {
            IntervalsCombined();
        }
    }
}
