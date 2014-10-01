using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.IO;

namespace Check_Up.Util {
    class Scripts {

        private List<string> scripts = new List<string>();

        private string scriptPath = "scripts";
        private string fullScriptPath;

        ScriptRuntime ipy = Python.CreateRuntime();

        public Scripts() {
            fullScriptPath = Path.GetFullPath(scriptPath);
        }

        public void checkDirectory() {
            if (!Directory.Exists(fullScriptPath)) {
#if DEBUG
                Console.WriteLine("Creating scripts directory");
#endif
                Directory.CreateDirectory(fullScriptPath);
            }
            else {
#if DEBUG
                Console.WriteLine("Scripts directory already exists");
#endif
            }
        }

        public void runScripts() {
            string[] files = Directory.GetFiles(fullScriptPath);
            foreach (string filename in files) {
                dynamic test = ipy.UseFile(filename);
                test.main();
            }
        }

    }
}
