using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Lte.Parameters.Concrete;
using Lte.WinApp.Models;
using Lte.WinApp.Service;
using MessageBox = System.Windows.MessageBox;

namespace Lte.WinApp.ViewPages
{
    /// <summary>
    /// TopDrop2GImportPage.xaml 的交互逻辑
    /// </summary>
    public partial class TopDrop2GImportPage
    {
        private readonly List<ImportedFileInfo> _fileInfoList = new List<ImportedFileInfo>();
        private readonly KpiFileInfoListImporterAsync _kpiImporterAsync;
        private readonly Precise4GFileInfoListImporterAsync _preciseImporterAsync;
        private readonly FileInfoListImporter _neighborImporter;

        public TopDrop2GImportPage()
        {
            InitializeComponent();
            PageTitle.Content = Title;
            _kpiImporterAsync = new KpiFileInfoListImporterAsync(1000)
            {
                FileInfoList = _fileInfoList,
                FileListGrid = FileList
            };
            _preciseImporterAsync = new Precise4GFileInfoListImporterAsync
            {
                FileInfoList = _fileInfoList,
                FileListGrid = FileList
            };
            _neighborImporter = new NeighborFileListImporter(new EFLteNeighborCellRepository())
            {
                FileInfoList = _fileInfoList,
                FileListGrid = FileList
            };
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            ImportedFileInfo[] validKpiFileInfos = _kpiImporterAsync.Query().ToArray();
            ImportedFileInfo[] validPreciseFileInfos = _preciseImporterAsync.Query().ToArray();
            ImportedFileInfo[] validNeighborFileInfos = _neighborImporter.Query().ToArray();
            if (validPreciseFileInfos.Length + validKpiFileInfos.Length 
                + validNeighborFileInfos.Length == 0)
            {
                MessageBox.Show("未选择任何有效的CSV文件。请先导入或选择！");
                return;
            }
            if (validKpiFileInfos.Any())
            {
                _kpiImporterAsync.Import(validKpiFileInfos);
            }

            if (validPreciseFileInfos.Any())
            {
                _preciseImporterAsync.Import(validPreciseFileInfos);
            }

            if (validNeighborFileInfos.Any())
            {
                 _neighborImporter.Import(validNeighborFileInfos);
            }
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            FileDialogWrapper wrapper = new OpenKpiFileDialogWrapper();
            if (wrapper.ShowDialog())
            {
                _kpiImporterAsync.ImportFiles(wrapper.FileNames);
                FileList.SetDataSource(_fileInfoList);
            }
        }

        private void OpenPreciseFile_Click(object sender, RoutedEventArgs e)
        {
            FileDialogWrapper wrapper = new OpenPreciseFileDialogWrapper();
            if (wrapper.ShowDialog())
            {
                _preciseImporterAsync.ImportFiles(wrapper.FileNames);
                FileList.SetDataSource(_fileInfoList);
            }
        }

        private void OpenNeighborFile_Click(object sender, RoutedEventArgs e)
        {
            FileDialogWrapper wrapper = new OpenLteNeighborFileDialogWrapper();
            if (wrapper.ShowDialog())
            {
                _neighborImporter.ImportFiles(wrapper.FileNames);
                FileList.SetDataSource(_fileInfoList);
            }
        }
    }
}
