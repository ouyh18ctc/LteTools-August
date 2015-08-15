using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lte.Domain.LinqToCsv.StreamDef
{
    public class CsvStreamBuffer
    {
        private char[] buffer = new char[4096];
        private int pos = 0;
        private int length = 0;

        private CsvStream stream;
        private CsvStreamLine line;

        public CsvStreamLine Line
        {
            set { line = value; }
        }

        public CsvStreamBuffer(CsvStream stream)
        {
            this.stream = stream;
        }

        public char GetNextChar(bool eat)
        {
            if (pos >= length)
            {
                length = this.stream.InStream.ReadBlock(buffer, 0, buffer.Length);
                if (length == 0)
                {
                    this.line.Eos = true;
                    return '\0';
                }
                pos = 0;
            }
            if (eat)
                return buffer[pos++];
            else
                return buffer[pos];
        }
    }
}
