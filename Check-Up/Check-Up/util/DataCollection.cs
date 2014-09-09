using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace Check_Up {
    class DataCollection {

        #region Performance Counters
        // create perf mon objects
        private PerformanceCounter perfCpuCount = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
        private PerformanceCounter perfMemCount = new PerformanceCounter("Memory", "Available MBytes");

        // currently cannot use these two performance metrics - dunno why not...
        //private PerformanceCounter perfNetCount = new PerformanceCounter("Network Interface", "Bytes Total/sec");
        //private PerformanceCounter perfDiskCount = new PerformanceCounter("LogicalDisk", "% Disk Time", "_Total");
        #endregion

        public int currentCPUUsage;
        public int currentMemUsage;

        public bool shouldGatherData;

        public DataCollection() {

        }

        public void ReadSettings() {
            #region Debug Console Output
#if DEBUG
            Console.WriteLine("CPU property: " + Properties.Settings.Default.CPU);
            Console.WriteLine("Memory property: " + Properties.Settings.Default.Memory);
            Console.WriteLine("Network property: " + Properties.Settings.Default.Network);
            Console.WriteLine("DiskIO property: " + Properties.Settings.Default.DiskIO);
#endif
            #endregion
            if (!Properties.Settings.Default.CPU &&
                !Properties.Settings.Default.Memory &&
                !Properties.Settings.Default.Network &&
                !Properties.Settings.Default.DiskIO) {
#if DEBUG
                Console.WriteLine("Settings indicate there is nothing to monitor");
#endif
                shouldGatherData = false;
            }
            else {
                shouldGatherData = true;
            }
        }

        public bool GatherData() {

            // gather and record CPU utilization
            if (Properties.Settings.Default.CPU) {
                currentCPUUsage = (int)perfCpuCount.NextValue();
#if DEBUG
                Console.WriteLine("Cpu Load: {0}%", currentCPUUsage);
#endif
            }
            // gather and record available memory
            if (Properties.Settings.Default.Memory) {
                currentMemUsage = (int)perfMemCount.NextValue();
#if DEBUG
                Console.WriteLine("Available Memory: {0}MB", currentMemUsage);
#endif
            }

            return true;
        }
    }
}
