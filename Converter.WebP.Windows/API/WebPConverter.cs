namespace Converter.WebP.Windows.API {
    public class WebPConverter {

        public static void Convert() {
            foreach (DroppedImage droppedImage in Reference.ImageCollection) {
                WebpConversion conversion = new WebpConversion {
                    DroppedImage = droppedImage,
                    ImageGuid = droppedImage.ImageGuid
                };
                conversion.StartConvert();
            }
        }

    }
}
