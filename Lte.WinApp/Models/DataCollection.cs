using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Lte.WinApp.Models
{
    public class DataCollection<T>
        where T : DataSeries
    {
        public List<T> DataList { get; set; }

        public DataCollection()
        {
            DataList = new List<T>();
        }

        public void AddLines(IChartStyle cs)
        {
            int j = 0;
            foreach (T ds in DataList)
            {
                if (ds.SeriesName == "Default Name")
                    ds.SeriesName = "DataSeries" + j;
                ds.AddLinePattern(ds.LineSeries);
                for (int i = 0; i < ds.LineSeries.Points.Count; i++)
                {
                    ds.LineSeries.Points[i] =
                        cs.NormalizePoint(ds.LineSeries.Points[i]);
                }
                cs.ChartCanvas.Children.Add(ds.LineSeries);
                j++;
            }
        }

        private IEnumerable<string> GetLegendLabels()
        {
            int n = 0;
            string[] legendLabels = new string[DataList.Count];
            foreach (T ds in DataList)
            {
                legendLabels[n] = ds.SeriesName;
                n++;
            }
            return legendLabels;
        }

        public double CalculateLegendWidth()
        {
            IEnumerable<string> legendLabels = GetLegendLabels();

            double legendWidth = 0;
            foreach (TextBlock tb in legendLabels.Select(label => new TextBlock
            {
                Text = label
            }))
            {
                tb.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                if (legendWidth < tb.DesiredSize.Width)
                    legendWidth = tb.DesiredSize.Width;
            }
            return legendWidth;
        }

        public double CalculateLegendHeight(double textHeight)
        {
            return textHeight*DataList.Count;
        }
    }
}