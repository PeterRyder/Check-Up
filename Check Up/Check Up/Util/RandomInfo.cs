using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Check_Up.Util {
    public static class RandomInfo {

        public static int logicalCpuCount = Environment.ProcessorCount;
        public static DriveInfo[] drives = DriveInfo.GetDrives();

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
