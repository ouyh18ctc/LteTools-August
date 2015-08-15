using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lte.Domain.LinqToCsv.Context;
using Lte.Domain.LinqToCsv.Description;
using Lte.Domain.LinqToCsv.Test;
using NUnit.Framework;

namespace Lte.Domain.Test.LinqToCsv
{
    public abstract class Test
    {        
        protected void AssertCollectionsEqual<T>(IEnumerable<T> actual, IEnumerable<T> expected) where T : IAssertable<T>
        {
            int count = actual.Count();
            Assert.AreEqual(count, expected.Count(), "counts");

            for (int i = 0; i < count; i++)
            {
                actual.ElementAt(i).AssertEqual(expected.ElementAt(i));
            }
        }

        protected void AssertRead<T>(string testInput, CsvFileDescription fileDescription, IEnumerable<T> expected)
            where T : class, IAssertable<T>, new()
        {
            List<T> actual = CsvContext.ReadString<T>(testInput, fileDescription).ToList();
            AssertCollectionsEqual<T>(actual, expected);
        }

        private string TestWrite<T>(IEnumerable<T> values, CsvFileDescription fileDescription) where T : class
        {
            TextWriter stream = new StringWriter();
            CsvContext.Write(values, stream, fileDescription);
            return stream.ToString();
        }

        public void AssertWrite<T>(IEnumerable<T> values, CsvFileDescription fileDescription, string expected) where T : class
        {
            string actual = TestWrite<T>(values, fileDescription);
            Assert.AreEqual(Utils.NormalizeString(actual), Utils.NormalizeString(expected));
        }
    }
}
