using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Check_Up.Util;

namespace CheckUpUnitTests {
    [TestFixture]
    class LoggerTests {
        #region DEBUG
        [Test]
        public void DebugMsgNull() {
            Logger.Debug(null);
        }

        [Test]
        public void DebugMsg() {
            Logger.Debug("Test Message");
        }
        #endregion

        #region WARN
        [Test]
        public void WarnMsgNull() {
            Logger.Warn(null);
        }

        [Test]
        public void WarnMsg() {
            Logger.Warn("Test Message");
        }
        #endregion

        #region ERROR
        [Test]
        public void ErrorMsgNull() {
            Logger.Error(null);
        }

        [Test]
        public void ErrorMsg() {
            Logger.Error("Test Message");
        }
        #endregion

        #region FATAL
        [Test]
        public void FatalMsgNull() {
            Logger.Fatal(null);
        }

        [Test]
        public void FatalMsg() {
            Logger.Fatal("Test Message");
        }
        #endregion

        #region INFO
        [Test]
        public void InfoMsgNull() {
            Logger.Info(null);
        }

        [Test]
        public void InfoMsg() {
            Logger.Info("Test Message");
        }
        #endregion
    }
}
