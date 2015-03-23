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

            input.Add(new BackgroundData("chrome#1", 10f, 101f));
            input.Add(new BackgroundData("chrome#2", 9.9f, 50f));

            dataCollection.SetDataValues(input);

            List<BackgroundData> expected = new List<BackgroundData>();
            expected.Add(new BackgroundData("chrome", 10.9f, 151f));

            List<BackgroundData> returned = dataCollection.AggregateData();

            Assert.AreEqual(expected, returned);
        }

        [Test]
        public void AggregateDataInput2Processes() {
            ProcessesDataCollection dataCollection = new ProcessesDataCollection();

            List<BackgroundData> input = new List<BackgroundData>();
            input.Add(new BackgroundData("chrome#1", 9.9f, 100f));
            input.Add(new BackgroundData("chrome#2", 10.0f, 50f));

            input.Add(new BackgroundData("test#1", 9.9f, 100f));
            input.Add(new BackgroundData("test#2", 12.1f, 50f));

            List<BackgroundData> expected = new List<BackgroundData>();
            expected.Add(new BackgroundData("chrome", 19.9f, 150f));
            expected.Add(new BackgroundData("test", 22f, 150f));

            dataCollection.SetDataValues(input);

            List<BackgroundData> returned = dataCollection.AggregateData();

            Assert.AreEqual(expected, returned);
        }

        [Test]
        public void AggregateDataWeirdInputProcesses() {
            ProcessesDataCollection dataCollection = new ProcessesDataCollection();

            List<BackgroundData> input = new List<BackgroundData>();
            input.Add(new BackgroundData("chrome#1", 9.9f, 100f));
            input.Add(new BackgroundData("chrome#3", 10.0f, 50f));

            List<BackgroundData> expected = new List<BackgroundData>();
            expected.Add(new BackgroundData("chrome", 19.9f, 150f));

            dataCollection.SetDataValues(input);

            List<BackgroundData> returned = dataCollection.AggregateData();

            Assert.AreEqual(expected, returned);
        }

        [Test]
        public void CheckDataCounterInputNull() {

        }

    }
}
