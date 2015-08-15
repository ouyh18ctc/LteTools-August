
namespace Lte.Domain.LinqToCsv.Description
{
    public class DataRowItem
    {
        private string value;
        private int lineNbr;


        public DataRowItem(string value, int lineNbr)
        {
            this.value = value;
            this.lineNbr = lineNbr;
        }


        public int LineNbr
        {
            get { return this.lineNbr; }
        }


        public string Value
        {
            get { return this.value; }
        }
    }

}
