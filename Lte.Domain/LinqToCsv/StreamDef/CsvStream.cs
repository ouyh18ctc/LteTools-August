using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Lte.Domain.LinqToCsv.Description;

namespace Lte.Domain.LinqToCsv.StreamDef
{
    /// <summary>
    /// Based on code found at
    /// http://knab.ws/blog/index.php?/archives/3-CSV-file-parser-and-writer-in-C-Part-1.html
    /// and
    /// http://knab.ws/blog/index.php?/archives/10-CSV-file-parser-and-writer-in-C-Part-2.html
    /// </summary>
    public class CsvStream
    {
        private TextReader inStream;

        public TextReader InStream
        {
            get { return inStream; }
        }

        private TextWriter outStream;
        private char separatorChar;

        public char SeparatorChar
        {
            get { return separatorChar; }
        }

        private char[] specialChars;

        private bool ignoreTrailingSeparatorChar;

        public bool IgnoreTrailingSeparatorChar
        {
            get { return ignoreTrailingSeparatorChar; }
        }

        // Current line number in the file. Only used when reading a file, not when writing a file.
        private int lineNbr;

        public int LineNbr
        {
            get { return lineNbr; }
            set { lineNbr = value; }
        }

        private CsvStreamLine line;
        private CsvStreamBuffer buffer;

        public CsvStream(TextReader inStream, TextWriter outStream, char separatorChar, bool ignoreTrailingSeparatorChar)
        {
            this.inStream = inStream;
            this.outStream = outStream;
            this.separatorChar = separatorChar;
            this.ignoreTrailingSeparatorChar = ignoreTrailingSeparatorChar;
            this.specialChars = ("\"\x0A\x0D" + separatorChar.ToString()).ToCharArray();
            this.lineNbr = 1;

            this.buffer = new CsvStreamBuffer(this);
            this.line = new CsvStreamLine(this, buffer);
            this.buffer.Line = this.line;
        }

        public void WriteRow(List<string> row, bool quoteAllFields)
        {
            bool firstItem = true;
            foreach (string item in row)
            {
                if (!firstItem) { outStream.Write(this.separatorChar); }

                // If the item is null, don't write anything.
                if (item != null)
                {
                    // If user always wants quoting, or if the item has special chars
                    // (such as "), or if item is the empty string or consists solely of
                    // white space, surround the item with quotes.
                    if ((quoteAllFields ||
                        (item.IndexOfAny(specialChars) > -1) ||
                        (item.Trim() == "")))
                    {
                        outStream.Write("\"" + item.Replace("\"", "\"\"") + "\"");
                    }
                    else
                    {
                        outStream.Write(item);
                    }
                }

                firstItem = false;
            }

            outStream.WriteLine("");
        }

        /// <param name="row">
        /// Contains the values in the current row, in the order in which they 
        /// appear in the file.
        /// </param>
        /// <returns>
        /// True if a row was returned in parameter "row".
        /// False if no row returned. In that case, you're at the end of the file.
        /// </returns>
        public bool ReadRow(IDataRow row, List<int> charactersLength = null)
        {
            row.Clear();

            int i = 0;
            while (true)
            {
                // Number of the line where the item starts. Note that an item
                // can span multiple lines.
                int startingLineNbr = lineNbr;
                string item = null;
                int? itemLength = charactersLength == null ? (int?)null : charactersLength.Skip(i).First();

                bool moreAvailable = line.GetNextItem(ref item, itemLength);
                if (charactersLength != null)
                {
                    if (!(charactersLength.Count() > i + 1))
                    {
                        if (moreAvailable)
                        {
                            row.Add(new DataRowItem(item, startingLineNbr));
                        }


                        if (!line.Eol)
                        {
                            //line.AdvanceToEndOfLine();
                            moreAvailable = false;
                        }
                    }
                }

                if (!moreAvailable)
                {
                    return (row.Count > 0);
                }


                row.Add(new DataRowItem(item, startingLineNbr));


                i++;
            }
        }
        
    }

}
