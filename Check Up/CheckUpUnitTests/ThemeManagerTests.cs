using System;
using Check_Up;
using Check_Up.Util;
using NUnit.Framework;

namespace CheckUpUnitTests {
    [TestFixture]
    public class ThemeManagerTests {

        [Test]
        public void ParseTheme_SameInputandReturn() {
            ThemeManager themeManager = new ThemeManager();

            string given = "Theme1.xaml";
            string expected = "Theme1.xaml";

            string returned = themeManager.ParseTheme(given);

            Assert.AreEqual(expected, returned);
        }

        [Test]
        public void ParseTheme_FullFilepath() {
            ThemeManager themeManager = new ThemeManager();

            string given = "C:\\Users\\Peter\\AppData\\Roaming\\Check Up\\Themes\\Theme1.xaml";
            string expected = "Theme1.xaml";

            string returned = themeManager.ParseTheme(given);

            Assert.AreEqual(expected, returned);
        }

        [Test]
        public void ParseTheme_FileNoExtension() {
            ThemeManager themeManager = new ThemeManager();

            string given = "Theme1";
            string expected = "Theme1";

            string returned = themeManager.ParseTheme(given);

            Assert.AreEqual(expected, returned);
        }

        [Test]
        public void ParseTheme_FullFileNoExtension() {
            ThemeManager themeManager = new ThemeManager();

            string given = "C:\\Users\\Peter\\AppData\\Roaming\\Check Up\\Themes\\Theme1";
            string expected = "Theme1";

            string returned = themeManager.ParseTheme(given);

            Assert.AreEqual(expected, returned);
        }

        [Test]
        public void ParseTheme_InputNull() {
            ThemeManager themeManager = new ThemeManager();

            string given = null;
            string expected = null;

            string returned = themeManager.ParseTheme(given);

            Assert.AreEqual(expected, returned);
        }

        [Test]
        public void ChangeTheme_InputNull() {
            ThemeManager themeManager = new ThemeManager();
            
            string given = null;

            themeManager.ChangeTheme(given);
        }

        [Test]
        public void ChangeTheme_InputNotExist() {
            ThemeManager themeManager = new ThemeManager();

            string given = "ThisThemeDoesntExist.xaml";

            themeManager.ChangeTheme(given);
        }
    }
}
