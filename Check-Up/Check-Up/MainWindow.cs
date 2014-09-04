using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ReadWriteCsv {
    public partial class MainWindow : Form {

        public MainWindow() {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        // OK Button
        private void button2_Click(object sender, EventArgs e) {
            DataGatheringForm subForm = new DataGatheringForm();
            subForm.Show();
        }

        // Cancel Button
        private void deny_Click(object sender, EventArgs e) {
            Application.Exit();

        }

        private void confirm_MouseHover(object sender, EventArgs e) {

        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e) {
            PropertiesForm subForm = new PropertiesForm();
            subForm.Show();

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void aboutCheckUpToolStripMenuItem_Click(object sender, EventArgs e) {
            HelpForm subForm = new HelpForm();
            subForm.Show();
        }

        private void button1_Click(object sender, EventArgs e) {

        }

        private void button2_Click_1(object sender, EventArgs e) {
            using (CsvFileReader reader = new CsvFileReader("DataOutput.csv")) {
                CsvRow row = new CsvRow();
                int i = 1;
                while (reader.ReadRow(row)) {
                    string type = row[0];
                    string value = row[1];
                    
                    try {
                        this.chart1.Series.Add(type);
                    }
                    catch {
#if DEBUG
                        Console.WriteLine("Already had chart with data type {0}", type);
#endif
                    }

                    try {
                        this.chart1.Series[type].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    }
                    catch {
                        Console.WriteLine("Couldn't change {0} into line graph", type);
                    }
                    try {
                        this.chart1.Series[type].Points.AddXY("" + i, value);
                        chart1.Series[type].ToolTip = "X: #VALX, Y: #VALY";
                    }
                    catch {
                        Console.WriteLine("Couldn't create point on graph X: {0}, Y: {1}", i, value);
                    }
                    i++;
                }
            }

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e) {

        }
    }
}
