using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Converter.WebP.Windows.API;

namespace Converter.WebP.Windows.Views {
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl {
        public SettingsView() {
            InitializeComponent();
            LosslessCheck.IsChecked = Settings.SettingsFile.Lossless;
            NoAlphaCheck.IsChecked = Settings.SettingsFile.Noalpha;
            DeleteFileCheck.IsChecked = Settings.SettingsFile.DeleteFile;
            BackupFileCheck.IsChecked = Settings.SettingsFile.BackupFile;
            MetadataCombo.SelectedIndex = Settings.SettingsFile.Metadata == "none" ? 0 : 1;
            WebPCompressionBox.Text = Settings.SettingsFile.WebpCompression.ToString(CultureInfo.InvariantCulture);
            GifCompressionBox.Text = Settings.SettingsFile.GifCompression.ToString(CultureInfo.InvariantCulture);
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e) {
            if (LosslessCheck.IsChecked != null) Settings.SettingsFile.Lossless = LosslessCheck.IsChecked.Value;
            if (NoAlphaCheck.IsChecked != null) Settings.SettingsFile.Noalpha = NoAlphaCheck.IsChecked.Value;
            if (DeleteFileCheck.IsChecked != null) Settings.SettingsFile.DeleteFile = DeleteFileCheck.IsChecked.Value;
            if (BackupFileCheck.IsChecked != null) Settings.SettingsFile.BackupFile = BackupFileCheck.IsChecked.Value;
            Settings.SettingsFile.Metadata = MetadataCombo.SelectedIndex == 0 ? "none" : "all";
            Settings.SettingsFile.WebpCompression = float.Parse(WebPCompressionBox.Text);
            Settings.SettingsFile.GifCompression = float.Parse(GifCompressionBox.Text);
            Settings.SaveSettings();
        }

        private void ResetButton_OnClick(object sender, RoutedEventArgs e) {
            LosslessCheck.IsChecked = true;
            NoAlphaCheck.IsChecked = false;
            DeleteFileCheck.IsChecked = true;
            BackupFileCheck.IsChecked = false;
            MetadataCombo.SelectedIndex = 0;
            WebPCompressionBox.Text = "80";
            GifCompressionBox.Text = "75";
        }

        private void ResetSetting(object sender, RoutedEventArgs e) {
            MenuItem menuItem = (MenuItem)sender;
            string header = menuItem.Name;
            switch (header) {
                case "ResetSettingLosses":
                    LosslessCheck.IsChecked = true;
                    break;
                case "ResetSettingNoAlpha":
                    NoAlphaCheck.IsChecked = false;
                    break;
                case "ResetSettingDeleteFile":
                    DeleteFileCheck.IsChecked = true;
                    break;
                case "ResetSettingBackupFile":
                    BackupFileCheck.IsChecked = false;
                    break;
                case "ResetSettingWebPCompression":
                    WebPCompressionBox.Text = "80";
                    break;
                case "ResetSettingAnimatedCompression":
                    GifCompressionBox.Text = "75";
                    break;
                case "ResetSettingMetadata":
                    MetadataCombo.SelectedIndex = 0;
                    break;
            }
        }

        private void TextBox_Input(object sender, TextCompositionEventArgs e) => e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e) => e.CancelCommand();
    }
}
