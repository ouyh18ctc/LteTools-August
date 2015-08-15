using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Lte.Domain.Geo;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Entities;
using Lte.Evaluations.Infrastructure.Entities;
using Lte.Evaluations.Rutrace.Record;
using Lte.Evaluations.Service;
using Lte.Parameters.Entities;

namespace Lte.Evaluations.Entities
{
    public abstract class StatFieldRepository
    {
        protected IList<StatValueField> fieldList = new List<StatValueField>();

        public IList<StatValueField> FieldList
        {
            get { return fieldList; }

            set { fieldList = value; }
        }

        public XDocument FieldDoc
        {
            get
            {
                XDocument fieldDoc = new XDocument();
                fieldDoc.Add(new XElement("Setting"));
                fieldDoc.Descendants("Setting").First().Descendants().Remove();
                foreach (StatValueField field in fieldList)
                {
                    fieldDoc.Descendants("Setting").First().Add(field.GetFieldElement());
                }
                return fieldDoc;
            }

            set
            {
                fieldList.Clear();
                foreach (XElement field in value.Descendants("Field"))
                {
                    fieldList.Add(new StatValueField(field));
                }
            }
        }

        public StatValueField this[string fieldName]
        {
            get
            {
                return fieldList.FirstOrDefault(x => x.FieldName == fieldName);
            }

        }

        public StatValueField GenerateDefaultField(string fieldName)
        {
            StatValueField field = FieldList[(int)fieldName.GetStatValueIndex()];
            if (field.IntervalList.Count == 0)
            {
                field.AutoGenerateIntervals(8);
            }
            return field;
        }

        public List<SectorTriangle> GenerateSectors(IEnumerable<RuInterferenceStat> statList,
            IEnumerable<IOutdoorCell> outdoorCellList, string fieldName)
        {
            GenerateSectorsStatService<RuInterferenceStat> service
                = new GenerateRuSectorsStatService(statList, outdoorCellList);
            return service.Generate(this[fieldName]);
        }

        public List<SectorTriangle> GenerateSectors(IEnumerable<MrsCellDateView> statList,
            IEnumerable<IOutdoorCell> outdoorCellList, string fieldName)
        {
            GenerateSectorsStatService<MrsCellDateView> service
                = new GenerateMrsSectorsService(statList, outdoorCellList);
            return service.Generate(this[fieldName]);
        }
    }

    public class StatValueFieldRepository : StatFieldRepository
    {
        public StatValueFieldRepository()
        {
            foreach (string fieldName in StatValueChoiceQueries.Choices)
            {
                fieldList.Add(new StatValueField { FieldName = fieldName });
            }
        }
    }

    public class StatRuFieldRepository : StatFieldRepository
    {
        public StatRuFieldRepository()
        {
            foreach (string fieldName in StatRuChoiceQueries.Choices)
            {
                fieldList.Add(new StatValueField { FieldName = fieldName });
            }
        }
    }

    public class StatComplexFieldRepository : StatFieldRepository
    {
        public StatComplexFieldRepository()
        {
            foreach (string fieldName in StatValueChoiceQueries.Choices)
            {
                fieldList.Add(new StatValueField { FieldName = fieldName });
            }

            foreach (string fieldName in StatRuChoiceQueries.Choices)
            {
                fieldList.Add(new StatValueField { FieldName = fieldName });
            }
        }
    }
}
