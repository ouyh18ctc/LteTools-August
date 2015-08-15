using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;

namespace Lte.WinApp.Models
{
    public class ChartStyle
    {
        public Canvas ChartCanvas { get; set; }

        public double Xmin { get; set; }

        public double Xmax { get; set; }

        public double Ymin { get; set; }

        public double Ymax { get; set; }

        public ChartStyle()
        {
            Xmin = 0;
            Xmax = 10;
            Ymin = 0;
            Ymax = 10;
        }
    }

    public class ChartStyleGridLines : ChartStyle, IDataSeries, ICanvasRange
    {
        public string Title { get; set; }

        public string XLabel { get; set; }

        public string YLabel { get; set; }

        public double XTick { get; set; }

        public double YTick { get; set; }

        public Canvas TextCanvas { get; set; }

        public bool IsXGrid { get; set; }

        public bool IsYGrid { get; set; }

        public LinePattern LinePattern { get; set; }

        public double LineThickness { get; set; }

        public Brush LineColor { get; set; }

        private double leftOffset = 20;
        private const double bottomOffset = 15;
        private double rightOffset = 10;

        public ChartStyleGridLines()
        {
            IsXGrid = true;
            IsYGrid = true;
            LineColor = Brushes.LightGray;
            XTick = 1;
            YTick = 0.5;
            Title = "Title";
            XLabel = "X Axis";
            YLabel = "Y Axis";
            LineThickness = 1;
        }

        public void AddChartStyle(TextBlock tbTitle, TextBlock tbXLabel, TextBlock tbYLabel)
        {
            TextBlock tb = new TextBlock {Text = Xmax.ToString()};
            tb.Measure(new Size(Double.PositiveInfinity,Double.PositiveInfinity));
            rightOffset = tb.DesiredSize.Width / 2 + 2;
            leftOffset = this.CalculateLeftOffset();

            Canvas.SetLeft(ChartCanvas, leftOffset);
            Canvas.SetBottom(ChartCanvas,bottomOffset);
            ChartCanvas.Width = Math.Abs(TextCanvas.Width - leftOffset - rightOffset);
            ChartCanvas.Height = Math.Abs(TextCanvas.Height - bottomOffset - tb.DesiredSize.Height / 2);
            Rectangle chartRectangle = new Rectangle
            {
                Stroke = Brushes.Black,
                Width = ChartCanvas.Width,
                Height = ChartCanvas.Height
            };
            ChartCanvas.Children.Add(chartRectangle);

            if (IsYGrid)
            {
                this.GenerateYGrids(this);
            }

            if (IsXGrid)
            {
                this.GenerateXGrids(this);
            }

            this.GenerateXLabels(leftOffset);
            this.GenerateYLabels();

            tbTitle.Text = Title;
            tbXLabel.Text = XLabel;
            tbYLabel.Text = YLabel;
            tbXLabel.Margin = new Thickness(leftOffset + 2, 2, 2, 2);
            tbTitle.Margin = new Thickness(leftOffset + 2, 2, 2, 2);
        }

    }
}
