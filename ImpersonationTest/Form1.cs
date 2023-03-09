using ImpersonationTest.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImpersonationTest {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            textBox2.Clear();
            List<String> tt = Extensions.ImpersonateLoginByProcess(Convert.ToUInt32(textBox1.Text));

            textBox2.Text = String.Join("\r\n", tt);
        }
    }
}
