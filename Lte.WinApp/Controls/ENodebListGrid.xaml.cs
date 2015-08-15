using System.Collections.Generic;
using System.Windows.Controls;
using Lte.Parameters.Entities;

namespace Lte.WinApp.Controls
{
    /// <summary>
    /// ENodebListGrid.xaml 的交互逻辑
    /// </summary>
    public partial class ENodebListGrid : UserControl
    {
        public ENodebListGrid()
        {
            InitializeComponent();
        }

        public void SetDataSource(IEnumerable<ENodebExcel> list)
        {
            DataList.ItemsSource = null;
            DataList.ItemsSource = list;
        }
    }
}
