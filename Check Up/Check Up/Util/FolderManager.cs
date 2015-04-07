using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Check_Up.Util {
    class FolderManager {
        // Storage Vars local to APPDATA/ROAMING/
        public static string RoamingDir = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\Check Up";

        public static string DataDir = RoamingDir + "\\" + "Data";
        public static string ScriptDir = RoamingDir + "\\" + "Scripts";
        public static string ThemeDir = RoamingDir + "\\" + "Themes";

        public static void CreateFolders() {
            CheckCreateFolder(DataDir);
            CheckCreateFolder(ScriptDir);
            CheckCreateFolder(ThemeDir);
        }

        public static void CheckCreateFolder(string folder) {
            if (!Directory.Exists(folder)) {
                Directory.CreateDirectory(folder);
            }
        }

    }
}
