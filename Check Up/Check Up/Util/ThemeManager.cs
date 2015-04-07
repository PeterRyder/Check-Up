using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleToAttribute("CheckUpUnitTests")]

namespace Check_Up.Util {
    public class ThemeManager {

        public ObservableCollection<string> themes = new ObservableCollection<string>();

        public ThemeManager() {

        }

        public void LoadThemes() {
            Logger.Info("Loading Themes...");
                string[] files = Directory.GetFiles(FolderManager.ThemeDir);
                foreach (string file in files) {
                    string parsedFile = ParseTheme(file);
                    Logger.Debug("Found Theme " + parsedFile);
                    themes.Add(parsedFile);
                }
                if (files.Length == 0) {
                    Logger.Warn("Couldn't find any themes to load");
                }
        }

        internal string ParseTheme(string file) {
            return Path.GetFileName(file);
        }

        public void ChangeTheme(string theme) {
            string s = FolderManager.ThemeDir + "\\" + theme;

            try {
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() {
                    Source = new Uri(s, UriKind.RelativeOrAbsolute)
                });
            }
            catch {
                Logger.Error("Couldn't change theme to " + theme);
            }
            
        }

    }
}
