using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.Parameters.Entities;
using NUnit.Framework;

namespace Lte.Parameters.Test.Entities
{
    [TestFixture]
    public class ENodebExcelComparerTest
    {
        [TestCase(new[] { 1, 3, 2, 4 }, 4)]
        [TestCase(new[] { 1, 3, 2, 4, 4 }, 4)]
        [TestCase(new[] { 1, 3, 2, 4, 2 }, 4)]
        [TestCase(new[] { 1, 3, 2, 4, 5 }, 5)]
        public void Test(int[] eNodebIds, int resultLength)
        {
            List<ENodebExcel> infos= eNodebIds.Select((t, i) => new ENodebExcel
            {
                ENodebId = t, Name = "name" + i
            }).ToList();

            IEnumerable<ENodebExcel> result = infos.Distinct(new ENodebExcelComparer());
            Assert.AreEqual(result.Count(),resultLength);
        }
    }
}
