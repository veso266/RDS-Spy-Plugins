using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace RDS_Sender
{
    class UDPClient
    {
        static Socket sending_socket;
        static IPAddress send_to_address;
        static IPEndPoint sending_end_point;
        public static void Configure()
        {
            sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            if (Constants.IP == "EMPTY") //Yea yea, I know its bad but I don't care :)
                send_to_address = IPAddress.Parse(Tools.GetInterfaceIP()[0]);
            else
                send_to_address = IPAddress.Parse(Constants.IP);
            sending_end_point = new IPEndPoint(send_to_address, Constants.Port);
        }

        public static void SendGroups(Constants.TRDSGroup Group)
        {
            //I cannot put NO-RDS Thread to sleep from here so need to improvise
            //Format data according to ASCII G Protocol, see https://github.com/veso266/SDRSharp.XDR/blob/master/Docs/ASCII%20G%20Protocol.md

            string data;

            data = "G:";
            data += "\r\n"; //CLRF
            data += Group.Blk1.ToString("X4") + Group.Blk2.ToString("X4") + Group.Blk3.ToString("X4") + Group.Blk4.ToString("X4"); //RDS Groups: AAABBBCCCDDD
            data += "\r\n"; //CLRF
            data += "\r\n"; //CLRF

            byte[] send_buffer = Encoding.ASCII.GetBytes(data);
            try
            {
                sending_socket.SendTo(send_buffer, sending_end_point);
            }
            catch (Exception send_exception)
            {
                System.Windows.Forms.MessageBox.Show(send_exception.Message);
            }
        }
        public static void SendDataLoop(string data, string lineBreak)
        {
            while (true)
            {
                Thread.Sleep(1000); //Because I cannot pause this thread when RDS is recieved I have to improvise, BUG: RDS indicator will blink once a second in XDR-GTK
                byte[] send_buffer = Encoding.ASCII.GetBytes(data + lineBreak);
                try
                {
                    sending_socket.SendTo(send_buffer, sending_end_point);
                }
                catch (Exception send_exception)
                {
                    System.Windows.Forms.MessageBox.Show(send_exception.Message);
                }
            }
        }
    }
}
