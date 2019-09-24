using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//STUFF I LIKE
using RGiesecke.DllExport;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO.Ports;
//STUFF I LIKE



namespace PS_Decoder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //GetSerialPorts();
        }
        static Form1 frm = new Form1();

        //Some Helper Functions

        static string Decode(string input)
        {
            byte[] utf8Bytes = Encoding.UTF8.GetBytes(input);
            return Encoding.UTF8.GetString(utf8Bytes);

            //byte[] bytes = Encoding.GetEncoding(28591)
            //                       .GetBytes(input);
            //return Encoding.UTF8.GetString(bytes);
        }
        static void UpdateText(string PI, string PS)
        {
            string text = "PS: " + Decode(PS) + '\n';
            frm.richTextBox1.AppendText(text); 
        }

        /*
        static void GetSerialPorts()
        {
            string[] Ports = SerialPort.GetPortNames();
            frm.comboBox1.Items.AddRange(Ports);

        }
        */

        static void SendGroups(string group1, string group2, string group3, string group4)
        {
            string text = "G:" + group1 + group2 + group3 + group4 + '\n';
            string serialText = "G:" + group1 + group2 + group3 + group4;
            frm.richTextBox1.AppendText(text);
            if (frm.serialPort1.IsOpen)
            {
                frm.serialPort1.WriteLine(serialText);
            }
        }
        //Some Helper Functions

            //RDS Spy need this to register this plugin properly
        public struct TRDSGroup
        {
            public ushort Year;
            public byte Month;
            public byte Day;
            public byte Hour;
            public byte Minutes;
            public byte Second;
            public byte Centisecond;
            public ushort RFU;
            public int Blk1;
            public int Blk2;
            public int Blk3;
            public int Blk4;

        }
        static int PI;
        static int PS;
        static TRDSGroup Group;

        [DllExport("RDSGroup", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        unsafe static void RDSGroup(TRDSGroup* PRDSGroup)
        {
            Group = *PRDSGroup;
            PI = Group.Blk1;
            PS = Group.Blk2;
            //UpdateText(PI.ToString("X4"), PS.ToString());
            SendGroups(Group.Blk1.ToString("X4"), Group.Blk2.ToString("X4"), Group.Blk3.ToString("X4"), Group.Blk4.ToString("X4"));

            
            if (Group.Blk1 >= 0)
            {
                if (PI != Group.Blk1) {
                    PI = Group.Blk1;
                }
            }
            

        }
        [DllExport("Command", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        static void Command(string Cmd, string Param)
        {
            var w = "";
            w = Cmd.ToUpper();
            if (w == "CONFIGURE")
            {
                frm.Text = "PI Code: " + PI.ToString("X4");
                frm.ShowDialog();

            }
            if (w == "RESETDATA")
            {
                PI = -1;
            }
        }
        [DllExport("PluginName", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        static string PluginName()
        {
            return "RDS Sender";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = textBox1.Text;
                serialPort1.BaudRate = Convert.ToInt32(textBox2.Text);
                serialPort1.Open();
                progressBar1.Value = 100;
                button1.Enabled = false;
                button2.Enabled = true;
            }
            catch(UnauthorizedAccessException)
            {
                Debug.WriteLine("Unauthorized Access Detected");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.Close();
                progressBar1.Value = 0;
                button1.Enabled = true;
                button2.Enabled = false;
            }
            catch (UnauthorizedAccessException)
            {
                Debug.WriteLine("Unauthorized Access Detected");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        //RDS Spy need this to register this plugin properly
    }
}
