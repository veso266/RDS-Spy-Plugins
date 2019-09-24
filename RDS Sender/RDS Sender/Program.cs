using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

using RGiesecke.DllExport;

namespace RDS_Sender
{
    class Program
    {
        static int PI;
        static Constants.TRDSGroup Group;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [DllExport("PluginName", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        static string PluginName()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            UDPClient.Configure();
            Thread th = new Thread(SendNORDS);
            th.Start();
            th.IsBackground = true; //We have to do this otherwise program will not quit
            return "RDS Sender ";
        }
        [DllExport("RDSGroup", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        unsafe static void RDSGroup(Constants.TRDSGroup* PRDSGroup)
        {
            Group = *PRDSGroup;
            UDPClient.SendGroups(Group);
            if (Group.Blk1 >= 0)
            {
                if (PI != Group.Blk1)
                {
                    PI = Group.Blk1;
                    //System.Windows.Forms.MessageBox.Show("New PI has been detected:" + PI.ToString("X4"));
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
                Application.Run(new Form1());

            }
            if (w == "RESETDATA")
            {
                PI = -1;
            }
        }

        static void SendNORDS()
        {
            UDPClient.SendDataLoop("NO-RDS", "\r\n");
        }
    }
}
