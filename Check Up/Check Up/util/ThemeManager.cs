using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;

namespace Check_Up.Util {
    class ThemeManager {

        public string themeDir = RandomInfo.roamingDir + "\\" + RandomInfo.themeDir;

        public ObservableCollection<string> themes = new ObservableCollection<string>();

        public ThemeManager() {
            CheckDirectory();
            LoadThemes(themeDir);
        }

        private void CheckDirectory() {
            if (!Directory.Exists(themeDir)) {
                Directory.CreateDirectory(themeDir);
            }
        }

        public void LoadThemes(string directory) {
            string[] files = Directory.GetFiles(directory);
            foreach (string file in files) {
                string parsedFile = ParseTheme(file);
                themes.Add(parsedFile);
            }
        }

        public string ParseTheme(string file) {
            return Path.GetFileName(file);
        }

    }
}
