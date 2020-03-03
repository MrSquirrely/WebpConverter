#region LICENSE
	/*
     * This Class1.cs is part of Converter.WebP.API.
     *
     * Converter.WebP.API is free software: you can redistribute it and/or modify
     * it under the terms of the GNU General Public License as published by
     * the Free Software Foundation, either version 3 of the License, or
     * (at your option) any later version.
     *
     * Converter.WebP.API is distributed in the hope that it will be useful,
     * but WITHOUT ANY WARRANTY; without even the implied warranty of
     * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
     * GNU General Public License for more details.
     *
     * You should have received a copy of the GNU General Public License
     * along with Converter.WebP.API.  If not, see <https://www.gnu.org/licenses/>.
     */
#endregion
using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using Converter.WebP.Windows.API;
using Microsoft.Win32;
using Squirrel_Sizer;
using Path = System.IO.Path;


namespace Converter.WebP.Windows {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {

        //Todo: create a logger
        //Todo: Globalization

        private readonly AppDomain _domain = AppDomain.CurrentDomain;

        public MainWindow() {
            InitializeComponent();
            _domain.UnhandledException += Reference.ExceptionHandler;
            Reference.ListView = ImageListView;
            ImageListView.ItemsSource = Reference.ImageCollection;
            Reference.MainDispatcher = Dispatcher.CurrentDispatcher;
            Reference.TotalSizeLabel = TotalSizeLabel;
            Reference.ConvertedSizeLabel = ConvertedSizeLabel;
            Settings.LoadSettings();
        }

        private void ImageListView_OnDrop(object sender, DragEventArgs e) {
            if (e.Data == null) return;
            Reference.ImageCollection.Clear();
            string[] dataStrings = (string[]) e.Data.GetData(DataFormats.FileDrop);
            if (dataStrings == null) return;
            foreach (string dataString in dataStrings) {
                FileInfo info = new FileInfo(dataString);
                if (!Reference.ImageTypes.Contains(info.Extension.ToLower())) continue;
                Reference.ImageCollection.Add(new DroppedImage() {
                    Name = Path.GetFileNameWithoutExtension(dataString),
                    Type = info.Extension,
                    Size = Sizer.SizeSuffix(info.Length),
                    Location = info.DirectoryName,
                    ConvertedSize = Sizer.Zero,
                    ImageGuid = Guid.NewGuid()
                });
                Reference.TotalSize += info.Length;
                Reference.TotalSizeLabel.Content = $"Original Size: {Sizer.SizeSuffix(Reference.TotalSize)}";
            }
        }

        private void ConvertButton_OnClick(object sender, RoutedEventArgs e) => WebPConverter.Convert();

        private void ToggleDialog(object sender, RoutedEventArgs e) {
            if (!SettingsDialog.IsOpen) {
                DialogContentHolder.Children.Clear();
                DialogContentHolder.Children.Add(new Views.SettingsView());
                SettingsDialog.IsOpen = true;
            }
            else {
                SettingsDialog.IsOpen = false;
            }
        }
        
        private void ClearSelectedImage(object sender, RoutedEventArgs e) {
            DroppedImage selectedImage = (DroppedImage)ImageListView.SelectedItem;
            DroppedImage removeDroppedImage = null;
            foreach (DroppedImage droppedImage in Reference.ImageCollection) {
                if (selectedImage.ImageGuid == droppedImage.ImageGuid) {
                    removeDroppedImage = droppedImage;
                }
            }

            if (removeDroppedImage == null) return;
            Reference.ImageCollection.Remove(removeDroppedImage);
            ImageListView.Items.Refresh();
        }

        private void ClearAllImages(object sender, RoutedEventArgs e) {
            Reference.ImageCollection.Clear();
            ImageListView.Items.Refresh();
        }

        private void MenuFileOpen_OnClick(object sender, RoutedEventArgs e) {
            OpenFileDialog openFile = new OpenFileDialog() {
                Multiselect = true,
                Filter = "Images |*.png;*.jpeg;*.jpg;*.exif;*.tiff;*.bmp;*.gif",
                InitialDirectory = Environment.CurrentDirectory
            };
            bool? result = openFile.ShowDialog();
            if (result != true) return;
            foreach (string openFileFileName in openFile.FileNames) {
                FileInfo info = new FileInfo(openFileFileName);
                if (!Reference.ImageTypes.Contains(info.Extension.ToLower())) continue;
                Reference.ImageCollection.Add(new DroppedImage() {
                    Name = Path.GetFileNameWithoutExtension(openFileFileName),
                    Type = info.Extension,
                    Size = Sizer.SizeSuffix(info.Length),
                    Location = info.DirectoryName,
                    ConvertedSize = Sizer.Zero,
                    ImageGuid = Guid.NewGuid()
                });
                Reference.TotalSize += info.Length;
                Reference.TotalSizeLabel.Content = $"Original Size: {Sizer.SizeSuffix(Reference.TotalSize)}";
            }
        }

        private void MenuFileExit_OnClick(object sender, RoutedEventArgs e) => Environment.Exit(0);
    }
}
