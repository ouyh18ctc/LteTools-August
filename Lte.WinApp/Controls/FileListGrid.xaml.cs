using System.Collections.Generic;
using System.Windows.Controls;
using Lte.WinApp.Models;

namespace Lte.WinApp.Controls
{
    /// <summary>
    /// FileListGrid.xaml 的交互逻辑
    /// </summary>
    public partial class FileListGrid : UserControl
    {
        public FileListGrid()
        {
            InitializeComponent();
        }

        public void SetDataSource(IEnumerable<ImportedFileInfo> list)
        {
            DataList.ItemsSource = null;
            DataList.ItemsSource = list;
        }
    }
}
