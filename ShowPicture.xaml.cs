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
using System.Windows.Shapes;

namespace OGTavlor_MainProgram
{
    /// <summary>
    /// Interaction logic for ShowPicture.xaml
    /// </summary>
    public partial class ShowPicture : Window
    {
        public ShowPicture()
        {
            InitializeComponent();
        }

        public int PassId { get; set; }
    }
}
