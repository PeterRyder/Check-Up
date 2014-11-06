﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using log4net;

namespace Check_Up.Util {
    class ProcessesDataCollection {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Maximum Values
        public Dictionary<string, float> HighestCpuUsage = new Dictionary<string, float>();
        public Dictionary<string, float> HighestMemUsage = new Dictionary<string, float>();

        List<PerformanceCounter> ProcessPerfCounters = new List<PerformanceCounter>();

        public ProcessesDataCollection() {
            
            foreach (var p in Process.GetProcesses()) {
                var cpuCounter = ProcessMonitor.GetPerfCounterForProcessId(p.Id, "% Processor Time");
                var memCounter = ProcessMonitor.GetPerfCounterForProcessId(p.Id, "Working Set - Private");

                if (cpuCounter != null) {
                    cpuCounter.NextValue();
                    ProcessPerfCounters.Add(cpuCounter);
                }

                if (memCounter != null) {
                    memCounter.NextValue();
                    ProcessPerfCounters.Add(memCounter);
                } 
            }
        }

        public void GatherData() {
#if DEBUG
            Stopwatch watch = new Stopwatch();
            watch.Start();
#endif

            for (int i = 0; i < ProcessPerfCounters.Count; i++ ) {
                float data;
                PerformanceCounter counter = ProcessPerfCounters[i];

                try {
                    data = counter.NextValue();
                }
                catch {
                    Console.WriteLine("Could not get information on process {0}", counter.InstanceName);
                    Console.WriteLine("Removing process {0}", counter.InstanceName);

                    ProcessPerfCounters.Remove(counter);
                    continue;
                }

                if (counter.CounterName == "% Processor Time") {
                    data = data / (float)RandomInfo.logicalCpuCount;
                    if (HighestCpuUsage.Count < Properties.Settings.Default.AmountProcesses) {
                        HighestCpuUsage[counter.InstanceName] = data;
                    }
                    else {
                        CheckData("CPU", counter, data, ref HighestCpuUsage);
                    }
                    continue;
                }

                if (counter.CounterName == "Working Set - Private") {
                    var MemMbs = data / 1024 / 1024;
                    if (HighestMemUsage.Count < Properties.Settings.Default.AmountProcesses) {
                        HighestMemUsage[counter.InstanceName] = MemMbs;
                    }
                    else {
                        CheckData("Memory", counter, MemMbs, ref HighestMemUsage);
                    }
                }
                 Thread.Sleep(30);
            }

#if DEBUG
            watch.Stop();

            TimeSpan ts = watch.Elapsed;

            // Format and display the TimeSpan value.
            
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
#endif
        }

        private void CheckData(string type, PerformanceCounter counter, float data, ref Dictionary<string, float> DataUsage) {
            List<string> keys = new List<string>(DataUsage.Keys);

            string lowest_key = keys[0];
            float lowest_value = DataUsage[keys[0]];

            foreach (string key in keys) {

                if (DataUsage[key] < lowest_value && key != lowest_key) {
                    lowest_key = key;
                    lowest_value = DataUsage[key];
                }
            }

            if (lowest_value < data) {
                DataUsage.Remove(lowest_key);
                DataUsage[counter.InstanceName] = data;
            }
        }

    }
}
