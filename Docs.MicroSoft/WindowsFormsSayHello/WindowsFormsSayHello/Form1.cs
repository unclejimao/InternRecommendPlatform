using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsSayHello
{
    public partial class Form1 : Form
    {
        Random rand;

        public Form1()
        {
            InitializeComponent();

            rand = new Random();

        }

        private byte[] GetRandomBytes(int n)
        {
            var randomBytes = new byte[n];

            rand.NextBytes(randomBytes);

            return randomBytes;
        }

        private void SayHelloButton_Click(object sender, EventArgs e)
        {
            byte[] rgb = GetRandomBytes(3);

            textBox1.ForeColor = Color.FromArgb(255, rgb[0], rgb[1], rgb[2]);
            SayHelloButton.ForeColor = Color.FromArgb(255, rgb[0], rgb[1], rgb[2]);
        }
    }
}
