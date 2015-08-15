using System;
using System.Data;
using Lte.Parameters.Entities;
using Moq;

namespace Lte.Parameters.Test.Entities
{
    public static class MockDataReaderOperations
    {
        public static void MockReaderContents(this Mock<IDataReader> mockReader,
            Tuple<string, string>[] contents)
        {
            mockReader.Setup(x => x.FieldCount).Returns(100);
            mockReader.Setup(x => x.GetName(It.IsAny<int>())).Returns("Undefined");
            for (int i = 0; i < contents.Length; i++)
            {
                mockReader.Setup(x => x.GetName(i)).Returns(contents[i].Item1);
                mockReader.Setup(x => x.GetValue(i)).Returns(contents[i].Item2);
            }
        }
    }

    public class CdmaTestConfig
    {
        protected MmlLineInfo btsLineInfo;
        protected MmlLineInfo cellLineInfo;
        protected CdmaBts bts;
        protected CdmaCell cell;

        protected void Initialize()
        {
            btsLineInfo = new MmlLineInfo(
                "ADD BSCBTSINF: BTSTP=IBSC, BTSID=50, BTSNM=\"张槎工贸\", FN=7, SN=8, SSN=3, ABISCAPOPTMSW=OFF, ABISOPTMCRCSW=OFF, ABISREVFRAMEPERIOD=3, HIGHPW=NOHIGHPW, ABISREDUNDANCESW=OFF, INTRAFADAPFILTER=OFF, ABISBETRFBWTHR=0, PNSHARENUM1X=0, PNSHARENUMDO=0, ABISSATTRANSSW=OFF, DATATRFCRCSW=OFF, TODSW=OFF, VIP1XOCCUPYRES=OFF, BTSGRADE=GRADEC, BTSLOCATIONTYPE=LOCATION0, BTSPHYTYPE=MACRO;"
                );
            bts = btsLineInfo.GenerateCdmaBts();
            cellLineInfo = new MmlLineInfo(
                "ADD CELL: BTSID=4, FN=7, CN=3964, SCTIDLST=\"3\", PNLST=\"198\", SID=13832, NID=65535, PZID=1, TYP=EVDO, LCN=3964, LSCTID=\"3\", ASSALWDO=NO, DOAREVRSSICARRASSNSW=OFF, DOAPRVPRIASSSW=OFF, DOMULTIBANDASSIGNSW=OFF, DOUSERCOUNTTHD=20, DOAUTODWNCOUNTTHD=600, DOUNBLKUSERCOUNTTHD=40, LOCATE=URBAN, MICROCELL=NO, STAYMODE=MODE0, BANDCLASSASSIGNSW=OFF, DOBLOADEQUIARISW=OFF;"
                );
            cell = cellLineInfo.GenerateCdmaCell();
        }
    }

}
