using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lte.Domain.LinqToCsv.StreamDef
{
    public class CsvStreamLine
    {
        private CsvStream stream;

        private CsvStreamBuffer buffer;

        public CsvStreamLine(CsvStream stream, CsvStreamBuffer buffer)
        {
            this.stream = stream;
            this.buffer = buffer;
        }

        private bool eos = false;

        public bool Eos
        {
            get { return eos; }
            set { eos = value; }
        }

        private bool eol = false;

        public bool Eol
        {
            get { return eol; }
        }

        private bool previousWasCr = false;

        public void AdvanceToEndOfLine()
        {
            while (true)
            {
                char c = buffer.GetNextChar(true);


                if (eos)
                    break;


                if (c == '\x0D') //'\r'
                {
                    stream.LineNbr++;
                    previousWasCr = true;


                    // we are at the end of the line, eat newline characters and exit
                    eol = true;
                    if (c == '\x0D' && buffer.GetNextChar(false) == '\x0A')
                    {
                        // new line sequence is 0D0A
                        buffer.GetNextChar(true);
                    }
                    eol = false;


                    break;
                }
            }
        }

        public bool GetNextItem(ref string itemString, int? itemLength = null)
        {
            itemString = null;
            if (eol)
            {
                // previous item was last in line, start new line
                eol = false;
                return false;
            }

            bool itemFound = false;
            bool quoted = false;
            bool predata = true;
            bool postdata = false;
            StringBuilder item = new StringBuilder();

            int cnt = 0;
            while (true)
            {
                if (itemLength.HasValue && cnt >= itemLength.Value)
                {
                    itemString = item.ToString();
                    return true;
                }

                char c = this.buffer.GetNextChar(true);
                cnt++;

                if (eos)
                {
                    if (itemFound) { itemString = item.ToString(); }
                    return itemFound;
                }

                // Keep track of line number. 
                // Note that line breaks can happen within a quoted field, not just at the
                // end of a record.
                // Don't count 0D0A as two line breaks.
                if ((!previousWasCr) && (c == '\x0A'))
                {
                    this.stream.LineNbr++;
                }


                if (c == '\x0D') //'\r'
                {
                    this.stream.LineNbr++;
                    previousWasCr = true;
                }
                else
                {
                    previousWasCr = false;
                }

                if ((postdata || !quoted) && (itemLength == null && c == this.stream.SeparatorChar))
                {
                    if (this.stream.IgnoreTrailingSeparatorChar)
                    {
                        char nC = this.buffer.GetNextChar(false);
                        if ((nC == '\x0A' || nC == '\x0D'))
                            continue;
                    }
                    // end of item, return
                    if (itemFound) { itemString = item.ToString(); }
                    return true;
                }


                if ((predata || postdata || !quoted) && (c == '\x0A' || c == '\x0D'))
                {
                    // we are at the end of the line, eat newline characters and exit
                    eol = true;
                    if (c == '\x0D' && this.buffer.GetNextChar(false) == '\x0A')
                    {
                        // new line sequence is 0D0A
                        this.buffer.GetNextChar(true);
                    }


                    if (itemFound) { itemString = item.ToString(); }
                    return true;
                }


                if (predata && c == ' ')
                    // whitespace preceeding data, discard
                    continue;


                if (predata && c == '"')
                {
                    // quoted data is starting
                    quoted = true;
                    predata = false;
                    itemFound = true;
                    continue;
                }


                if (predata)
                {
                    // data is starting without quotes
                    predata = false;
                    item.Append(c);
                    itemFound = true;
                    continue;
                }


                if (c == '"' && quoted)
                {
                    if (this.buffer.GetNextChar(false) == '"')
                    {
                        // double quotes within quoted string means add a quote       
                        item.Append(this.buffer.GetNextChar(true));
                    }
                    else
                    {
                        // end-quote reached
                        postdata = true;
                    }

                    continue;
                }


                // all cases covered, character must be data
                item.Append(c);
            }
        }

    }
}
