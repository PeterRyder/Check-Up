using System;
using Check_Up;
using Check_Up.Util;
using NUnit.Framework;

namespace CheckUpUnitTests {
    [TestFixture]
    public class ScriptControlTests {

        [Test]
        public void CheckDirectory() {
            ScriptControl scripts = new ScriptControl();
            scripts.CheckDirectory();
        }

        [Test]
        public void CheckNewScript_InputNull() {
            ScriptControl scripts = new ScriptControl();

            string given = null;

            scripts.CheckNewScript(given);
        }

        [Test]
        public void SanityCheckScript_InputNull() {
            ScriptControl scripts = new ScriptControl();

            string given = null;
            bool expected = false;

            bool returned = scripts.SanityCheckScript(given);

            Assert.AreEqual(expected, returned);
        }

        [Test]
        public void SanityCheckScript_InputNotExist() {
            ScriptControl scripts = new ScriptControl();

            string given = "ThisFileDoesntExist.py";
            bool expected = false;

            bool returned = scripts.SanityCheckScript(given);

            Assert.AreEqual(expected, returned);
        }

        [Test]
        public void SanityCheckScript_InputWithoutExtension() {
            ScriptControl scripts = new ScriptControl();

            string given = "ThisFileDoesntExist";
            bool expected = false;

            bool returned = scripts.SanityCheckScript(given);

            Assert.AreEqual(expected, returned);
        }

        [Test]
        public void CheckFunctions_InputNull() {
            ScriptControl scripts = new ScriptControl();

            string given = null;
            bool expected = false;

            bool returned = scripts.CheckFunctions(given);

            Assert.AreEqual(expected, returned);
        }

        [Test]
        public void CheckFunctions_InputNotExist() {
            ScriptControl scripts = new ScriptControl();

            string given = "ThisFileDoesntExist.py";
            bool expected = false;

            bool returned = scripts.CheckFunctions(given);

            Assert.AreEqual(expected, returned);
        }

        [Test]
        public void TestTravisCI() {
            
        }

    }
}