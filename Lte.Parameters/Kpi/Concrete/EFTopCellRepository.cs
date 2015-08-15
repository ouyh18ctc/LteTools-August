using System.Linq;
using Lte.Parameters.Concrete;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Entities;

namespace Lte.Parameters.Kpi.Concrete
{
    public class EFTopDrop2GCellRepository : ITopCellRepository<TopDrop2GCell>
    {
        private readonly EFParametersContext context = new EFParametersContext();

        public IQueryable<TopDrop2GCell> Stats 
        {
            get
            {
                return context.TopDrop2GStats.AsQueryable();
            }
        }

        public void AddOneStat(TopDrop2GCell stat)
        {
            context.TopDrop2GStats.Add(stat);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }

    public class EFTopConnection3GRepository : ITopCellRepository<TopConnection3GCell>
    {
        private readonly EFParametersContext context = new EFParametersContext();

        public IQueryable<TopConnection3GCell> Stats
        {
            get
            {
                return context.TopConnection3GStats.AsQueryable();
            }
        }

        public void AddOneStat(TopConnection3GCell stat)
        {
            context.TopConnection3GStats.Add(stat);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }

    public class EFTopDrop2GcellDailyRepository : ITopCellRepository<TopDrop2GCellDaily>
    {
        private readonly EFParametersContext context = new EFParametersContext();

        public IQueryable<TopDrop2GCellDaily> Stats
        {
            get
            {
                return context.TopDrop2GDailyStats;
            }
        }

        public void AddOneStat(TopDrop2GCellDaily stat)
        {
            context.TopDrop2GDailyStats.Add(stat);
        }

        public void SaveChanges()
        {
            context.SaveChangesWithDelay();
        }
    }
}
