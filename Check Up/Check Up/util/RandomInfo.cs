using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Check_Up.Util {
    static class RandomInfo {

        public static int logicalCpuCount = Environment.ProcessorCount;
        public static DriveInfo[] drives = DriveInfo.GetDrives();

        // Storage Vars local to APPDATA/ROAMING/
        public static string roamingDir = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\Check Up";

        public static string dataDir = "Data";
        public static string scriptDir = "Scripts";
        public static string themeDir = "Themes";

        public static void PrintDrives() {
            Console.WriteLine("\nPrinting Drive Info\n");
            foreach (DriveInfo drive in drives) {
                Console.WriteLine("Drive {0}", drive.Name);
                Console.WriteLine("  File Type: {0}", drive.DriveType);
                if (drive.IsReady == true) {
                    Console.WriteLine("  Total Size of Drive: {0}", drive.TotalSize);
                    Console.WriteLine("  Total Free Space on Drive: {0}", drive.AvailableFreeSpace);
                }   
            }
            Console.WriteLine("");
        }
    }
}
