using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Check_Up.Util;

namespace CheckUpUnitTests {
    [TestFixture]
    class ProcessDataCollectionTests {

        [Test]
        public void AggragateDataInputNull() {
            ProcessesDataCollection dataCollection = new ProcessesDataCollection();
            List<BackgroundData> expected = new List<BackgroundData>();
            List<BackgroundData> returned = dataCollection.AggregateData();
            Assert.AreEqual(expected, returned);
        }

        [Test]
        public void AggregateDataInput1Process() {
            ProcessesDataCollection dataCollection = new ProcessesDataCollection();

            List<BackgroundData> input = new List<BackgroundData>();

            BackgroundData process1 = new BackgroundData();
            process1.CounterName = "chrome#1";
            process1.Cpu = 10f;
            process1.Mem = 101f;

            BackgroundData process2 = new BackgroundData();
            process2.CounterName = "chrome#2";
            process2.Cpu = 9.9f;
            process2.Mem = 50f;

            input.Add(process1);
            input.Add(process2);

            dataCollection.SetDataValues(input);

            List<BackgroundData> expected = new List<BackgroundData>();

            BackgroundData expectedProcess = new BackgroundData();
            expectedProcess.CounterName = "chrome";
            expectedProcess.Cpu = 10.9f;
            expectedProcess.Mem = 151f;

            expected.Add(expectedProcess);

            List<BackgroundData> returned = dataCollection.AggregateData();

            Assert.AreEqual(expected, returned);
        }

        [Test]
        public void AggregateDataInput2Processes() {
            ProcessesDataCollection dataCollection = new ProcessesDataCollection();

            List<BackgroundData> input = new List<BackgroundData>();

            BackgroundData process1 = new BackgroundData();
            process1.CounterName = "chrome#1";
            process1.Cpu = 9.9f;
            process1.Mem = 100f;

            BackgroundData process2 = new BackgroundData();
            process2.CounterName = "chrome#2";
            process2.Cpu = 10.0f;
            process2.Mem = 50f;

            BackgroundData process3 = new BackgroundData();
            process3.CounterName = "test#1";
            process3.Cpu = 9.9f;
            process3.Mem = 100f;

            BackgroundData process4 = new BackgroundData();
            process4.CounterName = "test#2";
            process4.Cpu = 12.1f;
            process4.Mem = 50f;

            input.Add(process1);
            input.Add(process2);
            input.Add(process3);
            input.Add(process3);

            List<BackgroundData> expected = new List<BackgroundData>();

            BackgroundData expectedProcess1 = new BackgroundData();
            expectedProcess1.CounterName = "chrome";
            expectedProcess1.Cpu = 19.9f;
            expectedProcess1.Mem = 150f;

            BackgroundData expectedProcess2 = new BackgroundData();
            expectedProcess2.CounterName = "test";
            expectedProcess2.Cpu = 22f;
            expectedProcess2.Mem = 150f;

            expected.Add(expectedProcess1);
            expected.Add(expectedProcess2);

            dataCollection.SetDataValues(input);

            List<BackgroundData> returned = dataCollection.AggregateData();

            Assert.AreEqual(expected, returned);
        }

        [Test]
        public void AggregateDataWeirdInputProcesses() {
            ProcessesDataCollection dataCollection = new ProcessesDataCollection();

            List<BackgroundData> input = new List<BackgroundData>();

            BackgroundData process1 = new BackgroundData();
            process1.CounterName = "chrome#1";
            process1.Cpu = 9.9f;
            process1.Mem = 100f;

            BackgroundData process2 = new BackgroundData();
            process2.CounterName = "chrome#3";
            process2.Cpu = 10.0f;
            process2.Mem = 50f;

            input.Add(process1);
            input.Add(process2);

            List<BackgroundData> expected = new List<BackgroundData>();

            BackgroundData expectedProcess = new BackgroundData();
            expectedProcess.CounterName = "chrome";
            expectedProcess.Cpu = 19.9f;
            expectedProcess.Mem = 150f;

            expected.Add(expectedProcess);

            dataCollection.SetDataValues(input);

            List<BackgroundData> returned = dataCollection.AggregateData();

            Assert.AreEqual(expected, returned);
        }

        [Test]
        public void CheckDataCounterInputNull() {

        }

    }
}
