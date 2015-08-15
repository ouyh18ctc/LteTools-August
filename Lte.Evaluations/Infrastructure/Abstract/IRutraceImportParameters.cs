namespace Lte.Evaluations.Abstract
{
    public interface IRutraceImportParameters
    {
        double InterferenceThreshold { get; set; }

        double RatioThreshold { get; set; }

        double RtdExcessThreshold { get; set; }

        double TaLowerBound { get; set; }

        double TaUpperRatio { get; set; }

        void ReadData();

        void WriteData();
    }
}
