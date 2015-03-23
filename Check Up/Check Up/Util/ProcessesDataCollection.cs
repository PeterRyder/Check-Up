﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Check_Up.Util {
    public class ProcessesDataCollection {

        List<PerformanceCounter> processPerfCounters = new List<PerformanceCounter>();
        List<BackgroundData> dataValues = new List<BackgroundData>();

        public List<PerformanceCounter> ProcessPerfCounters {
            get { return processPerfCounters; }
        }

        public List<BackgroundData> DataValues {
            get { return dataValues; }
        }

        /// <summary>
        /// Only to be used in Unit Tests 
        /// </summary>
        /// <param name="b"></param>
        internal void SetDataValues(List<BackgroundData> b) {
            dataValues = b;
        }

        public ProcessesDataCollection() {
            
        }

        public void LoadProcessCounters() {
            processPerfCounters = ProcessMonitor.GetPerfCountersOfProcesses("% Processor Time");

            List<PerformanceCounter> temp = ProcessMonitor.GetPerfCountersOfProcesses("Working Set - Private");
            
            processPerfCounters.AddRange(temp);   
        }

        public void GatherData(bool FirstRun) {
#if DEBUG
            Stopwatch stopwatch = Stopwatch.StartNew();
#endif
            Logger.Info(String.Format("ProcessPerfCounters Count: {0}", processPerfCounters.Count));

            for (int i = 0; i < processPerfCounters.Count; i++ ) {

                float data;
                PerformanceCounter counter = processPerfCounters[i];

                try {

                    data = counter.NextValue();
                }
                catch {
                    Logger.Warn(string.Format("Could not get information on process {0}", counter.InstanceName));
                    Logger.Warn(string.Format("Removing process {0}", counter.InstanceName));

                    processPerfCounters.Remove(counter);
                    continue;
                }

                if (!FirstRun) {
                    var response = dataValues.Find(r => (r.CounterName == counter.InstanceName));
                    if (response == null) {
                        BackgroundData item = new BackgroundData(counter.InstanceName);

                        if (counter.CounterName == "% Processor Time") {
                            data = data / (float)RandomInfo.logicalCpuCount;
                            item.Cpu = data;
                        }

                        if (counter.CounterName == "Working Set - Private") {
                            item.Mem = data / 1024f / 1024f;
                        }

                        dataValues.Add(item);

                    }
                    else {
                        BackgroundData item = new BackgroundData(counter.InstanceName);
                        dataValues.Remove(item);
                        if (counter.CounterName == "% Processor Time") {
                            data = data / (float)RandomInfo.logicalCpuCount;
                            item.Cpu = data;
                            item.Mem = response.Mem;
                        }

                        if (counter.CounterName == "Working Set - Private") {
                            item.Mem = data / 1024f / 1024f;
                            item.Cpu = response.Cpu;
                        }

                        dataValues.Add(item);
                    }
                }
            }

            if (!FirstRun) {
                dataValues = AggregateData();
            }
            Logger.Debug("Finished gathering process data");

#if DEBUG
            stopwatch.Stop();
            Console.WriteLine("[time] GatherData (ProcessDataCollection) function completed in: " + stopwatch.ElapsedMilliseconds + "ms");
#endif
        }

        internal List<BackgroundData> AggregateData() {
#if DEBUG
            Stopwatch stopwatch = Stopwatch.StartNew();
#endif
            List<BackgroundData> NewData = new List<BackgroundData>();
            
            if (dataValues == null) {
                Logger.Warn("DataValues list in AggregateData function is null");
                return NewData;
            }

            foreach (BackgroundData item in dataValues) {
                int PoundIndex = item.CounterName.IndexOf('#');

                if (PoundIndex != -1) {
                    string newName = item.CounterName.Substring(0, PoundIndex);

                    var response = NewData.Find(r => (r.CounterName == newName));

                    if (response == null) {
                        BackgroundData newItem = new BackgroundData(newName);
                        newItem.Cpu = item.Cpu;
                        newItem.Mem = item.Mem;
                        NewData.Add(newItem);
                    }
                    else {

                        NewData.Remove(response);
                        response.Cpu += item.Cpu;
                        response.Mem += item.Mem;
                        NewData.Add(response);
                        
                    }
                }
                else {
                    var response = NewData.Find(r => (r.CounterName == item.CounterName));

                    if (response == null) {
                        BackgroundData newItem = new BackgroundData(item.CounterName);
                        newItem.Cpu = item.Cpu;
                        newItem.Mem = item.Mem;
                        NewData.Add(newItem);
                    }
                    else {

                        NewData.Remove(response);
                        response.Cpu += item.Cpu;
                        response.Mem += item.Mem;
                        NewData.Add(response);

                    }     
                }
            }


#if DEBUG
            stopwatch.Stop();
            Console.WriteLine("[time] Aggregate (ProcessDataCollection) function completed in: " + stopwatch.ElapsedMilliseconds + "ms");
#endif

            return NewData;
        }

    }
}
