using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lte.Domain.Regular;

namespace Lte.Evaluations.Signalling
{
    public class RadioResourceConfigDedicated
    {
        public bool SrbToAddModListPresent { get; private set; }

        public bool DrbToAddModListPresent { get; private set; }

        public bool DrbToReleaseListPresent { get; private set; }

        public bool MacMainConfigPresent { get; private set; }

        public bool SpsConfigPresent { get; private set; }

        public bool PhysicalConfigDedicatedPresent { get; private set; }

        public int SrbToAddModListLength { get; private set; }

        public SrbToAddMod[] SrbToAddModList { get; private set; }

        public RadioResourceConfigDedicated(string signalString)
        {
            string header = signalString.Substring(0, 3);
            SrbToAddModListPresent = (header.GetFieldContent(3) == 1);
            DrbToAddModListPresent = (header.GetFieldContent(4) == 1);
            DrbToReleaseListPresent = (header.GetFieldContent(5) == 1);
            MacMainConfigPresent = (header.GetFieldContent(6) == 1);
            SpsConfigPresent = (header.GetFieldContent(7) == 1);
            PhysicalConfigDedicatedPresent = (header.GetFieldContent(8) == 1);

            signalString = signalString.Substring(2);
            if (SrbToAddModListPresent)
            {
                SrbToAddModListLength = header.GetFieldContent(10) + 1;
                SrbToAddModList = new SrbToAddMod[SrbToAddModListLength];
                
                for (int i = 0; i < SrbToAddModListLength; i++)
                {
                    SrbToAddModList[i] = new SrbToAddMod();
                    signalString = SrbToAddModList[i].ImportString(signalString); 
                }
            }
        }
    }
}
