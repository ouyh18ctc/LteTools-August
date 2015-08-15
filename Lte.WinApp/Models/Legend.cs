using System.Windows.Controls;
using System.Windows.Shapes;

namespace Lte.WinApp.Models
{
    public abstract class Legend
    {
        public Canvas Canvas { get; set; }

        public bool IsLegend { get; set; }

        public bool IsBorder { get; set; }

        protected Legend()
        {
            IsLegend = false;
            IsBorder = true;
        }

        protected Legend(Legend model)
        {
            Canvas = model.Canvas;
            IsLegend = model.IsLegend;
            IsBorder = model.IsBorder;
        }

        protected abstract void SetCanvasPosition(Canvas canvas, Rectangle legendRect, double offset = 7);

        public void AddLegend(Canvas canvas, DataCollection<DataSeries> dc)
        {
            if (dc.DataList.Count == 0 || !IsLegend)
            {
                return;
            }

            const double textHeight = 17;
            const double lineLength = 34;
            double legendWidth = dc.CalculateLegendWidth();
            double legendHeight = dc.CalculateLegendHeight(textHeight);
            Rectangle legendRect = Canvas.GenerateLegendRect(legendWidth+lineLength+20, legendHeight+5);
            if (IsLegend && IsBorder)
            {
                Canvas.Children.Add(legendRect);
            }
            Canvas.GenerateLegendLines(dc.DataList, textHeight, lineLength);
            SetCanvasPosition(canvas, legendRect);
        }
    }

    public class EastLegend : Legend
    {
        protected override void SetCanvasPosition(Canvas canvas, Rectangle legendRect, double offset = 7)
        {
            Canvas.SetTop(Canvas, canvas.Height/2 - legendRect.Height/2);
            Canvas.SetRight(Canvas, offset);
        }

        public EastLegend()
        {
        }

        public EastLegend(Legend model) : base(model)
        {
        }
    }

    public class NorthEastLegend : Legend
    {
        protected override void SetCanvasPosition(Canvas canvas, Rectangle legendRect, double offset = 7)
        {
            Canvas.SetTop(Canvas, offset);
            Canvas.SetRight(Canvas, offset);
        }

        public NorthEastLegend()
        {
        }

        public NorthEastLegend(Legend model) : base(model)
        {
        }
    }

    public class NorthLegend : Legend
    {
        protected override void SetCanvasPosition(Canvas canvas, Rectangle legendRect, double offset = 7)
        {
            Canvas.SetTop(Canvas, offset);
            Canvas.SetLeft(Canvas, canvas.Width/2 - legendRect.Width/2);
        }

        public NorthLegend()
        {
        }

        public NorthLegend(Legend model) : base(model)
        {
        }
    }

    public class NorthWestLegend : Legend
    {
        protected override void SetCanvasPosition(Canvas canvas, Rectangle legendRect, double offset = 7)
        {
            Canvas.SetTop(Canvas, offset);
            Canvas.SetLeft(Canvas, offset);
        }

        public NorthWestLegend()
        {
        }

        public NorthWestLegend(Legend model) : base(model)
        {
        }
    }

    public class WestLegend : Legend
    {
        protected override void SetCanvasPosition(Canvas canvas, Rectangle legendRect, double offset = 7)
        {
            Canvas.SetTop(Canvas, canvas.Height / 2 - legendRect.Height / 2);
            Canvas.SetLeft(Canvas, offset);
        }

        public WestLegend()
        {
        }

        public WestLegend(Legend model) : base(model)
        {
        }
    }

    public class SouthWestLegend : Legend
    {
        protected override void SetCanvasPosition(Canvas canvas, Rectangle legendRect, double offset = 7)
        {
            Canvas.SetBottom(Canvas, offset);
            Canvas.SetLeft(Canvas, offset);
        }

        public SouthWestLegend()
        {
        }

        public SouthWestLegend(Legend model) : base(model)
        {
        }
    }

    public class SouthLegend : Legend
    {
        protected override void SetCanvasPosition(Canvas canvas, Rectangle legendRect, double offset = 7)
        {
            Canvas.SetBottom(Canvas, offset);
            Canvas.SetLeft(Canvas, canvas.Width / 2 - legendRect.Width / 2);
        }

        public SouthLegend()
        {
        }

        public SouthLegend(Legend model) : base(model)
        {
        }
    }

    public class SouthEastLegend : Legend
    {
        protected override void SetCanvasPosition(Canvas canvas, Rectangle legendRect, double offset = 7)
        {
            Canvas.SetBottom(Canvas, offset);
            Canvas.SetRight(Canvas, offset);
        }

        public SouthEastLegend()
        {
        }

        public SouthEastLegend(Legend model) : base(model)
        {
        }
    }
}