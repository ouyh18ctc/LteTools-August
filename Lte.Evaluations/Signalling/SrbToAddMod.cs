using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lte.Domain.Regular;

namespace Lte.Evaluations.Signalling
{
    public class SrbToAddMod
    {
        public bool RlcConfigPresent { get; private set; }
        public bool LogicalChannelConfigPresent { get; private set; }
        public int SrbIdentity { get; private set; }

        public string ImportString(string signalString)
        {
            string header = signalString.Substring(0, 2);
            RlcConfigPresent = (header.GetFieldContent(3) == 1);
            LogicalChannelConfigPresent = (header.GetFieldContent(4) == 1);
            SrbIdentity = header.GetFieldContent(5) + 1;
            signalString = signalString.Substring(1);
            return signalString;
        }
    }
}
