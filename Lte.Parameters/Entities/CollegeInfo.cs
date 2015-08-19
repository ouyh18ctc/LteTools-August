using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Abp.Domain.Entities.Auditing;
using Lte.Parameters.Abstract;

namespace Lte.Parameters.Entities
{
    public class CollegeInfo : AuditedEntity
    {
        public int TownId { get; set; }

        public string Name { get; set; }

        public int TotalStudents { get; set; }

        public int CurrentSubscribers { get; set; }

        public int GraduateStudents { get; set; }

        public int NewSubscribers { get; set; }

        public DateTime OldOpenDate { get; set; }

        public DateTime NewOpenDate { get; set; }

        public int ExpectedSubscribers
        {
            get { return CurrentSubscribers + NewSubscribers - GraduateStudents; }
        }

        public CollegeRegion CollegeRegion { get; set; }
    }

    public class CollegeRegion
    {
        [Key]
        public int AreaId { get; set; }

        public double Area { get; set; }

        public RegionType RegionType { get; set; }

        public string Info { get; set; }
    }

    public class CollegeStat
    {
        public int Id { get; set; }

        public string Name { get; private set; }

        public int ExpectedSubscribers { get; private set; }

        public double Area { get; private set; }

        public int TotalLteENodebs { get; set; }

        public int TotalLteCells { get; set; }

        public int TotalCdmaBts { get; set; }

        public int TotalCdmaCells { get; set; }

        public int TotalLteIndoors { get; set; }
        
        public int TotalCdmaIndoors { get; set; }

        public CollegeStat(ICollegeRepository repository, int id)
        {
            CollegeInfo info = repository.Get(id);
            CollegeRegion region = repository.GetRegion(id);
            Name = info.Name;
            ExpectedSubscribers = info.ExpectedSubscribers;
            Area = region.Area;
            Id = id;
        }

        public void UpdateStats(IInfrastructureRepository repository)
        {
            TotalLteENodebs = repository.InfrastructureInfos.Count(x => x.HotspotName == Name
                                                                        && x.HotspotType == HotspotType.College
                                                                        &&
                                                                        x.InfrastructureType ==
                                                                        InfrastructureType.ENodeb);
            TotalLteCells = repository.InfrastructureInfos.Count(x => x.HotspotName == Name
                                                                      && x.HotspotType == HotspotType.College
                                                                      && x.InfrastructureType == InfrastructureType.Cell);
            TotalCdmaBts = repository.InfrastructureInfos.Count(x => x.HotspotName == Name
                                                                     && x.HotspotType == HotspotType.College
                                                                     &&
                                                                     x.InfrastructureType == InfrastructureType.CdmaBts);
            TotalCdmaCells = repository.InfrastructureInfos.Count(x => x.HotspotName == Name
                                                                       && x.HotspotType == HotspotType.College
                                                                       &&
                                                                       x.InfrastructureType ==
                                                                       InfrastructureType.CdmaCell);
            TotalLteIndoors = repository.InfrastructureInfos.Count(x => x.HotspotName == Name
                                                                        && x.HotspotType == HotspotType.College
                                                                        &&
                                                                        x.InfrastructureType ==
                                                                        InfrastructureType.LteIndoor);
            TotalCdmaIndoors = repository.InfrastructureInfos.Count(x => x.HotspotName == Name
                                                                         && x.HotspotType == HotspotType.College
                                                                         &&
                                                                         x.InfrastructureType ==
                                                                         InfrastructureType.CdmaIndoor);
        }
    }
}
