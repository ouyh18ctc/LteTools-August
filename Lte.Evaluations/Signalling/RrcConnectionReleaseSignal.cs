using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lte.Domain.Regular;

namespace Lte.Evaluations.Signalling
{
    public class RrcConnectionReleaseSignal
    {
        public int RrcTransactionIdentifier { get; private set; }

        public RrcConnectionReleaseCause ReleaseCause { get; private set; }

        public RrcConnectionReleaseSignal(string signalString)
        {
            RrcTransactionIdentifier = signalString.GetFieldContent(5, 2);
            ReleaseCause = (RrcConnectionReleaseCause)signalString.GetFieldContent(13, 2);
        }
    }
}
