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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Converter.WebP.Windows.API {
    public class Reference {
        internal static PlatformID CurrentPlatformId => Environment.OSVersion.Platform;
        internal static bool Is64Bit => Environment.Is64BitOperatingSystem;
        internal static bool IsMultiCore => Environment.ProcessorCount > 1;
        internal static ObservableCollection<DroppedImage> ImageCollection { get; } = new ObservableCollection<DroppedImage>();
        internal static readonly List<string> ImageTypes = new List<string>() { ".png", ".jpeg", ".jpg", ".exif", ".tiff", ".bmp", ".gif" };
        internal static ListView ListView { get; set; }
        internal static Dispatcher MainDispatcher { get; set; }
        internal static List<Thread> StartedThreads = new List<Thread>();
        internal static long TotalSize { get; set; } = 0;
        internal static Label TotalSizeLabel { get; set; }
        internal static long ConvertedSize { get; set; } = 0;
        internal static Label ConvertedSizeLabel { get; set; }

        internal static Process Process = new Process() {
            StartInfo = {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                UseShellExecute = false
            }
        };

        internal static void StartProcess() {
            Process.Start();
            Process.StandardInput.WriteLine(Reference.Is64Bit
                ? $"cd {Environment.CurrentDirectory}\\Libs\\WindowsX64"
                : $"cd {Environment.CurrentDirectory}\\Libs\\WindowsX86");
        }

        internal static void ExitProcess() {
            Process.StandardInput.Flush();
            Process.StandardInput.Close();
            Process.WaitForExit();
            Process.Close();
        }

        /// <summary>
        /// We use this to try and stop any threads from continuing on a crash.
        /// This might not work on some crashes that just kill the program.
        /// https://docs.microsoft.com/en-us/dotnet/api/system.appdomain.unhandledexception?redirectedfrom=MSDN&amp;view=netcore-3.1
        /// </summary>
        internal static void ExceptionHandler(object sender, UnhandledExceptionEventArgs args) {
            foreach (Thread startedThread in StartedThreads) {
                startedThread.Abort();
            }
        }
    }
}
