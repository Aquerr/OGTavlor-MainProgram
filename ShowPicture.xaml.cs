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
    /// Interaction logic for ShowPicture.xaml
    /// </summary>
    public partial class ShowPicture : Window
    {
        int PassId;
        public ShowPicture(int _id)
        {
            InitializeComponent();
            PassId = _id;
            LoadImage();
        }

        private void LoadImage()
        {
            MessageBox.Show(PassId.ToString());
            var uripath = new Uri((Artworks.Invnetory.Where(x => x.ArtworkId == PassId).Select(y => y.ImagePath).FirstOrDefault()).ToString(),UriKind.RelativeOrAbsolute);
            image.Source = new BitmapImage(uripath);
        }
    }
}
