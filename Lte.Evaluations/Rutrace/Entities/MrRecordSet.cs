using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Lte.Domain.Regular;
using Lte.Evaluations.Infrastructure.Abstract;
using Lte.Evaluations.Service;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Evaluations.Rutrace.Entities
{
    public interface IMrRecordSet
    {
        DateTime RecordDate { get; set; }

        int ENodebId { get; set; }
    }

    public static class MrRecordSetOperations
    {
        public static XElement ImportBasicParameters(this IMrRecordSet set, StreamReader inStream)
        {
            XElement xElement = XDocument.Load(inStream).Element("bulkPmMrDataFile");
            if (xElement == null) return null;
            XElement element = xElement.Element("fileHeader");
            if (element != null)
                set.RecordDate =
                    element.Attribute("endTime").Value.ConvertToDateTime(DateTime.Today).Date;
            XElement eNBElement = xElement.Element("eNB");
            if (eNBElement != null)
            {
                set.ENodebId = eNBElement.Attribute("id").Value.ConvertToInt(10000);
            }
            return eNBElement;
        }
    }

    public abstract class MrRecordSet : IRecordSet<MrRecord, MrReferenceCell, MrNeighborCell>, IMrRecordSet
    {
        public List<MrRecord> RecordList { get; set; }

        public DateTime RecordDate { get; set; }

        public int ENodebId { get; set; }

        protected MrRecordSet()
        {
            RecordList = new List<MrRecord>();
        }

        protected MrRecordSet(StreamReader inStream) : this()
        {
            XElement eNBElement = this.ImportBasicParameters(inStream);
            if (eNBElement == null) return;
            IEnumerable<XElement> objects = eNBElement.ImportMeasurementDoc();
            if (objects == null) return;
            MrElementImporter importer = ConstructImporter(objects);
            importer.Import(ENodebId);
        }

        protected abstract MrElementImporter ConstructImporter(IEnumerable<XElement> objects);

        public void ImportRecordSet(INearestPciCellRepository repository)
        {
            foreach (MrRecord record in RecordList)
            {
                foreach (MrNeighborCell neighborCell in record.NbCells)
                {
                    NearestPciCell pciCell =repository.NearestPciCells.FirstOrDefault(x=>
                        x.CellId == record.RefCell.CellId &&
                        x.SectorId == record.RefCell.SectorId &&
                        x.Pci == neighborCell.Pci &&
                        (x.NearestSectorId < 30) == (neighborCell.Frequency < 1000)) 
                        ?? repository.Import(record.RefCell, neighborCell.Pci);
                    if (pciCell==null) continue;
                    neighborCell.CellId = pciCell.NearestCellId;
                    neighborCell.SectorId = pciCell.NearestSectorId;
                }
            }
        }
    }

    public sealed class MroRecordSet : MrRecordSet
    {
        public MroRecordSet(StreamReader inStream) : base(inStream)
        {
        }

        public MroRecordSet()
        {
        }

        protected override MrElementImporter ConstructImporter(IEnumerable<XElement> objects)
        {
            return new MroElementImporter(objects, RecordList);
        }
    }

    public sealed class MreRecordSet : MrRecordSet
    {
        public MreRecordSet(StreamReader inStream) : base(inStream)
        {
        }

        protected override MrElementImporter ConstructImporter(IEnumerable<XElement> objects)
        {
            return new MreElementImporter(objects, RecordList);
        }
    }

    public class MrsRecordSet : IMrRecordSet
    {
        public DateTime RecordDate { get; set; }

        public int ENodebId { get; set; }

        public List<MrsCell> MrsCells { get; set; }

        public MrsRecordSet()
        {
            MrsCells = new List<MrsCell>();
        }

        public MrsRecordSet(StreamReader inStream) : this()
        {
            XElement eNBElement = this.ImportBasicParameters(inStream);
            if (eNBElement == null) return;
            IEnumerable<XElement> objects = eNBElement.ImportMeasurementDoc("MR.RSRP");
            if (objects == null) return;
            foreach (XElement doc in objects)
            {
                int cgi = doc.Attribute("id").Value.ConvertToInt(0);
                string value = doc.Descendants("v").First().Value;
                MrsCells.Add(new MrsCell
                {
                    CellId = ENodebId,
                    SectorId = cgi.GetLastByte(),
                    RsrpCounts = value.GetSplittedFields(' ').Select(x => x.ConvertToInt(0)).ToArray()
                });
            }
            objects = eNBElement.ImportMeasurementDoc("MR.Tadv");
            if (objects == null) return;
            foreach (XElement doc in objects)
            {
                int cgi = doc.Attribute("id").Value.ConvertToInt(0);
                string value = doc.Descendants("v").First().Value;
                byte sectorId = cgi.GetLastByte();
                MrsCell cell = MrsCells.FirstOrDefault(x => x.CellId == ENodebId && x.SectorId == sectorId);
                if (cell == null)
                {
                    cell = new MrsCell();
                    MrsCells.Add(cell);
                }
                cell.CellId = ENodebId;
                cell.SectorId = sectorId;
                cell.TaCounts = value.GetSplittedFields(' ').Select(x => x.ConvertToInt(0)).ToArray();
            }
        }
    }
}
