using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.Domain.TypeDefs;

namespace Lte.Domain.Test.Ipv4
{
    public class StubIpAddress : IIpAddress
    {
        byte ipByte1 = 0;

        public byte IpByte1
        {
            get { return ipByte1; }
            set { ipByte1 = value; }
        }

        byte ipByte2 = 0;

        public byte IpByte2
        {
            get { return ipByte2; }
            set { ipByte2 = value; }
        }

        byte ipByte3 = 0;

        public byte IpByte3
        {
            get { return ipByte3; }
            set { ipByte3 = value; }
        }

        byte ipByte4 = 0;

        public byte IpByte4
        {
            get { return ipByte4; }
            set { ipByte4 = value; }
        }

    }
}
