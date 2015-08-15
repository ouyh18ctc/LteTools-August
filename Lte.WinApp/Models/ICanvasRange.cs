using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Lte.WinApp.Models
{
    public interface ICanvasRange : IChartStyle
    {
        double XTick { get; set; }

        double YTick { get; set; }

        Canvas TextCanvas { get; set; }
    }

    public static class CanvasRangeOperation
    {
        public static double CalculateLeftOffset(this ICanvasRange range)
        {
            double offset = 0;

            for (double dy = range.Ymin; dy < range.Ymax; dy += range.YTick)
            {
                TextBlock tb = new TextBlock
                {
                    Text = dy.ToString(),
                    TextAlignment = TextAlignment.Right
                };
                tb.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));

                if (offset < tb.DesiredSize.Width) offset = tb.DesiredSize.Width;
            }
            return offset + 5;
        }

        public static void GenerateYGrids(this ICanvasRange range, IDataSeries series)
        {
            for (double dx = range.Xmin + range.XTick; dx < range.Xmax; dx += range.XTick)
            {
                Line gridLine = new Line
                {
                    X1 = range.NormalizePoint(new Point(dx, range.Ymin)).X,
                    Y1 = range.NormalizePoint(new Point(dx, range.Ymin)).Y,
                    X2 = range.NormalizePoint(new Point(dx, range.Ymax)).X,
                    Y2 = range.NormalizePoint(new Point(dx, range.Ymax)).Y
                };
                series.AddLinePattern(gridLine);
                range.ChartCanvas.Children.Add(gridLine);
            }
        }

        public static void GenerateXGrids(this ICanvasRange range, IDataSeries series)
        {
            for (double dy = range.Ymin + range.YTick; dy < range.Ymax; dy += range.YTick)
            {
                Line gridLine = new Line
                {
                    X1 = range.NormalizePoint(new Point(range.Xmin, dy)).X,
                    Y1 = range.NormalizePoint(new Point(range.Xmin, dy)).Y,
                    X2 = range.NormalizePoint(new Point(range.Xmax, dy)).X,
                    Y2 = range.NormalizePoint(new Point(range.Xmax, dy)).Y
                };
                series.AddLinePattern(gridLine);
                range.ChartCanvas.Children.Add(gridLine);
            }
        }

        public static void GenerateXLabels(this ICanvasRange range, double leftOffset)
        {
            for (double dx = range.Xmin; dx <= range.Xmax; dx += range.XTick)
            {
                Point pt = range.NormalizePoint(new Point(dx, range.Ymin));
                Line tick = new Line
                {
                    Stroke = Brushes.Black,
                    X1 = pt.X,
                    Y1 = pt.Y,
                    X2 = pt.X,
                    Y2 = pt.Y - 5
                };
                range.ChartCanvas.Children.Add(tick);

                TextBlock tb = new TextBlock { Text = dx.ToString() };
                tb.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                range.TextCanvas.Children.Add(tb);
                Canvas.SetLeft(tb, leftOffset + pt.X - tb.DesiredSize.Width / 2);
                Canvas.SetTop(tb, pt.Y + 2 + tb.DesiredSize.Height / 2);
            }
        }

        public static void GenerateYLabels(this ICanvasRange range)
        {
            for (double dy = range.Ymin; dy <= range.Ymax; dy += range.YTick)
            {
                Point pt = range.NormalizePoint(new Point(range.Xmin, dy));
                Line tick = new Line
                {
                    Stroke = Brushes.Black,
                    X1 = pt.X,
                    Y1 = pt.Y,
                    X2 = pt.X + 5,
                    Y2 = pt.Y
                };
                range.ChartCanvas.Children.Add(tick);

                TextBlock tb = new TextBlock { Text = dy.ToString() };
                tb.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                range.TextCanvas.Children.Add(tb);
                Canvas.SetRight(tb, range.ChartCanvas.Width + 10);
                Canvas.SetTop(tb, pt.Y);
            }
        }
    }

}