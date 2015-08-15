using System;
using System.Collections.Generic;
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
using Lte.Domain.Geo.Entities;
using Lte.Parameters.Service.Coverage;

namespace Lte.WinApp.ViewPages
{
    /// <summary>
    /// DtDatabaseTestPage.xaml 的交互逻辑
    /// </summary>
    public partial class DtDatabaseTestPage : Page
    {
        public DtDatabaseTestPage()
        {
            InitializeComponent();
            PageTitle.Content = Title;
        }

        private void ShowInfo_OnClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            double longtitute = double.Parse(Longtitute.Text);
            double lattitute = double.Parse(Lattitute.Text);
            IEnumerable<GeoPoint> points = new List<GeoPoint>
            {
                new GeoPoint(longtitute, lattitute)
            };
            switch (button.Content.ToString())
            {
                case "查询2G结果":
                    Info2G.ItemsSource = null;
                    Info2G.ItemsSource = DCTestService.Query2GFileRecords(points, 0.03);
                    break;
                case "查询3G结果":
                    Info3G.ItemsSource = null;
                    Info3G.ItemsSource = DCTestService.Query3GFileRecords(points, 0.03);
                    break;
                case "查询4G结果":
                    Info4G.ItemsSource = null;
                    Info4G.ItemsSource = DCTestService.Query4GFileRecords(points, 0.03);
                    break;
            }
        }
    }
}
