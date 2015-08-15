using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Shapes;

namespace Lte.WinApp.Models
{
    public abstract class DataCollectionBar : DataCollection<DataSeriesBar>
    {
        public abstract void AddBars(ChartStyleGridLines csg);

        protected void DrawVerticalBar(Point point, ChartStyleGridLines csg, DataSeriesBar ds, double width, double y)
        {
            Polygon polygon = Polygon(ds);
            double x = point.X - 0.5*csg.XTick;

            DrawVerticalBarPolygon(polygon, point, csg, y, x, width);
        }

        protected void DrawMultiVerticalBar(Point point, ChartStyleGridLines csg, DataSeriesBar ds, int n)
        {
            Polygon polygon = Polygon(ds);
            double width = 0.7*csg.XTick;
            double w1 = width / DataList.Count;
            double w = ds.BarWidth*w1;
            double space = (w1 - w)/2;
            double x = point.X - 0.5*csg.XTick;

            DrawVerticalBarPolygon(polygon, point, csg, 0, x + space + n*w1, width, w);
        }

        private static void DrawVerticalBarPolygon(Polygon polygon, Point point, ChartStyleGridLines csg, double y, double x,
            double width, double w = 0)
        {
            polygon.Points.Add(csg.NormalizePoint(new Point(x - width/2, y)));
            polygon.Points.Add(csg.NormalizePoint(new Point(x + width/2 + w, y)));
            polygon.Points.Add(csg.NormalizePoint(new Point(x + width/2 + w, y + point.Y)));
            polygon.Points.Add(csg.NormalizePoint(new Point(x - width/2, y + point.Y)));
            csg.ChartCanvas.Children.Add(polygon);
        }

        private static Polygon Polygon(DataSeriesBar ds)
        {
            return new Polygon
            {
                Fill = ds.FillColor,
                Stroke = ds.BorderColor,
                StrokeThickness = ds.BorderThickness
            };
        }
    }

    public class VerticalBar : DataCollectionBar
    {
        public override void AddBars(ChartStyleGridLines csg)
        {
            if (DataList.Count == 1)
            {
                foreach (DataSeriesBar dataSeries in DataList)
                {
                    foreach (Point point in dataSeries.LineSeries.Points)
                        DrawVerticalBar(point, csg, dataSeries, csg.XTick * dataSeries.BarWidth, 0);
                }
            }
            else
            {
                int j = 0;
                foreach (DataSeriesBar dataSeries in DataList)
                {
                    foreach (Point point in dataSeries.LineSeries.Points)
                        DrawMultiVerticalBar(point, csg, dataSeries, j);
                    j++;
                }
            }
        }
    }

    public class VerticalOverlayBar : DataCollectionBar
    {
        public override void AddBars(ChartStyleGridLines csg)
        {
            if (DataList.Count <= 1) return;
            foreach (DataSeriesBar dataSeries in DataList)
            {
                double width = csg.XTick*dataSeries.BarWidth;
                foreach (Point point in dataSeries.LineSeries.Points)
                {
                    DrawVerticalBar(point,csg, dataSeries, width, 0);
                    width /= 2;
                }
            }
        }
    }

    public class VerticalStackBar : DataCollectionBar
    {
        public override void AddBars(ChartStyleGridLines csg)
        {
            if (DataList.Count <= 1) return;
            foreach (DataSeriesBar dataSeries in DataList)
            {
                double tempy = 0;
                double width = csg.XTick*dataSeries.BarWidth;
                foreach (Point point in dataSeries.LineSeries.Points)
                {
                    DrawVerticalBar(point, csg, dataSeries, width, tempy);
                    tempy += point.Y;
                }
            }
        }
    }
}