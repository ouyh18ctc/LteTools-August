using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Lte.WinApp.Models
{
    public abstract class Symbols
    {
        public double BorderThickness { get; set; }

        public Brush BorderColor { get; set; }

        public Brush FillColor { get; set; }

        public double SymbolSize { get; set; }

        protected Ellipse Ellipse { get; set; }

        protected Polygon Plg { get; set; }

        protected Symbols()
        {
            SymbolSize = 8.0;
            BorderColor = Brushes.Black;
            FillColor = Brushes.Black;
            BorderThickness = 1.0;
        }

        public void AddSymbol(Canvas canvas, Point pt)
        {
            Plg = new Polygon
            {
                Stroke = BorderColor,
                StrokeThickness = BorderThickness
            };
            Ellipse = new Ellipse
            {
                Stroke = BorderColor,
                StrokeThickness = BorderThickness
            };
            Panel.SetZIndex(Plg,5);
            Panel.SetZIndex(Ellipse,5);
            AddPolygon(canvas, pt);
        }

        protected abstract void AddPolygon(Canvas canvas, Point pt);
    }

    public class NoneSymbols : Symbols
    {
        protected override void AddPolygon(Canvas canvas, Point pt)
        {
        }
    }

    public class SquareSymbols : Symbols
    {
        protected override void AddPolygon(Canvas canvas, Point pt)
        {
            double halfSize = 0.5*SymbolSize;
            Plg.Fill = Brushes.White;
            Plg.Points.Add(new Point(pt.X - halfSize, pt.Y - halfSize));
            Plg.Points.Add(new Point(pt.X + halfSize, pt.Y - halfSize));
            Plg.Points.Add(new Point(pt.X + halfSize, pt.Y + halfSize));
            Plg.Points.Add(new Point(pt.X - halfSize, pt.Y + halfSize));
            canvas.Children.Add(Plg);
        }
    }

    public class OpenDiamondSymbols : Symbols
    {
        protected override void AddPolygon(Canvas canvas, Point pt)
        {
            double halfSize = 0.5 * SymbolSize;
            Plg.Fill = Brushes.White;
            Plg.Points.Add(new Point(pt.X - halfSize, pt.Y));
            Plg.Points.Add(new Point(pt.X, pt.Y - halfSize));
            Plg.Points.Add(new Point(pt.X + halfSize, pt.Y));
            Plg.Points.Add(new Point(pt.X, pt.Y + halfSize));
            canvas.Children.Add(Plg);
        }
    }

    public class CircleSymbols : Symbols
    {
        protected override void AddPolygon(Canvas canvas, Point pt)
        {
            double halfSize = 0.5 * SymbolSize;
            Ellipse.Fill = Brushes.White;
            Ellipse.Width = SymbolSize;
            Ellipse.Height = SymbolSize;
            Canvas.SetLeft(Ellipse,pt.X-halfSize);
            Canvas.SetTop(Ellipse,pt.Y-halfSize);
            canvas.Children.Add(Ellipse);
        }
    }

    public class TriangleSymbols : Symbols
    {
        protected override void AddPolygon(Canvas canvas, Point pt)
        {
            double halfSize = 0.5 * SymbolSize;
            Plg.Fill = FillColor;
            Plg.Points.Add(new Point(pt.X - halfSize, pt.Y + halfSize));
            Plg.Points.Add(new Point(pt.X, pt.Y - halfSize));
            Plg.Points.Add(new Point(pt.X + halfSize, pt.Y + halfSize));
            canvas.Children.Add(Plg);
        }
    }

    public class InvertedTriangleSymbols : Symbols
    {
        protected override void AddPolygon(Canvas canvas, Point pt)
        {
            double halfSize = 0.5 * SymbolSize;
            Plg.Fill = FillColor;
            Plg.Points.Add(new Point(pt.X, pt.Y + halfSize));
            Plg.Points.Add(new Point(pt.X - halfSize, pt.Y - halfSize));
            Plg.Points.Add(new Point(pt.X + halfSize, pt.Y - halfSize));
            canvas.Children.Add(Plg);
        }
    }

    public class CrossSymbols : Symbols
    {
        protected override void AddPolygon(Canvas canvas, Point pt)
        {
            double halfSize = 0.5 * SymbolSize;
            canvas.Children.Add(pt.GenerateLine(BorderColor, BorderThickness, halfSize, new[] {-1, 1, 1, -1}));
            canvas.Children.Add(pt.GenerateLine(BorderColor, BorderThickness, halfSize, new[] {-1, -1, 1, 1}));
        }
    }

    public class StarSymbols : Symbols
    {
        protected override void AddPolygon(Canvas canvas, Point pt)
        {
            double halfSize = 0.5 * SymbolSize;
            canvas.Children.Add(pt.GenerateLine(BorderColor, BorderThickness, halfSize, new[] { -1, 1, 1, -1 }));
            canvas.Children.Add(pt.GenerateLine(BorderColor, BorderThickness, halfSize, new[] { -1, -1, 1, 1 }));
            canvas.Children.Add(pt.GenerateLine(BorderColor, BorderThickness, halfSize, new[] { -1, 0, 1, 0 }));
            canvas.Children.Add(pt.GenerateLine(BorderColor, BorderThickness, halfSize, new[] { 0, -1, 0, 1 }));
        }
    }

    public class PlusSymbols : Symbols
    {
        protected override void AddPolygon(Canvas canvas, Point pt)
        {
            double halfSize = 0.5 * SymbolSize;
            canvas.Children.Add(pt.GenerateLine(BorderColor, BorderThickness, halfSize, new[] { -1, 0, 1, 0 }));
            canvas.Children.Add(pt.GenerateLine(BorderColor, BorderThickness, halfSize, new[] { 0, -1, 0, 1 }));
        }
    }

    public class DotSymbols : Symbols
    {
        protected override void AddPolygon(Canvas canvas, Point pt)
        {
            double halfSize = 0.5 * SymbolSize;
            Ellipse.Fill = FillColor;
            Ellipse.Width = SymbolSize;
            Ellipse.Height = SymbolSize;
            Canvas.SetLeft(Ellipse, pt.X - halfSize);
            Canvas.SetTop(Ellipse, pt.Y - halfSize);
            canvas.Children.Add(Ellipse);
        }
    }

    public class BoxSymbols : Symbols
    {
        protected override void AddPolygon(Canvas canvas, Point pt)
        {
            double halfSize = 0.5 * SymbolSize;
            Plg.Fill = FillColor;
            Plg.Points.Add(new Point(pt.X - halfSize, pt.Y - halfSize));
            Plg.Points.Add(new Point(pt.X + halfSize, pt.Y - halfSize));
            Plg.Points.Add(new Point(pt.X + halfSize, pt.Y + halfSize));
            Plg.Points.Add(new Point(pt.X - halfSize, pt.Y + halfSize));
            canvas.Children.Add(Plg);
        }
    }

    public class DiamondSymbols : Symbols
    {
        protected override void AddPolygon(Canvas canvas, Point pt)
        {
            double halfSize = 0.5 * SymbolSize;
            Plg.Fill = FillColor;
            Plg.Points.Add(new Point(pt.X - halfSize, pt.Y));
            Plg.Points.Add(new Point(pt.X, pt.Y - halfSize));
            Plg.Points.Add(new Point(pt.X + halfSize, pt.Y));
            Plg.Points.Add(new Point(pt.X, pt.Y + halfSize));
            canvas.Children.Add(Plg);
        }
    }

}
