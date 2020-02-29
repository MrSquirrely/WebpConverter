using System;

namespace Converter.WebP.Windows.API {
    public class DroppedImage {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public string Size { get; set; }
        public string ConvertedSize { get; set; }
        public Guid ImageGuid { get; set; }
    }
}
