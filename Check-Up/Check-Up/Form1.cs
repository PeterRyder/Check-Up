using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Check_Up {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        // OK Button
        private void button2_Click(object sender, EventArgs e) {
            gathering_form subForm = new gathering_form();
            subForm.Show();
        }

        // Cancel Button
        private void deny_Click(object sender, EventArgs e) {
            Application.Exit();
            
        }

        private void confirm_MouseHover(object sender, EventArgs e) {

        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e) {
            properties_form subForm = new properties_form();
            subForm.Show();
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void aboutCheckUpToolStripMenuItem_Click(object sender, EventArgs e) {
            help_form subForm = new help_form();
            subForm.Show();
        }

        private void button1_Click(object sender, EventArgs e) {

        }
    }
}
