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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Check_Up {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        List<Window> subWindows = new List<Window>();

        public MainWindow() {
            InitializeComponent();
        }

        private void MenuItemProperties_Click(object sender, RoutedEventArgs e) {
            PropertiesWindow subWindow = new PropertiesWindow();
            subWindows.Add(subWindow);
            subWindow.Show();
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void MenuItemAbout_Click(object sender, RoutedEventArgs e) {

        }

    }
}

