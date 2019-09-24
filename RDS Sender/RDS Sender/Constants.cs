using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS_Sender
{
    class Constants
    {
        public static string IP = "EMPTY";
        public static int Port = 8888;

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
    }
}
