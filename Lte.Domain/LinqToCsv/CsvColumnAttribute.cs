using System;
using System.Globalization;
using Lte.Domain.Regular;

namespace Lte.Domain.LinqToCsv
{
    public class CsvColumnAttribute : ColumnAttribute
    {
        public NumberStyles NumberStyle { get; set; }
        public string OutputFormat { get; set; }
        public int CharLength { get; set; }

        public CsvColumnAttribute()
        {
            NumberStyle = NumberStyles.Any;
            OutputFormat = "G";
        }


        public CsvColumnAttribute(string name, int fieldIndex, bool canBeNull,
                    string outputFormat, NumberStyles numberStyle, int charLength)
            : base(name, fieldIndex, canBeNull)
        {

            NumberStyle = numberStyle;
            OutputFormat = outputFormat;
            CharLength = charLength;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class CsvInputFormatAttribute : Attribute
    {
        private NumberStyles m_NumberStyle = NumberStyles.Any;
        public NumberStyles NumberStyle
        {
            get { return m_NumberStyle; }
            set { m_NumberStyle = value; }
        }

        public CsvInputFormatAttribute(NumberStyles numberStyle)
        {
            m_NumberStyle = numberStyle;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class CsvOutputFormatAttribute : Attribute
    {
        private string m_Format = "";
        public string Format
        {
            get { return m_Format; }
            set { m_Format = value; }
        }

        public CsvOutputFormatAttribute(string format)
        {
            m_Format = format;
        }
    }
}
