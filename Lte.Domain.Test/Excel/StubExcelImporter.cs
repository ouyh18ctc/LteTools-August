using System.Linq;
using System.Data;
using Lte.Domain.Regular;

namespace Lte.Domain.Test.Excel
{
    public class StubExcelImporter : IExcelImporter
    {
        private readonly DataTable[] dataTables;

        public DataTable this[string tableName]
        {
            get { return dataTables.FirstOrDefault(x => x.TableName == tableName); }
        }

        public StubExcelImporter(string[] tableNames)
        {
            dataTables = new DataTable[tableNames.Length];
            for (int i = 0; i < tableNames.Length; i++)
            {
                dataTables[i] = new DataTable(tableNames[i]);
            }
        }

        public void Close()
        { }
    }
}
