using System.Windows.Controls;

namespace Lte.WinApp.ViewPages
{
    public class ViewPageFactory
    {
        public Page NavigateToPage(string pageName)
        {
            Page viewPage;
            switch (pageName)
            { 
                case "TopDrop2G":
                    viewPage = new TopDrop2GImportPage();
                    break;
                case "RutraceCdr":
                    viewPage = new RutraceCdrImportPage();
                    break;
                case "RuMrDisplay":
                    viewPage = new RutraceMrDisplay();
                    break;
                case "Parameters":
                    viewPage = new ParametersImportPage();
                    break;
                case "DtDisplay":
                    viewPage=new DtDatabaseTestPage();
                    break;
                default:
                    viewPage = null;
                    break;
            }
            return viewPage;
        }
    }
}
