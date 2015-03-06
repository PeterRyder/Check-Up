using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Check_Up.Util;

namespace CheckUpUnitTests {
    [TestFixture]
    class OSDataCollectionTests {

        [Test]
        public void AddDiskCounterInputNull() {
            OSDataCollection os = new OSDataCollection();
            os.AddDiskCounter(null);
        }
    }
}
