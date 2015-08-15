using System.Windows;
using System.Windows.Controls;
using Lte.WinApp.ViewPages;

namespace Lte.WinApp
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {
        private readonly ViewPageFactory _factory = new ViewPageFactory();

        public MainPage()
        {
            InitializeComponent();
            PageTitle.Content = Title;
            TopDropImport.ButtonTitle = Properties.Resources.MainMenu_KpiImportTitle;
            TopDropImport.ButtonContent = Properties.Resources.MainMenu_KpiImportComments;
            RutraceImport.ButtonTitle = Properties.Resources.MainMenu_RutraceImportTitle;
            RutraceImport.ButtonContent = Properties.Resources.MainMenu_RutraceImportComments;
            RutraceDisplay.ButtonTitle = Properties.Resources.MainMenu_RuMrDisplayTitle;
            RutraceDisplay.ButtonContent = Properties.Resources.MainMenu_RuMrDisplayComments;
            ParametersImport.ButtonTitle = Properties.Resources.MainMenu_ParametersTitle;
            ParametersImport.ButtonContent = Properties.Resources.MainMenu_ParametersComments;
            DtDisplay.ButtonTitle = Properties.Resources.MainMenu_DtDisplayTitle;
            DtDisplay.ButtonContent = Properties.Resources.MainMenu_DtDisplayContents;
            TopDropImport.Button.Click += Button_Click;
            RutraceImport.Button.Click += Button_Click;
            RutraceDisplay.Button.Click += Button_Click;
            ParametersImport.Button.Click += Button_Click;
            DtDisplay.Button.Click += Button_Click;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Page page = _factory.NavigateToPage(((Button)sender).Tag.ToString());
            if (page != null  && NavigationService != null)
            {
                NavigationService.Navigate(page);
            }
        }
    }
}
