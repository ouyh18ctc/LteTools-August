using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Lte.WinApp.Models
{
    public static class CanvasOperations
    {
        public static Rectangle GenerateLegendRect(this Canvas canvas, double legendWidth, double legendHeight)
        {
            canvas.Width = legendWidth + 55;
            Rectangle rect = new Rectangle
            {
                Stroke = Brushes.Black,
                Fill = Brushes.White,
                Width = legendWidth,
                Height = legendHeight
            };
            Panel.SetZIndex(canvas, 10);
            canvas.Width = rect.Width;
            canvas.Height = rect.Height;
            return rect;
        }

        public static void GenerateLegendLines(this Canvas canvas, IEnumerable<DataSeries> dataList, double textHeight, double lineLength)
        {
            const double sx = 6;
            const double sy = 0;
            int n = 1;
            double xText = 2 * sx + lineLength;
            foreach (DataSeries ds in dataList)
            {
                double yText = n * sy + (2 * n - 1) * textHeight / 2;
                Line line = new Line
                {
                    X1 = sx,
                    Y1 = yText,
                    X2 = sx + lineLength,
                    Y2 = yText
                };
                ds.AddLinePattern(line);
                canvas.Children.Add(line);
                ds.Symbols.AddSymbol(canvas, new Point(0.5 * (line.X2 - line.X1 + ds.Symbols.SymbolSize) + 1, line.Y1));
                TextBlock tb = new TextBlock { Text = ds.SeriesName };
                canvas.Children.Add(tb);
                Canvas.SetTop(tb, yText - textHeight / 2);
                Canvas.SetLeft(tb, xText);
                n++;
            }
        }
    }
}