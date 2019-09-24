using RGiesecke.DllExport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RDS_Sender
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void RDSSend_Click(object sender, EventArgs e)
        {
            if (IPBox.Text == "localhost")
                Constants.IP = "172.0.0.1";
            else
                Constants.IP = IPBox.Text;
            Constants.Port = Convert.ToInt32(PortBox.Text);
        }
    }
}
