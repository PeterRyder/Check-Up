using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Check_Up.Util;
using System.IO;
using System.Collections.ObjectModel;

namespace Check_Up {
    /// <summary>
    /// Interaction logic for ScriptWindow.xaml
    /// </summary>
    public partial class ScriptWindow : Window {

        public class ScriptData {
            public string ScriptName { get; set; }
            public string FullPath { get; set; }
        }

        ObservableCollection<ScriptData> _ScriptCollection = new ObservableCollection<ScriptData>();

        private ScriptControl scripts;

        public ScriptWindow() {
            InitializeComponent();

            scripts = new ScriptControl();

            InitializeListView();

        }

        public ObservableCollection<ScriptData> ScriptCollection { get { return _ScriptCollection; } }

        public void InitializeListView() {
            scripts.checkNewScripts();

            foreach (String script in scripts.getScripts()) {
                _ScriptCollection.Add(new ScriptData {
                    ScriptName = System.IO.Path.GetFileName(script),
                    FullPath = script
                });
            }
        }

        private void StartScript(object sender, RoutedEventArgs e) {

            Button b = sender as Button;
            ScriptData scriptData = b.CommandParameter as ScriptData;

            if (scripts.runScript(scriptData.FullPath)) {
                //b.IsEnabled = false;
            }

        }

        private void StopScript(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            ScriptData scriptData = b.CommandParameter as ScriptData;

            if (scripts.stopScript(scriptData.FullPath)) {
                //b.IsEnabled = false;
            }
        }
    }
}
