using System;

namespace Lte.Domain.Regular
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ColumnAttribute : Attribute
    {
        public const int McDefaultFieldIndex = Int32.MaxValue;

        public string Name { get; set; }

        public bool CanBeNull { get; set; }

        public int FieldIndex { get; set; }

        public string DateTimeFormat { get; set; }

        public ColumnAttribute()
        {
            Name = "";

            FieldIndex = McDefaultFieldIndex;

            CanBeNull = true;
        }

        public ColumnAttribute(string name, int fieldIndex, bool canBeNull)
        {
            Name = name;

            FieldIndex = fieldIndex;

            CanBeNull = canBeNull;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RowAttribute : Attribute
    {
        public char InterColumnSplitter { get; set; }

        public char IntraColumnSplitter { get; set; }

        public RowAttribute()
        {
            InterColumnSplitter = ',';
            IntraColumnSplitter = '=';
        }
    }

    [Row(InterColumnSplitter = ',', IntraColumnSplitter = '=')]
    public class ZteSignal
    {
        [Column(Name = "Sequence")]
        public int Sequence { get; set; }

        [Column(Name = "Time", DateTimeFormat = "yyyy-MM-dd HH:mm:ss:fff")]
        public DateTime Time { get; set; }

        [Column(Name = "MsgType")]
        public string MsgType { get; set; }

        [Column(Name = "MsgName")]
        public string MsgName { get; set; }

        [Column(Name = "Direction")]
        public string Direction { get; set; }

        [Column(Name = "eNodeBId")]
        public int ENodebId { get; set; }

        [Column(Name = "Cell")]
        public byte CellId { get; set; }

        [Column(Name = "GID")]
        public int Gid { get; set; }

        [Column(Name = "MsgId")]
        public int MsgId { get; set; }

        [Column(Name = "DataId")]
        public byte DataId { get; set; }

        [Column(Name = "MsgCode")]
        public string MsgCode { get; set; }
    }
}
