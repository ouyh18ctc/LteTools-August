using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Lte.Evaluations.ViewHelpers;
using Lte.Parameters.Abstract;
using Lte.Parameters.Concrete;
using Lte.Parameters.Entities;
using Lte.WinApp.Controls;
using Lte.WinApp.Models;
using Lte.WinApp.Service;

namespace Lte.WinApp.ViewPages
{
    /// <summary>
    /// RutraceCdrImportPage.xaml 的交互逻辑
    /// </summary>
    public partial class RutraceCdrImportPage : Page
    {
        private readonly List<ImportedFileInfo> _fileInfoList = new List<ImportedFileInfo>();
        private readonly List<InterferenceStat> _statList = new List<InterferenceStat>();
        private readonly IFileInfoListImporter _ruImporter;
        private readonly IFileInfoListImporter _cdrImporter;
        private IFileInfoListImporter _mrImporter;
        private readonly RutraceParametersModel _parameters = new RutraceParametersModel();
        private IInterferenceStatRepository _repository;
        private ILteNeighborCellRepository _neighborRepository;

        public RutraceCdrImportPage()
        {
            InitializeComponent();
            PageTitle.Content = Title;
            _ruImporter = new RuFileInfoListImporter(_statList) { FileInfoList = _fileInfoList };
            _cdrImporter = new CdrFileInfoListImporter(_statList) { FileInfoList = _fileInfoList };

            _parameters.ReadData();
            ParametersSetting.DataContext = _parameters;
        }

        private void OpenRu_Click(object sender, RoutedEventArgs e)
        {
            FileDialogWrapper wrapper = new OpenRuFileDialogWrapper();
            if (wrapper.ShowDialog())
            {
                _ruImporter.ImportFiles(wrapper.FileNames);
                FileList.SetDataSource(_fileInfoList);
            }
        }

        private void OpenCdr_Click(object sender, RoutedEventArgs e)
        {
            FileDialogWrapper wrapper = new OpenCdrFileDialogWrapper();
            if (wrapper.ShowDialog())
            {
                _cdrImporter.ImportFiles(wrapper.FileNames);
                FileList.SetDataSource(_fileInfoList);
            }
        }

        private void OpenMr_Click(object sender, RoutedEventArgs e)
        {
            _neighborRepository = new EFLteNeighborCellRepository();
            List<Cell> cells = (new EFCellRepository()).GetAllList();
            _mrImporter = new MrFileInfoListImporter(_statList, cells, _neighborRepository)
            {
                FileInfoList = _fileInfoList
            };

            FileDialogWrapper wrapper = new OpenMrFileDialogWrapper();
            if (wrapper.ShowDialog())
            {
                _mrImporter.ImportFiles(wrapper.FileNames);
                FileList.SetDataSource(_fileInfoList);
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            _repository = new EFInterferenceStatRepository();
            _ruImporter.ImportRu();
            _cdrImporter.ImportCdr();
            _mrImporter.ImportCdr();

            if (saveDb.IsChecked == true)
            {
                if (_statList.Count == 0)
                    MessageBox.Show("\n没有需要导入数据库的RUTRACE和CDR信息");
                _repository.Save(_statList);
                MessageBox.Show("\n导入数据库记录" + _statList.Count + "条");
                _statList.Clear();
            }
            FileList.SetDataSource(_fileInfoList);
        }

        private void SineGrid_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            sineChart.Width = sineGrid.ActualWidth;
            sineChart.Height = sineChart.ActualHeight;
            AddData(sineChart);
        }

        private void AddData(LineChartControlLib myLineChart)
        {
            myLineChart.DataCollection.DataList.Clear();

            DataSeries ds=new DataSeries
            {
                LineColor = Brushes.Blue, 
                LineThickness = 1, 
                SeriesName = "Sine"
            };
            ds.Symbols = new CircleSymbols
            {
                BorderColor = ds.LineColor,
                SymbolSize = 6
            };
            for (int i = 0; i < 15; i++)
            {
                double x = i/2.0;
                double y = Math.Sin(x);
                ds.LineSeries.Points.Add(new Point(x,y));
            }
            myLineChart.DataCollection.DataList.Add(ds);

            ds=new DataSeries
            {
                LineColor = Brushes.Red, 
                SeriesName = "Cosine"
            };
            ds.Symbols = new OpenDiamondSymbols
            {
                BorderColor = ds.LineColor,
                SymbolSize = 6
            };
            for (int i = 0; i < 15; i++)
            {
                double x = i / 2.0;
                double y = Math.Cos(x);
                ds.LineSeries.Points.Add(new Point(x, y));
            }
            myLineChart.DataCollection.DataList.Add(ds);

            myLineChart.IsLegend = true;
            myLineChart.LegendPosition = LegendPosition.NorthEast;
        }

        private void Grid1_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            chart1.Width = grid1.ActualWidth;
            chart1.Height = grid1.ActualHeight;
            AddData(chart1);
        }

        private void Grid2_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            chart2.Width = grid2.ActualWidth;
            chart2.Height = grid2.ActualHeight;
            AddData(chart2);
        }

        private void Grid3_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            chart3.Width = grid3.ActualWidth;
            chart3.Height = grid3.ActualHeight;
            AddData(chart3);
        }

        private void Grid4_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            chart4.Width = grid4.ActualWidth;
            chart4.Height = grid4.ActualHeight;
            AddData(chart4);
        }
    }
}
