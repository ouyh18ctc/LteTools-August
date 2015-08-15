using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;

namespace Lte.Domain.Regular
{
    public interface IExcelImporter
    {
        DataTable this[string tableName] { get; }
        void Close();
    }

    public class ExcelImporter : IExcelImporter, IDisposable
    {
        private readonly string[] sheetNames;

        private readonly DataSet dataSet = new DataSet();

        public void Dispose()
        {
            dataSet.Dispose();
            GC.SuppressFinalize(this);
        }

        public DataTable this[string tableName]
        {
            get
            {
                string name = sheetNames.FirstOrDefault(x => x == tableName);
                if (name == null)
                {
                    return null;
                }
                return dataSet.Tables[name];
            }
        }

        private readonly OleDbConnection conn;

        public ExcelImporter(string filePath, string[] sheetNames)
        {
            this.sheetNames = sheetNames;

            conn = GenerateOleConnection(filePath);
            if (conn != null)
            { conn.Open(); }
            else return;

            foreach (string sheet in sheetNames)
            {
                OleDbDataAdapter odda = new OleDbDataAdapter("select * from [" + sheet + "$]", conn);
                odda.Fill(dataSet, sheet);
            }
            conn.Close();
        }

        private static OleDbConnection GenerateOleConnection(string filePath)
        {
            string connstr2003 = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="
                + filePath + ";Extended Properties='Excel 8.0; HDR=YES; IMEX=1'";
            string connstr2007 = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="
                + filePath + ";Extended Properties='Excel 12.0; HDR=YES'";

            string extension = Path.GetExtension(filePath);
            if (extension != null)
            {
                string fileExt = extension.ToLower();
                OleDbConnection conn;

                if (fileExt == ".xls")
                { conn = new OleDbConnection(connstr2003); }
                else if (fileExt == ".xlsx")
                { conn = new OleDbConnection(connstr2007); }
                else { return null; }
                return conn;
            }
            return null;
        }

        public void Close()
        {
            if (conn != null) { conn.Close(); }
        }
    }
}
