using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Entities;
using Lte.Evaluations.Entities;
using Lte.Evaluations.Infrastructure.Entities;
using Lte.Evaluations.Rutrace.Record;
using Lte.Parameters.Entities;

namespace Lte.Evaluations.Service
{
    public abstract class GenerateStatService<T>
    {
        protected readonly IEnumerable<T> _statList;

        protected GenerateStatService(IEnumerable<T> statList)
        {
            _statList = statList;
        }
    }

    public abstract class GenerateSectorsStatService<T> : GenerateStatService<T>
    {
        protected readonly IEnumerable<SectorTriangle> _sectors;

        protected GenerateSectorsStatService(IEnumerable<T> statList,
            IEnumerable<IOutdoorCell> outdoorCellList)
            : base(statList)
        {
            _sectors = outdoorCellList.GetSectors();
        }

        public List<SectorTriangle> Generate(StatValueField field)
        {
            IEnumerable<Tuple<SectorTriangle, T>> filterSectors = FilterSectors();

            return SectorTriangles(field, filterSectors);
        }

        protected abstract IEnumerable<Tuple<SectorTriangle, T>> FilterSectors();

        protected abstract List<SectorTriangle> SectorTriangles(StatValueField field,
            IEnumerable<Tuple<SectorTriangle, T>> filterSectors);
    }

    public class GenerateMrsSectorsService : GenerateSectorsStatService<MrsCellDateView>
    {
        public GenerateMrsSectorsService(IEnumerable<MrsCellDateView> statList, IEnumerable<IOutdoorCell> outdoorCellList) 
            : base(statList, outdoorCellList)
        {
        }

        protected override IEnumerable<Tuple<SectorTriangle, MrsCellDateView>> FilterSectors()
        {
            return from s in _sectors
                   join st in _statList
                       on s.CellName equals st.CellName
                   select new Tuple<SectorTriangle, MrsCellDateView>(s, st);
        }

        protected override List<SectorTriangle> SectorTriangles(StatValueField field, 
            IEnumerable<Tuple<SectorTriangle, MrsCellDateView>> filterSectors)
        {
            Func<MrsCellDateView, InterferenceService<MrsCellDateView>> generator 
                = field.FieldName.MrsGenerator();

            return (generator == null)
                ? new List<SectorTriangle>()
                : filterSectors.Select(x =>
                {
                    x.Item1.ColorString = (generator(x.Item2)).GetColor(field);
                    x.Item1.Info += x.Item2.StatInfo;
                    return x.Item1;
                }).ToList();
        }
    }

    public class GenerateRuSectorsStatService : GenerateSectorsStatService<RuInterferenceStat>
    {
        public GenerateRuSectorsStatService(IEnumerable<RuInterferenceStat> statList,
            IEnumerable<IOutdoorCell> outdoorCellList)
            : base(statList, outdoorCellList)
        {
        }

        protected override List<SectorTriangle> SectorTriangles(StatValueField field, 
            IEnumerable<Tuple<SectorTriangle, RuInterferenceStat>> filterSectors)
        {
            Func<RuInterferenceStat, InterferenceService<RuInterferenceStat>> generator 
                = field.FieldName.GetGenerator();

            return (generator == null)
                ? new List<SectorTriangle>()
                : filterSectors.Select(x =>
                {
                    x.Item1.ColorString = (generator(x.Item2)).GetColor(field);
                    x.Item1.Info += x.Item2.StatInfo;
                    return x.Item1;
                }).ToList();
        }

        protected override IEnumerable<Tuple<SectorTriangle, RuInterferenceStat>> FilterSectors()
        {
            return from s in _sectors
                join st in _statList
                    on s.CellName equals st.LteCellName
                select new Tuple<SectorTriangle, RuInterferenceStat>(s, st);
        }
    }

    public class GenerateValuesStatService : GenerateStatService<RuInterferenceStat>
    {
        public GenerateValuesStatService(IEnumerable<RuInterferenceStat> statList)
            : base(statList)
        {
        }

        public List<double> GenerateValues(string fieldName)
        {
            Func<RuInterferenceStat, InterferenceService<RuInterferenceStat>> generator = fieldName.GetGenerator();

            return generator == null ? new List<double>() : _statList.Select(x => (generator(x)).GetValue()).ToList();
        }
    }

    public class GenerateValuesMrsService : GenerateStatService<MrsCellDateView>
    {
        public GenerateValuesMrsService(IEnumerable<MrsCellDateView> statList) : base(statList)
        {
        }

        public List<double> GenerateValues(string fieldName)
        {
            Func<MrsCellDateView, InterferenceService<MrsCellDateView>> generator = fieldName.MrsGenerator();

            return generator == null ? new List<double>() : _statList.Select(x => (generator(x)).GetValue()).ToList();
        }
    }
}
