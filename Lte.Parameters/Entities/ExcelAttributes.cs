using System;

namespace Lte.Parameters.Entities
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class SimpleExcelColumnAttribute : Attribute
    {
        public string Name { get; set; }

        public string DefaultValue { get; set; }

        public SimpleExcelColumnAttribute()
        {
            DefaultValue = "1";
        }
    }

    public class LteExcelColumnAttribute : SimpleExcelColumnAttribute
    {
        public LteExcelColumnAttribute()
            : base()
        { }
    }

    public class CdmaExcelColumnAttribute : SimpleExcelColumnAttribute
    {
        public CdmaExcelColumnAttribute()
            : base()
        { }
    }
}
