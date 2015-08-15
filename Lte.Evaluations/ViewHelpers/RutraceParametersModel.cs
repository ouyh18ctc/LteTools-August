using Lte.Evaluations.Abstract;
using Lte.Evaluations.Rutrace.Record;
using Lte.Parameters.Entities;

namespace Lte.Evaluations.ViewHelpers
{
    public class RutraceParametersModel : IRutraceImportParameters
    {
        public double InterferenceThreshold { get; set; }

        public double RatioThreshold { get; set; }

        public double RtdExcessThreshold { get; set; }

        public double TaLowerBound { get; set; }

        public double TaUpperRatio { get; set; }

        public void ReadData()
        {
            InterferenceThreshold = RuInterferenceRecord.InterferenceThreshold;
            RatioThreshold = (int)(RuInterferenceStat.RatioThreshold * 100);
            RtdExcessThreshold = CdrTaRecord.ExcessThreshold;
            TaLowerBound = InterferenceStat.LowerBound;
            TaUpperRatio = (int)(InterferenceStat.UpperBound / InterferenceStat.LowerBound * 10) / 10;
        }

        public void WriteData()
        {
            RuInterferenceRecord.InterferenceThreshold = InterferenceThreshold;
            RuInterferenceStat.RatioThreshold = RatioThreshold * 0.01;
            CdrTaRecord.ExcessThreshold = RtdExcessThreshold;
            InterferenceStat.LowerBound = TaLowerBound;
            InterferenceStat.UpperBound = InterferenceStat.LowerBound * TaUpperRatio;
        }

        public void ResetDefaultValues()
        {
            RuInterferenceRecord.ResetDefault();
            RuInterferenceStat.ResetDefault();
            CdrTaRecord.ResetDefault();
            InterferenceStat.ResetDefault();
        }
    }
}