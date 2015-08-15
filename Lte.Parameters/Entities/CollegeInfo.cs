using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;

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
}
