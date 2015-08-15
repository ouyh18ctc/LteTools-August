using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Entities;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Service;
using Lte.Domain.TypeDefs;
using Lte.Domain.Regular;
using System.ComponentModel.DataAnnotations;
using Lte.Parameters.Abstract;

namespace Lte.Parameters.Entities
{
    public class ENodebBase : Entity, ITownId
    {
        [Display(Name = "LTE基站编号")]
        public int ENodebId { get; set; }

        [Display(Name = "基站名称")]
        [MaxLength(50)]
        public string Name { get; set; }

        public int TownId { get; set; }

        public ENodebBase()
        {
            ENodebId = -1;
            Name = "";
        }
    }

    public class ENodeb : ENodebBase, IGeoPoint<double>, IGeoPointReadonly<double>
    {
        [Display(Name = "经度")]
        public double Longtitute { get; set; }

        [Display(Name = "纬度")]
        public double Lattitute { get; set; }

        public double BaiduLongtitute
        { get { return Longtitute + GeoMath.BaiduLongtituteOffset; } }

        public double BaiduLattitute
        { get { return Lattitute + GeoMath.BaiduLattituteOffset; } }

        [Display(Name = "厂家")]
        public string Factory { get; set; }

        public bool IsFdd { get; set; }

        [Display(Name = "地址")]
        public string Address { get; set; }

        public int Gateway { get; set; }

        public byte SubIp { get; set; }

        public IpAddress GatewayIp
        {
            get
            {
                return new IpAddress
                {
                    AddressValue = Gateway
                };
            }
        }

        public IpAddress Ip 
        {
            get 
            {
                IpAddress ip = new IpAddress {AddressValue = Gateway, IpByte4 = SubIp};
                return ip;
            }
        }

        [Display(Name = "规划编号")]
        public string PlanNum { get; set; }

        [Display(Name = "入网日期")]
        public DateTime OpenDate { get; set; }

        public void Import(ENodebExcel eNodebExcelInfo, int townId = -1, bool updateId = true)
        {
            int oldId = ENodebId;
            eNodebExcelInfo.CloneProperties(this);
            if (!updateId) { ENodebId = oldId; }
            IsFdd = (eNodebExcelInfo.DivisionDuplex.IndexOf("FDD", StringComparison.Ordinal) >= 0);
            Gateway = eNodebExcelInfo.Gateway.AddressValue;
            SubIp = eNodebExcelInfo.Ip.IpByte4;
            TownId = townId;
        }
    }

    public class ENodebPhoto : Entity
    {
        public int ENodebId { get; set; }

        public byte SectorId { get; set; }

        public short Angle { get; set; }

        public string Path { get; set; }
    }

    public class CdmaBts : ENodebBase, IGeoPoint<double>, IExcelImportable<BtsExcel>
    {
        [Display(Name = "经度")]
        public double Longtitute { get; set; }

        [Display(Name = "纬度")]
        public double Lattitute { get; set; }

        public double BaiduLongtitute
        { get { return Longtitute + GeoMath.BaiduLongtituteOffset; } }

        public double BaiduLattitute
        { get { return Lattitute + GeoMath.BaiduLattituteOffset; } }

        [Display(Name = "地址")]
        public string Address { get; set; }

        [Display(Name = "CDMA基站编号")]
        public int BtsId { get; set; }

        [Display(Name = "BSC编号")]
        public short BscId { get; set; }

        private const double Eps = 1E-7;

        public CdmaBts()
        {
            Longtitute = 0;
            Lattitute = 0;
            Address = "";
            BtsId = -1;
            BscId = -1;
        }

        public void Import(BtsExcel btsExcelInfo, bool updateName)
        {
            if (Math.Abs(Longtitute) < Eps && Math.Abs(Lattitute) < Eps)
            {
                string oldName = Name;
                btsExcelInfo.CloneProperties(this);
                if (!updateName)
                {
                    Name = oldName;
                }
            }

        }

        public void ImportLteInfo(IEnumerable<ENodeb> lteRepository)
        {
            if (lteRepository != null && ENodebId == -1)
            {
                ENodeb eNodeb = lteRepository.FirstOrDefault(
                    x => x.TownId == TownId && x.Name == Name);
                if (eNodeb != null &&
                    eNodeb.Distance(this) < 0.05)
                {
                    ENodebId = eNodeb.ENodebId;
                }
            }
        }
    }
}
