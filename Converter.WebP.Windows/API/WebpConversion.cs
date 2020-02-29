using System;
using System.IO;
using System.Threading;
using Squirrel_Sizer;

namespace Converter.WebP.Windows.API {
    internal class WebpConversion {
        private ThreadStart ConvertThreadStart { get; set; }
        private Thread ConvertThread { get; set; }
        internal DroppedImage DroppedImage { get; set; }
        internal Guid ImageGuid { get; set; }
        private string Image { get; set; }

        internal void StartConvert() {
            Image = $"{DroppedImage.Location}\\{DroppedImage.Name}{DroppedImage.Type}";
            ConvertThreadStart = Convert;
            ConvertThreadStart += () => {
                Reference.MainDispatcher.Invoke(() => {
                    foreach (DroppedImage image in Reference.ListView.ItemsSource) {
                        if (image.ImageGuid != ImageGuid) continue;
                        FileInfo info = new FileInfo($"{image.Location}\\{image.Name}.webp");
                        image.ConvertedSize = Sizer.SizeSuffix(info.Length);
                        Reference.ListView.Items.Refresh();
                    }
                }, System.Windows.Threading.DispatcherPriority.Background);
            };
            ConvertThread = new Thread(ConvertThreadStart);
            ConvertThread.Start();
        }

        internal void Convert() {
            Reference.StartProcess();
            Reference.Process.StandardInput.WriteLine(DroppedImage.Type.ToLower() != ".gif"
                ? $"cwebp \"{Image}\" -o \"{DroppedImage.Location}\\{DroppedImage.Name}.webp\""
                : $"gif2webp \"{Image}\" -o \"{DroppedImage.Location}\\{DroppedImage.Name}.webp\"");
            Reference.ExitProcess();
        }
    }
}
