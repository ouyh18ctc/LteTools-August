using System;
using Lte.Domain.LinqToExcel.Entities;

namespace Lte.Domain.Test.LinqToExcel
{
    public class Company
    {
        public string Name { get; set; }

        public string CEO { get; set; }

        public int EmployeeCount { get; set; }

        public DateTime StartDate { get; set; }

        public bool IsActive { get; set; }
    }

    public class CompanyNullable
    {
        public string Name { get; set; }

        public string CEO { get; set; }

        public int? EmployeeCount { get; set; }

        public DateTime? StartDate { get; set; }
    }

    class CompanyWithCity : Company
    {
        public string City { get; set; }
    }

    public class CompanyWithColumnAnnotations
    {
        [ExcelColumn("Company Title")]
        public string Name { get; set; }

        [ExcelColumn("Boss")]
        public string CEO { get; set; }

        [ExcelColumn("Number of People")]
        public int EmployeeCount { get; set; }

        [ExcelColumn("Initiation Date")]
        public DateTime StartDate { get; set; }

        [ExcelColumn("Active")]
        public string IsActive { get; set; }
    }
}
