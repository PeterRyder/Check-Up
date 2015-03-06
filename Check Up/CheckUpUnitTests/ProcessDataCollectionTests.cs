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
            Dictionary<string, float> expected = new Dictionary<string, float>();
            Dictionary<string, float> returned = dataCollection.AggregateData(null);
            Assert.AreEqual(expected, returned);
        }

        [Test]
        public void AggregateDataInput1Process() {
            ProcessesDataCollection dataCollection = new ProcessesDataCollection();

            Dictionary<string, float> input = new Dictionary<string,float>();
            input.Add("chrome#1", 9.9f);
            input.Add("chrome#2", 10.0f);
            Dictionary<string, float> expected = new Dictionary<string, float>();
            expected.Add("chrome", 19.9f);
            Dictionary<string, float> returned = dataCollection.AggregateData(input);

            Assert.AreEqual(expected, returned);
        }

        [Test]
        public void AggregateDataInput2Processes() {
            ProcessesDataCollection dataCollection = new ProcessesDataCollection();

            Dictionary<string, float> input = new Dictionary<string, float>();
            input.Add("chrome#1", 9.9f);
            input.Add("chrome#2", 10.0f);

            input.Add("test#1", 9.9f);
            input.Add("test#2", 12.1f);

            Dictionary<string, float> expected = new Dictionary<string, float>();
            expected.Add("chrome", 19.9f);
            expected.Add("test", 22f);
            Dictionary<string, float> returned = dataCollection.AggregateData(input);

            Assert.AreEqual(expected, returned);
        }

        [Test]
        public void AggregateDataWeirdInputProcesses() {
            ProcessesDataCollection dataCollection = new ProcessesDataCollection();

            Dictionary<string, float> input = new Dictionary<string, float>();
            input.Add("chrome#1", 9.9f);
            input.Add("chrome#3", 10.0f);

            Dictionary<string, float> expected = new Dictionary<string, float>();
            expected.Add("chrome", 19.9f);
            Dictionary<string, float> returned = dataCollection.AggregateData(input);

            Assert.AreEqual(expected, returned);
        }

        [Test]
        public void CheckDataCounterInputNull() {

        }

    }
}
