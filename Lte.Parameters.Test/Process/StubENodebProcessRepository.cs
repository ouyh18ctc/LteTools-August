using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Test.Process
{
    public class StubENodebProcessRepository
    {
        public IQueryable<ENodeb> ENodebs 
        {
            get
            {
                return new List<ENodeb>
                {
                    new ENodeb
                    {
                        ENodebId = 1,
                        Name = "aaa",
                        TownId = 122
                    }
                }.AsQueryable();
            }
        }

        public int CurrentProgress { get; private set; }

        public StubENodebProcessRepository()
        {
            CurrentProgress = 0;
        }

        public void AddOneENodeb(ENodeb eNodeb)
        {
            CurrentProgress += 10;
        }

        public bool SaveENodeb()
        { return true; }

        public int SaveENodebs(List<ENodebExcel> eNodebInfoList, ITownRepository townRepository)
        {
            if (CurrentProgress > 10) { CurrentProgress = 0; }
            return CurrentProgress++; 
        }

        public bool RemoveOneENodeb(ENodeb eNodeb)
        { return true; }

        public bool DeleteENodeb(int eNodebId)
        { return true; }

        public bool DeleteENodeb(ITownRepository townRepository,
            string cityName, string districtName, string townName, string eNodebName)
        { return true; }

        public void SaveChanges()
        { }
    }
}
