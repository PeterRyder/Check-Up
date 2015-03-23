using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Check_Up.Util;
using System.Diagnostics;

namespace CheckUpUnitTests {
    [TestFixture]
    class OSDataCollectionTests {

        [Test]
        public void AddDiskCounterInputNull() {
            OSDataCollection os = new OSDataCollection();
            os.AddDiskCounter(null);
        }

        [Test]
        public void AddDiskCounterInputStringWrong() {
            OSDataCollection os = new OSDataCollection();

            string input = "bad_disk";
            bool expected = false;

            bool returned = os.AddDiskCounter(input);

            Assert.AreEqual(expected, returned);
        }

        [Test]
        public void AddDiskCounterInputCorrect() {
            OSDataCollection os = new OSDataCollection();

            string input = "C:";
            bool expected = true;

            bool returned = os.AddDiskCounter(input);

            Assert.AreEqual(expected, returned);
        }

        [Test]
        public void RemoveCounterInputNull() {
            OSDataCollection os = new OSDataCollection();

            bool expected = false;

            bool returned = os.RemoveCounter(null);

            Assert.AreEqual(expected, returned);
        }

        [Test]
        public void RemoveCounterInputStringWrong() {
            OSDataCollection os = new OSDataCollection();

            string input = "bad input";
            bool expected = false;

            bool returned = os.RemoveCounter(input);

            Assert.AreEqual(expected, returned);
        }

        [Test]
        public void RemoveCounterInputCorrect() {
            OSDataCollection os = new OSDataCollection();

            // Add item to structures which are then removed properly (hopefully)
            PerformanceCounter perfCpuCount = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            os.PerfCounters.Add(CounterNames.CPUName, perfCpuCount);
            os.DataValues.Add(CounterNames.CPUName, 0);

            string input = CounterNames.CPUName;
            bool expected = true;

            bool returned = os.RemoveCounter(input);
            
            // Check return value
            Assert.AreEqual(expected, returned);

            // Check both structures are empty
            Assert.IsEmpty(os.PerfCounters);
            Assert.IsEmpty(os.DataValues);
        }

        
    }
}
