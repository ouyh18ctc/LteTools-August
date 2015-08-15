using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lte.WinApp.Models;

namespace Lte.WinApp.Controls
{
    /// <summary>
    /// LineChartControlLib.xaml 的交互逻辑
    /// </summary>
    public partial class LineChartControlLib : UserControl
    {
        public LineChartControlLib()
        {
            InitializeComponent();
            ChartStyle = new ChartStyleGridLines();
            DataCollection = new DataCollection<DataSeries>();
            DataSeries = new DataSeries();
            ChartStyle.TextCanvas = TextCanvas;
            ChartStyle.ChartCanvas = ChartCanvas;
            Legend = new NorthEastLegend {Canvas = LegendCanvas};
        }

        private void ChartGrid_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            TextCanvas.Width = chartGrid.ActualWidth;
            TextCanvas.Height = chartGrid.ActualHeight;
            LegendCanvas.Children.Clear();
            ChartCanvas.Children.RemoveRange(1, ChartCanvas.Children.Count - 1);
            TextCanvas.Children.RemoveRange(1, TextCanvas.Children.Count - 1);
            AddChart();
        }

        private void AddChart()
        {
            ChartStyle.AddChartStyle(tbTitle, tbXLabel, tbYLabel);
            if (DataCollection.DataList.Count == 0) return;
            DataCollection.AddLines(ChartStyle);
            Legend.AddLegend(ChartCanvas, DataCollection);
        }

        public ChartStyleGridLines ChartStyle { get; set; }

        public DataCollection<DataSeries> DataCollection { get; set; }

        public DataSeries DataSeries { get; set; }

        public Legend Legend { get; set; }
        
        public double Xmin
        {
            get { return (double)GetValue(XminProperty); }
            set
            {
                SetValue(XminProperty, value);
                ChartStyle.Xmin = value;
            }
        }

        // Using a DependencyProperty as the backing store for Xmin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XminProperty =
            DependencyProperty.Register("Xmin", typeof(double), typeof(LineChartControlLib), 
            new FrameworkPropertyMetadata(0.0, OnPropetyChanged));

        
        public double Xmax
        {
            get { return (double)GetValue(XmaxProperty); }
            set
            {
                SetValue(XmaxProperty, value);
                ChartStyle.Xmax = value;
            }
        }

        // Using a DependencyProperty as the backing store for Xmax.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XmaxProperty =
            DependencyProperty.Register("Xmax", typeof(double), typeof(LineChartControlLib), 
            new FrameworkPropertyMetadata(10.0, OnPropetyChanged));


        public double Ymin
        {
            get { return (double)GetValue(YminProperty); }
            set
            {
                SetValue(YminProperty, value);
                ChartStyle.Ymin = value;
            }
        }

        // Using a DependencyProperty as the backing store for Ymin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YminProperty =
            DependencyProperty.Register("Ymin", typeof(double), typeof(LineChartControlLib), 
            new FrameworkPropertyMetadata(0.0, OnPropetyChanged));


        public double Ymax
        {
            get { return (double)GetValue(YmaxProperty); }
            set
            {
                SetValue(YmaxProperty, value);
                ChartStyle.Ymax = value;
            }
        }

        // Using a DependencyProperty as the backing store for Ymax.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YmaxProperty =
            DependencyProperty.Register("Ymax", typeof(double), typeof(LineChartControlLib), 
            new FrameworkPropertyMetadata(10.0, OnPropetyChanged));
        

        public double XTick
        {
            get { return (double)GetValue(XTickProperty); }
            set
            {
                SetValue(XTickProperty, value);
                ChartStyle.XTick = value;
            }
        }

        // Using a DependencyProperty as the backing store for XTick.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XTickProperty =
            DependencyProperty.Register("XTick", typeof(double), typeof(LineChartControlLib), 
            new FrameworkPropertyMetadata(2.0, OnPropetyChanged));


        public double YTick
        {
            get { return (double)GetValue(YTickProperty); }
            set
            {
                SetValue(YTickProperty, value);
                ChartStyle.YTick = value;
            }
        }

        // Using a DependencyProperty as the backing store for YTick.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YTickProperty =
            DependencyProperty.Register("YTick", typeof(double), typeof(LineChartControlLib), 
            new FrameworkPropertyMetadata(2.0, OnPropetyChanged));


        public string XLabel
        {
            get { return (string)GetValue(XLabelProperty); }
            set
            {
                SetValue(XLabelProperty, value);
                ChartStyle.XLabel = value;
            }
        }

        // Using a DependencyProperty as the backing store for XLabel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XLabelProperty =
            DependencyProperty.Register("XLabel", typeof(string), typeof(LineChartControlLib), 
            new FrameworkPropertyMetadata("X Axis", OnPropetyChanged));


        public string YLabel
        {
            get { return (string)GetValue(YLabelProperty); }
            set
            {
                SetValue(YLabelProperty, value);
                ChartStyle.YLabel = value;
            }
        }

        // Using a DependencyProperty as the backing store for YLabel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YLabelProperty =
            DependencyProperty.Register("YLabel", typeof(string), typeof(LineChartControlLib), 
            new FrameworkPropertyMetadata("Y Axis", OnPropetyChanged));


        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set
            {
                SetValue(TitleProperty, value);
                ChartStyle.Title = value;
            }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(LineChartControlLib), 
            new FrameworkPropertyMetadata("Title", OnPropetyChanged));


        public bool IsXGrid
        {
            get { return (bool)GetValue(IsXGridProperty); }
            set
            {
                SetValue(IsXGridProperty, value);
                ChartStyle.IsXGrid = value;
            }
        }

        // Using a DependencyProperty as the backing store for IsXGrid.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsXGridProperty =
            DependencyProperty.Register("IsXGrid", typeof(bool), typeof(LineChartControlLib), 
            new FrameworkPropertyMetadata(true, OnPropetyChanged));


        public bool IsYGrid
        {
            get { return (bool)GetValue(IsYGridProperty); }
            set
            {
                SetValue(IsYGridProperty, value);
                ChartStyle.IsYGrid = value;
            }
        }

        // Using a DependencyProperty as the backing store for IsYGrid.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsYGridProperty =
            DependencyProperty.Register("IsYGrid", typeof(bool), typeof(LineChartControlLib), 
            new FrameworkPropertyMetadata(true, OnPropetyChanged));


        public Brush GridLineColor
        {
            get { return (Brush)GetValue(GridLineColorProperty); }
            set
            {
                SetValue(GridLineColorProperty, value);
                ChartStyle.LineColor = value;
            }
        }

        // Using a DependencyProperty as the backing store for GridLineColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GridLineColorProperty =
            DependencyProperty.Register("GridLineColor", typeof(Brush), typeof(LineChartControlLib), 
            new FrameworkPropertyMetadata(Brushes.Gray, OnPropetyChanged));


        public LinePattern LinePattern
        {
            get { return (LinePattern)GetValue(LinePatternProperty); }
            set
            {
                SetValue(LinePatternProperty, value);
                ChartStyle.LinePattern = value;
            }
        }

        // Using a DependencyProperty as the backing store for LinePattern.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LinePatternProperty =
            DependencyProperty.Register("LinePattern", typeof(LinePattern), typeof(LineChartControlLib), 
            new FrameworkPropertyMetadata(LinePattern.Solid, OnPropetyChanged));


        public bool IsLegend
        {
            get { return (bool)GetValue(IsLegendProperty); }
            set
            {
                SetValue(IsLegendProperty, value);
                Legend.IsLegend = value;
            }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLegendProperty =
            DependencyProperty.Register("IsLegend", typeof(bool), typeof(LineChartControlLib), 
            new FrameworkPropertyMetadata(false, OnPropetyChanged));


        public LegendPosition LegendPosition
        {
            get { return (LegendPosition)GetValue(LegendPositionProperty); }
            set
            {
                SetValue(LegendPositionProperty, value);
                switch (value)
                {
                    case LegendPosition.East:
                        Legend = new EastLegend(Legend);
                        break;
                    case LegendPosition.North:
                        Legend = new NorthLegend(Legend);
                        break;
                    case LegendPosition.NorthEast:
                        Legend = new NorthEastLegend(Legend);
                        break;
                    case LegendPosition.NorthWest:
                        Legend = new NorthWestLegend(Legend);
                        break;
                    case LegendPosition.South:
                        Legend = new SouthLegend(Legend);
                        break;
                    case LegendPosition.SouthEast:
                        Legend = new SouthEastLegend(Legend);
                        break;
                    case LegendPosition.SouthWest:
                        Legend = new SouthWestLegend(Legend);
                        break;
                    default:
                        Legend = new WestLegend(Legend);
                        break;
                }
            }
        }

        // Using a DependencyProperty as the backing store for LegendPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LegendPositionProperty =
            DependencyProperty.Register("LegendPosition", typeof(LegendPosition), typeof(LineChartControlLib), 
            new FrameworkPropertyMetadata(LegendPosition.NorthEast, OnPropetyChanged));
        
        
        private static void OnPropetyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            LineChartControlLib lib = sender as LineChartControlLib;
            if (lib == null) return;
            if (e.Property == XminProperty)
                lib.Xmin = (double) e.NewValue;
            else if (e.Property == XmaxProperty)
                lib.Xmax = (double) e.NewValue;
            else if (e.Property == YminProperty)
                lib.Ymin = (double) e.NewValue;
            else if (e.Property == YmaxProperty)
                lib.Ymax = (double) e.NewValue;
            else if (e.Property == XTickProperty)
                lib.XTick = (double) e.NewValue;
            else if (e.Property == YTickProperty)
                lib.YTick = (double) e.NewValue;
            else if (e.Property == LinePatternProperty)
                lib.LinePattern = (LinePattern) e.NewValue;
            else if (e.Property == GridLineColorProperty)
                lib.GridLineColor = (Brush) e.NewValue;
            else if (e.Property == TitleProperty)
                lib.Title = (string) e.NewValue;
            else if (e.Property == XLabelProperty)
                lib.XLabel = (string) e.NewValue;
            else if (e.Property == YLabelProperty)
                lib.YLabel = (string) e.NewValue;
            else if (e.Property == IsXGridProperty)
                lib.IsXGrid = (bool) e.NewValue;
            else if (e.Property == IsYGridProperty)
                lib.IsYGrid = (bool) e.NewValue;
            else if (e.Property == IsLegendProperty)
                lib.IsLegend = (bool) e.NewValue;
            else if (e.Property == LegendPositionProperty)
                lib.LegendPosition = (LegendPosition) e.NewValue;
        }
    }
}
