using System;
using System.IO;
using System.Text.Json;

namespace Converter.WebP.Windows.API {
    internal class Settings {
        public static SettingsFile SettingsFile;
        internal static void LoadSettings() => SettingsFile = JsonSerializer.Deserialize<SettingsFile>(File.ReadAllText($"{Environment.CurrentDirectory}\\settings.json"));
        internal static void SaveSettings() => File.WriteAllText("settings.json", JsonSerializer.Serialize(SettingsFile));
    }
}
