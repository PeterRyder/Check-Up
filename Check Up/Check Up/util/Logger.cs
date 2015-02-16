using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Check_Up.Util
{
    static class Logger {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Debug(string msg) {
            log.Debug(msg);
        }

        public static void Warn(string msg) {
            log.Warn(msg);
        }

        public static void Error(string msg) {
            log.Error(msg);
            
        }

        public static void Fatal(string msg) {
            log.Fatal(msg);
        }

        public static void Info(string msg) {
            log.Info(msg);
        }
    }
}
