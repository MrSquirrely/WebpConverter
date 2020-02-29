namespace Converter.WebP.Windows.API {
    public class SettingsFile {
        public bool Lossless { get; set; }
        public bool Noalpha { get; set; }
        public string Metadata { get; set; }
        public float WebpCompression { get; set; }
        public float GifCompression { get; set; }

    }
}
