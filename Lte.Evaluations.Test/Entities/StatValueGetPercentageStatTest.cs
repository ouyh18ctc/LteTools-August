using System.Collections.Generic;
using System.Linq;
using Lte.Evaluations.Entities;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Entities
{
    [TestFixture]
    public class StatValueGetPercentageStatTest
    {
        private readonly StatValueField field = new StatValueField
        {
            FieldName = "aaa",
            IntervalList = new List<StatValueInterval> {
                new StatValueInterval {
                    IntervalLowLevel = 1,
                    IntervalUpLevel = 2 },
                new StatValueInterval {
                    IntervalLowLevel = 2,
                    IntervalUpLevel = 3 },
                new StatValueInterval {
                    IntervalLowLevel = 3,
                    IntervalUpLevel = 4 },
                new StatValueInterval {
                    IntervalLowLevel = 4,
                    IntervalUpLevel = 5 }
            }
        };

        [Test]
        public void TestStatValueGetPercentageStat()
        {
            IEnumerable<double> values = new[]{
                2.1,2.2,3.1,2.4,4.6,3.7,1.8,2.1,2.2,1.1,1.7,3.1,3.3,3.8,4.7};
            Dictionary<string, double> result = field.GetPercentageStat(values);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 4);
            Assert.AreEqual(result.ElementAt(0).Key, "[ 1 , 2 )");
            Assert.AreEqual(result["[ 1 , 2 )"], 0.2);
            Assert.AreEqual(result["[ 2 , 3 )"], 0.333333, 1E-6);
            Assert.AreEqual(result["[ 3 , 4 )"], 0.333333, 1E-6);
            Assert.AreEqual(result["[ 4 , 5 )"], 0.133333, 1E-6);
        }
    }
}
