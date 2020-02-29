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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using Converter.WebP.Windows.API;
using Squirrel_Sizer;
using Path = System.IO.Path;


namespace Converter.WebP.Windows {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {

        private readonly ObservableCollection<DroppedImage> _droppedImagesCollection = Reference.ImageCollection;
        public MainWindow() {
            InitializeComponent();
            Reference.ListView = ImageListView;
            ImageListView.ItemsSource = _droppedImagesCollection;
            Reference.MainDispatcher = Dispatcher.CurrentDispatcher;

            //Debug.WriteLine($"\n ########## \n Platform: {Reference.CurrentPlatformId} \n ########## \n");
            //switch (Reference.CurrentPlatformId) {
            //    case PlatformID.Win32S:
            //    case PlatformID.Win32Windows:
            //    case PlatformID.WinCE:
            //    case PlatformID.Xbox:
            //        break;
            //    case PlatformID.Unix:
            //        break;
            //    case PlatformID.MacOSX:
            //        break;
            //    case PlatformID.Win32NT:
            //        Debug.WriteLine($"\n ########## \n Is64: {Environment.Is64BitOperatingSystem} \n ########## \n Processors: {Environment.ProcessorCount} \n ########## \n");
            //        break;
            //    default:
            //        throw new ArgumentOutOfRangeException();
            //}
        }

        private void ImageListView_OnDrop(object sender, DragEventArgs e) {
            if (e.Data == null) return;
            _droppedImagesCollection.Clear();
            string[] dataStrings = (string[]) e.Data.GetData(DataFormats.FileDrop);
            foreach (string dataString in dataStrings) {
                FileInfo info = new FileInfo(dataString);
                if (!Reference.ImageTypes.Contains(info.Extension.ToLower())) continue;
                Debug.WriteLine($"Adding {info.Name}");
                _droppedImagesCollection.Add(new DroppedImage() {
                    Name = Path.GetFileNameWithoutExtension(dataString),
                    Type = info.Extension,
                    Size = Sizer.SizeSuffix(info.Length),
                    Location = info.DirectoryName,
                    ConvertedSize = Sizer.Zero,
                    ImageGuid = Guid.NewGuid()
                });
                //Todo: UpdateNumber();
            }
        }

        private void ConvertButton_OnClick(object sender, RoutedEventArgs e) {
            WebPConverter.Convert();
        }
    }
}
