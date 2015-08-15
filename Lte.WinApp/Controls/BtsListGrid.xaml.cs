using System.Collections.Generic;
using System.Windows.Controls;
using Lte.Parameters.Entities;

namespace Lte.WinApp.Controls
{
    /// <summary>
    /// BtsListGrid.xaml 的交互逻辑
    /// </summary>
    public partial class BtsListGrid : UserControl
    {
        public BtsListGrid()
        {
            InitializeComponent();
        }

        public void SetDataSource(IEnumerable<BtsExcel> list)
        {
            DataList.ItemsSource = null;
            DataList.ItemsSource = list;
        }
    }
}
