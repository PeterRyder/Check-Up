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

        public Dictionary<string, float> HighestCpuUsage = new Dictionary<string, float>();
        public Dictionary<string, float> HighestMemUsage = new Dictionary<string, float>();
        
        List<PerformanceCounter> perfCounters = new List<PerformanceCounter>();

        public ProcessesDataCollection() {
            
            foreach (var p in Process.GetProcesses()) {
                var cpuCounter = ProcessMonitor.GetPerfCounterForProcessId(p.Id, "% Processor Time");
                var memCounter = ProcessMonitor.GetPerfCounterForProcessId(p.Id, "Working Set - Private");

                if (cpuCounter != null) {
                    cpuCounter.NextValue();
                    perfCounters.Add(cpuCounter);
                }

                if (memCounter != null) {
                    memCounter.NextValue();
                    perfCounters.Add(memCounter);
                } 
            }
        }

        public void GatherData() {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            for (int i = 0; i < perfCounters.Count; i++ ) {
                float data;
                PerformanceCounter counter = perfCounters[i];

                try {
                    data = counter.NextValue();
                }
                catch {
                    Console.WriteLine("Could not get information on process {0}", counter.InstanceName);
                    Console.WriteLine("Removing process {0}", counter.InstanceName);

                    perfCounters.Remove(counter);
                    continue;
                }

                if (counter.CounterName == "% Processor Time") {
                    data = data / (float)RandomInfo.logicalCpuCount;
                    if (HighestCpuUsage.Count < 5) {
                        HighestCpuUsage[counter.InstanceName] = data;
                    }
                    else {
                        CheckData("CPU", counter, data, ref HighestCpuUsage);
                    }
                    continue;
                }

                if (counter.CounterName == "Working Set - Private") {
                    var MemMbs = data / 1024 / 1024;
                    if (HighestMemUsage.Count < 5) {
                        HighestMemUsage[counter.InstanceName] = MemMbs;
                    }
                    else {
                        CheckData("Memory", counter, MemMbs, ref HighestMemUsage);
                    }
                }
                 Thread.Sleep(30);
            }

            
            watch.Stop();

            TimeSpan ts = watch.Elapsed;

            // Format and display the TimeSpan value.
            
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
             
        }

        private void CheckData(string type, PerformanceCounter counter, float data, ref Dictionary<string, float> DataUsage) {
            

            List<string> keys = new List<string>(DataUsage.Keys);

            string lowest_key = keys[0];
            float lowest_value = DataUsage[keys[0]];

            foreach (string key in keys) {

                float current_value = DataUsage[key];


                //Console.WriteLine("{0} : {1}", key, current_value);

                if (current_value < lowest_value) {

                    //Console.WriteLine("Prev Lowest Value: {0}, {1}",lowest_key, lowest_value);

                    lowest_key = key;
                    lowest_value = DataUsage[key];

                    
                    //Console.WriteLine("New Lowest Value: {0}, {1}", lowest_key, lowest_value);

                    break;
                }
            }

            if (lowest_value < data) {
                DataUsage.Remove(lowest_key);
                DataUsage[counter.InstanceName] = data;
            }
        }

    }
}
