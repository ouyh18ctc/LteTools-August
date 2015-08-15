using Lte.Parameters.Abstract;
using System.Data;

namespace Lte.Parameters.Test.Import
{
    public class ImportClass : IValueImportable
    {
        public string Name { get; set; }
        public int Value { get; private set; }

        private readonly IDataReader dataReader;

        public ImportClass(){}

        public ImportClass(IDataReader dataReader)
        {
            this.dataReader = dataReader;
        }

        public void Import()
        {
            Name = dataReader.GetName(dataReader.Depth);
            Value = (int)dataReader.GetValue(dataReader.Depth);
        }
    }

}
