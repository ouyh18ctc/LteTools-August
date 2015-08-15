using Lte.Parameters.Abstract;
using System.Data;
using Lte.Domain.Regular;

namespace Lte.Parameters.Test.Import
{
    internal class ColumnImportClass : IDataReaderImportable
    {
        public string Column1 { get; private set; }

        public int Column2 { get; private set; }

        public void Import(IDataReader dataReader)
        {
            Column1 = dataReader.GetField("Column1");
            Column2 = dataReader.GetField("Column2").ConvertToInt(0);
        }
    }

}
