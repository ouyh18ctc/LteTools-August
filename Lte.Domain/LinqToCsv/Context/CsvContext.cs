using System.Collections.Generic;
using System.IO;
using Lte.Domain.Regular;
using Lte.Domain.LinqToCsv.Description;
using Lte.Domain.LinqToCsv.Mapper;
using Lte.Domain.LinqToCsv.StreamDef;

namespace Lte.Domain.LinqToCsv.Context
{
    /// <summary>
    /// Summary description for CsvContext
    /// </summary>
    public static class CsvContext
    {
       
        public static IEnumerable<T> Read<T>(string fileName, CsvFileDescription fileDescription) where T : class, new()
        {
            
            FileDataAccess dataAccess = new FileDataAccess(null, fileDescription);
            return dataAccess.ReadData<T>(fileName);
        }
        
        public static IEnumerable<T> Read<T>(StreamReader stream) where T : class, new()
        {
            return Read<T>(stream, new CsvFileDescription());
        }
        
        public static IEnumerable<T> Read<T>(string fileName) where T : class, new()
        {
            return Read<T>(fileName, new CsvFileDescription());
        }
        
        public static IEnumerable<T> Read<T>(StreamReader stream, CsvFileDescription fileDescription) where T : class, new()
        {
            FileDataAccess dataAccess = new FileDataAccess(stream, fileDescription);
            return dataAccess.ReadData<T>(null);
        }

        public static IEnumerable<T> ReadString<T>(string testInput, CsvFileDescription fileDescription) where T : class, new()
        {
            return Read<T>(testInput.GetStreamReader(), fileDescription);
        }

        public static IEnumerable<T> ReadString<T>(string testInput) where T : class, new()
        {
            return Read<T>(testInput.GetStreamReader());
        }

        public static void Write<T>(IEnumerable<T> values, string fileName, CsvFileDescription fileDescription)
        {
            using (StreamWriter sw = new StreamWriter(fileName, false, fileDescription.TextEncoding))
            {
                WriteData(values, fileName, sw, fileDescription);
            }
        }

        public static void Write<T>(IEnumerable<T> values, TextWriter stream)
        {
            Write(values, stream, new CsvFileDescription());
        }

        public static void Write<T>(IEnumerable<T> values, string fileName)
        {
            Write(values, fileName, new CsvFileDescription());
        }

        public static void Write<T>(IEnumerable<T> values, TextWriter stream, CsvFileDescription fileDescription)
        {
            WriteData(values, null, stream, fileDescription);
        }

        private static void WriteData<T>(IEnumerable<T> values, string fileName, TextWriter stream,
            CsvFileDescription fileDescription)
        {
            FieldMapper<T> fm = new FieldMapper<T>(fileDescription, fileName, true);
            CsvStream cs = new CsvStream(null, stream, fileDescription.SeparatorChar, 
                fileDescription.IgnoreTrailingSeparatorChar);

            List<string> row = new List<string>();

            // If first line has to carry the field names, write the field names now.
            if (fileDescription.FirstLineHasColumnNames)
            {
                fm.WriteNames(row);
                cs.WriteRow(row, fileDescription.QuoteAllFields);
            }

            foreach (T obj in values)
            {
                // Convert obj to row
                fm.WriteObject(obj, row);
                cs.WriteRow(row, fileDescription.QuoteAllFields);
            }
        }

    }

}
