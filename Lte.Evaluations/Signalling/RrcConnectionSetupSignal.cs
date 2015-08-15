using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lte.Domain.Regular;

namespace Lte.Evaluations.Signalling
{
    public class RrcConnectionSetupSignal
    {
        public int RrcTransactionIdentifier { get; private set; }

        public RadioResourceConfigDedicated RadioResourceConfigDedicated { get; private set; }

        public RrcConnectionSetupSignal(string signalString)
        {
            RrcTransactionIdentifier = signalString.Substring(0, 2).GetFieldContent(3, 2);
            RadioResourceConfigDedicated = new RadioResourceConfigDedicated(
                signalString.Substring(2, signalString.Length - 2));
        }
    }
}
