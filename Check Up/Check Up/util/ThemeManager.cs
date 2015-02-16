using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows;

namespace Check_Up.Util {
    class ThemeManager {

        private static string themeDir = RandomInfo.roamingDir + "\\" + RandomInfo.themeDir;

        public ObservableCollection<string> themes = new ObservableCollection<string>();

        public ThemeManager() {
            CheckDirectory();
        }

        private void CheckDirectory() {
            if (!Directory.Exists(themeDir)) {
                Directory.CreateDirectory(themeDir);
            }
        }

        public void LoadThemes() {
            LoadThemes(themeDir);
        }

        public void LoadThemes(string directory) {
            Logger.Info("Loading Themes...");
            string[] files = Directory.GetFiles(directory);
            foreach (string file in files) {
                string parsedFile = ParseTheme(file);
                Logger.Debug("Found Theme " + parsedFile);
                themes.Add(parsedFile);
            }
        }

        private string ParseTheme(string file) {
            return Path.GetFileName(file);
        }

        public void ChangeTheme(string theme) {
            string s = themeDir + "\\" + theme;

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
