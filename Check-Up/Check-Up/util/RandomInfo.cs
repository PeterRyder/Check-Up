using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_Up.Util {
    static class RandomInfo {

        public static int logicalCpuCount = InitializeCpuCount();

        private static int InitializeCpuCount() {
            int logicalCpuCount = 0;
            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get()) {
                logicalCpuCount = Convert.ToInt32(item["NumberOfLogicalProcessors"]);
                Console.WriteLine("Found logical cores");
            }
            return logicalCpuCount;
        }

    }
}
