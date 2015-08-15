using System.IO;
using System.Xml;
using Lte.Evaluations.Kml;

namespace Lte.Evaluations.Test.Kml
{
    public class FrameworkWriter
    {
        protected TextWriter writer;

        protected TextReader reader;

        protected void Initialize()
        {
            KmlFramework framework = new KmlFramework("aaa");
            writer = new StringWriter();
            XmlWriter xmlWriter = framework.GenerateWriter(writer);
            xmlWriter.Close();
            reader = new StringReader(writer.ToString());
        }
    }
}
