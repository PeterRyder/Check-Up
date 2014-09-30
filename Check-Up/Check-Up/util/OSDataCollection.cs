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

namespace Check_Up.Util {
    class OSDataCollection {

        #region Performance Counters
        // create perf mon objects
        private PerformanceCounter perfCpuCount;
        private PerformanceCounter perfMemCount;
        private PerformanceCounter perfNetCount;
        private PerformanceCounter perfDiskCount;
        #endregion

        public int currentCPUUsage;

        public double totalMemMBs;
        public double availableMemMBs;
        public double currentMemUsage;

        public int currentNetUsageBytes;
        public double currentNetUsageMBs;

        public int percentDiskTime;

        public bool canGatherNet;

        public OSDataCollection() {
#if DEBUG
            //ListCounters("Network Adapter");
#endif

            #region CPU Counter Initialization
            perfCpuCount = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            #endregion

            #region Memory Counter Initialization
            ulong totalMemBytes = new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;
            totalMemMBs = (int)(totalMemBytes / 1024 / 1024);
            perfMemCount = new PerformanceCounter("Memory", "Available MBytes");
            #endregion

            #region Network Counter Initialization
            string WifiNicDescription = "";
            string ethernetNicDescription = "";

            // find the wifi network interface
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces()) {
                //Console.WriteLine(nic.Name);
                if (nic.Name == "Wi-Fi" || nic.Name == "Wireless Network Connection") {
                    WifiNicDescription = nic.Description;
                    WifiNicDescription = WifiNicDescription.Replace("(", "[");
                    WifiNicDescription = WifiNicDescription.Replace(")", "]");
                    //Console.WriteLine(WifiNicDescription);
                }

                if (nic.Name == "Ethernet" || nic.Name == "Local Area Connection") {
                    ethernetNicDescription = nic.Description;
                    ethernetNicDescription = ethernetNicDescription.Replace("(", "[");
                    ethernetNicDescription = ethernetNicDescription.Replace(")", "]");
                    //Console.WriteLine(ethernetNicDescription);
                }
            }

            if (WifiNicDescription != "") {
#if DEBUG
                Console.WriteLine(WifiNicDescription);
#endif
                try {
                    perfNetCount = new PerformanceCounter("Network Adapter", "Bytes Total/sec", WifiNicDescription);
                    canGatherNet = true;
                }
                catch {
#if DEBUG
                    Console.WriteLine("Could not find a counter for your network adapter");
#endif
                    canGatherNet = false;
                }

                try {
                    perfNetCount = new PerformanceCounter("Network Interface", "Bytes Total/sec", WifiNicDescription);
                    canGatherNet = true;
                }
                catch {
#if DEBUG
                    Console.WriteLine("Could not find a counter for your network interface");
#endif
                    canGatherNet = false;
                }
            }
            else {
                perfNetCount = new PerformanceCounter("Network Adapter", "Bytes Total/sec", ethernetNicDescription);
            }

            #endregion

            #region Disk Counter Initialization
            perfDiskCount = new PerformanceCounter("LogicalDisk", "% Disk Time", "C:");
            #endregion

#if DEBUG
            Console.WriteLine("Finished PerformanceCounter Initialization");
#endif
        }

        /// <summary>
        /// Will gather data on devices which are checked from the properties form
        /// Converts all data to percentages EXCEPT for Networking - this is in MBps
        /// </summary>
        /// <returns></returns>
        public bool GatherData() {
            #region CPU Data Gathering
            if (Properties.Settings.Default.CPU) {
                currentCPUUsage = (int)perfCpuCount.NextValue();
#if DEBUG
                Console.WriteLine("Cpu Load: {0}%", currentCPUUsage);
#endif
            }
            #endregion

            #region Memory Data Gathering
            if (Properties.Settings.Default.Memory) {
                availableMemMBs = (int)perfMemCount.NextValue();
                currentMemUsage = Math.Round((totalMemMBs - availableMemMBs) / totalMemMBs * 100d, 2);
#if DEBUG
                Console.WriteLine("Available MBs: {0}", availableMemMBs);
                Console.WriteLine("Total MBs: {0}", totalMemMBs);
                Console.WriteLine("Available Memory: {0}%", currentMemUsage);
#endif
            }
            #endregion

            #region Network Data Gathering
            if (Properties.Settings.Default.Network) {
                if (canGatherNet) {
                    currentNetUsageBytes = (int)perfNetCount.NextValue();
                    currentNetUsageMBs = Math.Round(currentNetUsageBytes / 1024d / 1024d, 2);
#if DEBUG
                    Console.WriteLine("Network Bytes Total/sec: {0} MBs", currentNetUsageMBs);
#endif
                }
            }
            #endregion

            #region Disk Data Gathering
            if (Properties.Settings.Default.DiskIO) {
                percentDiskTime = (int)perfDiskCount.NextValue();
#if DEBUG
                Console.WriteLine("Percent Disk Time: {0}%", percentDiskTime);
#endif
            }
            #endregion
            return true;
        }

        /// <summary>
        /// Debug function to list all counters on the system
        /// </summary>
        /// <param name="categoryName"></param>
        public void ListCounters(string categoryName) {
            PerformanceCounterCategory category = PerformanceCounterCategory.GetCategories().First(c => c.CategoryName == categoryName);
            Console.WriteLine("{0} [{1}]", category.CategoryName, category.CategoryType);

            string[] instanceNames = category.GetInstanceNames();

            if (instanceNames.Length > 0) {
                // MultiInstance categories
                foreach (string instanceName in instanceNames) {
                    ListInstances(category, instanceName);
                }
            }
            else {
                // SingleInstance categories
                ListInstances(category, string.Empty);
            }
        }

        /// <summary>
        /// Debug function to list all instances of a performanceCounterCategory
        /// </summary>
        /// <param name="category"></param>
        /// <param name="instanceName"></param>
        private static void ListInstances(PerformanceCounterCategory category, string instanceName) {
            Console.WriteLine("    {0}", instanceName);
            PerformanceCounter[] counters = category.GetCounters(instanceName);

            foreach (PerformanceCounter counter in counters) {
                Console.WriteLine("        {0}", counter.CounterName);
            }
        }
    }
}
