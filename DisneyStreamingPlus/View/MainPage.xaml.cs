﻿using DisneyStreamingPlus.ViewModel;
using System;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace DisneyStreamingPlus
{
    /// <summary>
    /// The main page to navigate videos.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string ImageNotFoundErrorRelativePath = "Assets\\ImageNotFoundError.png";

        public MainPage()
        {
            ViewModel = new MainPageViewModel();
            InitializeComponent();
            RowsCVS.Source = ViewModel.ImagesGrouped;
        }

        public MainPageViewModel ViewModel { get; set; }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            var image = sender as Image;
            if (image != null)
            {
                image.Source = new BitmapImage(new Uri(Path.Combine(Environment.CurrentDirectory, ImageNotFoundErrorRelativePath)));
            }
        }
    }
}