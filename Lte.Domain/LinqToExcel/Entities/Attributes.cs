using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lte.Domain.LinqToExcel.Entities
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class ExcelColumnAttribute : Attribute
    {
        private readonly string _columnName;

        public ExcelColumnAttribute(string columnName)
        {
            _columnName = columnName;
        }

        public string ColumnName
        {
            get { return _columnName; }
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class CloneProtectionAttribute : Attribute
    {
        public bool Protection { get; set; }

        public CloneProtectionAttribute()
        {
            Protection = true;
        }
    }
}
