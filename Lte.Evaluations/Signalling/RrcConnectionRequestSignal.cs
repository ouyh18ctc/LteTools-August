using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lte.Domain.Regular;

namespace Lte.Evaluations.Signalling
{
    public class RrcConnectionRequestSignal
    {
        public bool STmsiIncluded { get; private set; }

        public int MmeCode { get; private set; }

        public string STmsi { get; private set; }

        public RrcConnectionEstablishmentCause EstablishmentCause { get; private set; }

        public RrcConnectionRequestSignal(string signalString)
        {
            string firstPart = signalString.Substring(0, 3);
            STmsiIncluded = (firstPart.GetFieldContent(3) == 0);
            if (STmsiIncluded)
            {
                MmeCode = firstPart.GetFieldContent(4, 8);
                STmsi = signalString.Substring(3, 8);
            }
            else
            {
                MmeCode = -1;
                STmsi = null;
            }
            EstablishmentCause
                = (RrcConnectionEstablishmentCause)signalString.Substring(11, 1).GetFieldContent(length: 3);
        }
    }
}
