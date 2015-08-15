using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Entities;
using Lte.Parameters.Abstract;

namespace Lte.Parameters.Concrete
{
    public class ENodebBaseRepository : IDisposable
    {
        private List<ENodebBase> eNodebBaseList = new List<ENodebBase>();

        public List<ENodebBase> ENodebBaseList
        {
            get { return eNodebBaseList; }
        }

        public ENodebBaseRepository(IENodebRepository inputRepository)
        {
            eNodebBaseList.Clear();
            foreach (ENodeb eNodeb in inputRepository.GetAllList())
            {                
                eNodebBaseList.Add(new ENodebBase
                {
                    ENodebId = eNodeb.ENodebId,
                    Name = eNodeb.Name,
                    TownId = eNodeb.TownId
                });
            }
        }

        public ENodebBaseRepository(IBtsRepository inputRepository)
        {
            eNodebBaseList.Clear();
            foreach (CdmaBts bts in inputRepository.GetAllList())
            {
                eNodebBaseList.Add(new ENodebBase
                {
                    ENodebId = bts.BtsId,
                    Name = bts.Name,
                    TownId = bts.TownId
                });
            }
        }

        public ENodebBase QueryENodeb(int eNodebId)
        {
            return eNodebBaseList.FirstOrDefault(x => x.ENodebId == eNodebId);
        }

        public ENodebBase QueryENodeb(int townId, string eNodebName)
        {
            return eNodebBaseList.FirstOrDefault(x => x.TownId == townId && x.Name == eNodebName);
        }

        public void ImportNewBtsInfo(BtsExcel btsInfo, int townId)
        {
            ENodebBase existedENodeb = eNodebBaseList.FirstOrDefault(x => x.ENodebId == btsInfo.BtsId);
            if (existedENodeb == null)
            {
                eNodebBaseList.Add(new ENodebBase
                {
                    ENodebId = btsInfo.BtsId,
                    Name = btsInfo.Name,
                    TownId = townId
                });
            }
        }

        public void Dispose()
        {
            eNodebBaseList = null;
        }
    }
}
