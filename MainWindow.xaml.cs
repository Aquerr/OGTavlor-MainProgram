﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OGTavlor_MainProgram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FillList();
        }

        private void LäggTillKonstverk_Click(object sender, RoutedEventArgs e)
        {
            AddArtwork AddArt = new AddArtwork();
            this.Close();
            AddArt.Show();
        }

        private void FillList()
        {
            ArtworkListView.ItemsSource = Artworks.Invnetory;
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
            var id = ((Artwork)item).ArtworkId;
            MessageBox.Show(id.ToString());
            ShowPicture Sp = new ShowPicture(id);
         //   Sp.Id = id;
            this.Close();
            Sp.Show();
        }
    }
}
