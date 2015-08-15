using System.Collections.Generic;
using System.Windows.Controls;
using Lte.Parameters.Entities;

namespace Lte.WinApp.Controls
{
    /// <summary>
    /// CellListGrid.xaml 的交互逻辑
    /// </summary>
    public partial class CellListGrid : UserControl
    {
        public CellListGrid()
        {
            InitializeComponent();
        }

        public void SetDataSource(IEnumerable<CellExcel> list)
        {
            DataList.ItemsSource = null;
            DataList.ItemsSource = list;
        }
    }
}
