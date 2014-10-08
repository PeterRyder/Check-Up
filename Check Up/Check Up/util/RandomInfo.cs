using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_Up.Util {
    static class RandomInfo {

        public static int logicalCpuCount = InitializeCpuCount();

        private static int InitializeCpuCount() {
            return Environment.ProcessorCount;
        }
    }
}
