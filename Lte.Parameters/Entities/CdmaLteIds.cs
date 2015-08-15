using System;
using System.Collections.Generic;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Regular;

namespace Lte.Parameters.Entities
{
    public class CdmaLteIds
    {
        public int ENodebId { get; set; }

        public int CdmaCellId { get; set; }
    }

    public class BtsENodebIds
    {
        public int ENodebId { get; set; }

        public int BtsId { get; set; }
    }

    public class CdmaLteNames : CdmaLteIds, ICdmaLteNames
    {
        public string CdmaName { get; set; }

        public string LteName { get; set; }

        public byte SectorId { get; set; }
    }

    public class MmlLineInfo
    {
        public string KeyWord { get; set; }

        public Dictionary<string, string> FieldInfos { get; set; }

        public MmlLineInfo(string line)
        {
            string[] parts = line.GetSplittedFields(": ");
            KeyWord = parts[0];
            if (parts.Length > 1)
            {
                string contents = parts[1].Substring(0, parts[1].Length - 1);
                string[] fields = contents.GetSplittedFields(", ");

                FieldInfos = new Dictionary<string, string>();

                for (int i = 0; i < fields.Length; i++)
                {
                    string[] fieldInfo = fields[i].GetSplittedFields('=');
                    if (fieldInfo.Length < 2) { break; }
                    FieldInfos.Add(fieldInfo[0],
                        (fieldInfo[1].Substring(0, 1) == "\"")
                        ? fieldInfo[1].Substring(1, fieldInfo[1].Length - 2)
                        : fieldInfo[1]);
                }
            }
        }

        public CdmaBts GenerateCdmaBts()
        {
            return new CdmaBts
            {
                BtsId = Convert.ToInt32(FieldInfos["BTSID"]),
                Name = FieldInfos["BTSNM"],
                ENodebId = -1
            };
        }

        public CdmaCell GenerateCdmaCell()
        {
            return new CdmaCell
            {
                BtsId = Convert.ToInt32(FieldInfos["BTSID"]),
                CellId = Convert.ToInt32(FieldInfos["CN"]),
                SectorId = Convert.ToByte(FieldInfos["SCTIDLST"]),
                Pn = Convert.ToInt16(FieldInfos["PNLST"]),
                CellType = FieldInfos["TYP"] == "CDMA1X" ? "1X" : "DO",
                Lac = FieldInfos["TYP"] == "CDMA1X" ? FieldInfos["LAC"] : ""
            };
        }
    }
}
