using System.IO;
using Lte.Domain.Regular;
using NUnit.Framework;

namespace Lte.Domain.Test.Regular
{
    [TestFixture]
    public class GetStreamReaderTest
    {

        [Test]
        public void TestStreamReaderFromString()
        {
            const string input = @"Id,Name,Last Name,Age,City
1,John,Doe,15,Washington";

            StreamReader sReader = input.GetStreamReader();
            Assert.IsNotNull(sReader);
        }

    }
}
