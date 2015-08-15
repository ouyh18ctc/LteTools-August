using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lte.Domain.Regular;

namespace Lte.Domain.Test.Excel
{
    public class ColumnClass
    {
        [Column(Name = "First Field", CanBeNull = true)]
        public int FirstField { get; set; }

        [Column(Name = "Second Field", FieldIndex = 2)]
        public double SecondField { get; set; }

        public int NoAttributeField { get; set; }
    }
}
