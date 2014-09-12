using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Net.NetworkInformation;

namespace Check_Up {
    class DataCollection {

        #region Performance Counters
        // create perf mon objects
        private PerformanceCounter perfCpuCount; 
        private PerformanceCounter perfMemCount; 
        private PerformanceCounter perfNetCount;
        private PerformanceCounter perfDiskCount;
        #endregion

        public int currentCPUUsage;

        public int availableMemMBs;
        public int availableMemGBs;

        public int currentNetUsageBytes;
        public int currentNetUsageMBs;

        public int percentDiskTime;

        public bool shouldGatherData;

        public DataCollection() {
            #region Network Adapter Initialization
            string WifiNicDescription = "";
            string ethernetNicDescription = "";

            // find the wifi network interface
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces()) {
                if (nic.Name == "Wi-Fi") {
                    WifiNicDescription = nic.Description;
                    WifiNicDescription = WifiNicDescription.Replace("(", "[");
                    WifiNicDescription = WifiNicDescription.Replace(")", "]");
                    Console.WriteLine(WifiNicDescription);
                }

                if (nic.Name == "Ethernet") {
                    ethernetNicDescription = nic.Description;
                    ethernetNicDescription = ethernetNicDescription.Replace("(", "[");
                    ethernetNicDescription = ethernetNicDescription.Replace(")", "]");
                    Console.WriteLine(ethernetNicDescription);
                }
            }
            #endregion

            // initialize performance counters
            perfCpuCount = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            perfMemCount = new PerformanceCounter("Memory", "Available MBytes");

            if (WifiNicDescription != "") {
                perfNetCount = new PerformanceCounter("Network Adapter", "Bytes Total/sec", WifiNicDescription);
            }
            else {
                perfNetCount = new PerformanceCounter("Network Adapter", "Bytes Total/sec", ethernetNicDescription);
            }

            perfDiskCount = new PerformanceCounter("LogicalDisk", "% Disk Time", "C:");

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
                availableMemMBs = (int)perfMemCount.NextValue();
#if DEBUG
                Console.WriteLine("Available Memory: {0}MB", availableMemMBs);
#endif
            }

            if (Properties.Settings.Default.Network) {
                currentNetUsageBytes = (int)perfNetCount.NextValue();
                currentNetUsageMBs = currentNetUsageBytes / 1000;
#if DEBUG
                Console.WriteLine("Network Bytes Total/sec: {0} Bytes", currentNetUsageMBs);
#endif
            }

            if (Properties.Settings.Default.DiskIO) {
                percentDiskTime = (int)perfDiskCount.NextValue();
#if DEBUG
                Console.WriteLine("Percent Disk Time: {0}%", percentDiskTime);
#endif
            }

            return true;
        }
    }
}
