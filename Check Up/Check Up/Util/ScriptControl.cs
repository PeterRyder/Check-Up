using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using CSScriptLibrary;

namespace Check_Up.Util {
    public class ScriptControl {

        List<String> scriptFiles = new List<string>();

        public List<String> ScriptFiles {
            get { return scriptFiles; }
        }

        public ScriptControl() {
            
        }

        public void CheckScripts() {
            CheckScripts(FolderManager.ScriptDir);
        }

        public void CheckScripts(string dir) {
            foreach(string file in Directory.GetFiles(dir)) {
                if (!scriptFiles.Contains(dir)) {
                    scriptFiles.Add(file);
                }
               
            }
        }

        public string GetScriptContents(string filename) {
            if (filename != null) {
                return System.IO.File.ReadAllText(filename);
            }
            else {
                return null;
            }
        }

        private void test() {
            dynamic script = CSScript.Evaluator
                         .LoadCode(@"using System;
                                     public class Script
                                     {
                                         public int Sum(int a, int b)
                                         {
                                             return a+b;
                                         }
                                     }");
            int result = script.Sum(1, 2);
        }

        private Assembly BuildAssembly(string code) {
            Microsoft.CSharp.CSharpCodeProvider provider =
               new CSharpCodeProvider();
            ICodeCompiler compiler = provider.CreateCompiler();
            CompilerParameters compilerparams = new CompilerParameters();
            compilerparams.GenerateExecutable = false;
            compilerparams.GenerateInMemory = true;
            CompilerResults results =
               compiler.CompileAssemblyFromSource(compilerparams, code);
            if (results.Errors.HasErrors) {
                StringBuilder errors = new StringBuilder("Compiler Errors :\r\n");
                foreach (CompilerError error in results.Errors) {
                    errors.AppendFormat("Line {0},{1}\t: {2}\n",
                           error.Line, error.Column, error.ErrorText);
                }
                Console.WriteLine(errors.ToString());
                //throw new Exception(errors.ToString());
                return null;
            }
            else {
                return results.CompiledAssembly;
            }
        }

        public object ExecuteCode(string code,
    string namespacename, string classname,
    string functionname, bool isstatic, params object[] args) {

            object returnval = null;
            Assembly asm = BuildAssembly(code);
            if (asm == null) {
                return null;
            }
            object instance = null;
            Type type = null;
            if (isstatic) {
                type = asm.GetType(namespacename + "." + classname);
            }
            else {
                instance = asm.CreateInstance(namespacename + "." + classname);
                type = instance.GetType();
            }

            MethodInfo method = type.GetMethod(functionname);

            if (method == null) {
                Console.WriteLine("Method is null");
            }
            else {
                returnval = method.Invoke(instance, args);
                return returnval;
            }

            return null;
        }
    }
}
