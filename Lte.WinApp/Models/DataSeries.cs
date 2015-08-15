using System.Windows.Media;
using System.Windows.Shapes;

namespace Lte.WinApp.Models
{
    public class DataSeries : IDataSeries
    {
        public Brush LineColor { get; set; }

        public Polyline LineSeries { get; set; }

        public double LineThickness { get; set; }

        public LinePattern LinePattern { get; set; }

        public string SeriesName { get; set; }

        public Symbols Symbols { get; set; }

        public DataSeries()
        {
            LineSeries = new Polyline();
            LineThickness = 1;
            SeriesName = "Default Name";
            LineColor = Brushes.Black;
            Symbols = new NoneSymbols();
        }
    }

    public class DataSeriesBar : DataSeries
    {
        public Brush FillColor { get; set; }

        public Brush BorderColor { get; set; }

        public double BorderThickness { get; set; }

        public double BarWidth { get; set; }

        public DataSeriesBar()
        {
            FillColor = Brushes.Black;
            BorderColor = Brushes.Black;
            BorderThickness = 1.0;
            BarWidth = 0.8;
        }
    }
}