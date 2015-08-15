using System.Collections.Generic;
using System.Windows.Controls;
using Lte.Parameters.Entities;

namespace Lte.WinApp.Controls
{
    /// <summary>
    /// CdmaCellListGrid.xaml 的交互逻辑
    /// </summary>
    public partial class CdmaCellListGrid : UserControl
    {
        public CdmaCellListGrid()
        {
            InitializeComponent();
        }

        public void SetDataSource(IEnumerable<CdmaCellExcel> list)
        {
            DataList.ItemsSource = null;
            DataList.ItemsSource = list;
        }
    }
}
