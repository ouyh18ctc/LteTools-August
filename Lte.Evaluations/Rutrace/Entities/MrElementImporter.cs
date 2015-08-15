using System.Collections.Generic;
using System.Xml.Linq;

namespace Lte.Evaluations.Rutrace.Entities
{
    public abstract class MrElementImporter
    {
        private readonly IEnumerable<XElement> _objects;
        protected readonly List<MrRecord> _recordList;

        protected MrElementImporter(IEnumerable<XElement> objects, List<MrRecord> recordList)
        {
            _objects = objects;
            _recordList = recordList;
        }

        protected abstract void ImportObj(XElement obj, int eNodebId);

        public void Import(int eNodebId)
        {
            foreach (XElement obj in _objects)
            {
                ImportObj(obj, eNodebId);
            }
        }
    }

    public class MroElementImporter : MrElementImporter
    {
        public MroElementImporter(IEnumerable<XElement> objects, List<MrRecord> recordList)
            : base(objects, recordList)
        {
        }

        protected override void ImportObj(XElement obj, int eNodebId)
        {
            MrRecord record = new MroRecord(eNodebId, obj);
            if (record.RefCell.Rsrp < 255)
            {
                _recordList.Add(record);
            }
        }
    }

    public class MreElementImporter : MrElementImporter
    {
        public MreElementImporter(IEnumerable<XElement> objects, List<MrRecord> recordList) 
            : base(objects, recordList)
        {
        }

        protected override void ImportObj(XElement obj, int eNodebId)
        {
            if (obj.Attribute("EventType").Value == "A3")
            {
                XElement vElement = obj.Element("v");
                if (vElement != null) _recordList.Add(new MreRecord(eNodebId, vElement.Value));
            }
        }
    }
}
