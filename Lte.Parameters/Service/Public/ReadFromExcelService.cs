using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Lte.Domain.Regular;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Service.Public
{
    public class ExcelColumnProperty
    {
        public PropertyInfo Property { get; set; }

        public string AttributeName { private get; set; }

        public string DefaultValue { get; set; }

        public string FieldContent { get; private set; }

        public void UpdateFieldContent(IDataReader tableReader)
        {
            FieldContent = tableReader.GetField(AttributeName);
        }
    }

    public class ReadExcelTableService<TValue, TAtttribute> : ReadFromExcelService<TValue, TAtttribute>
        where TAtttribute : SimpleExcelColumnAttribute
    {
        public ReadExcelTableService(TValue record) : base(record)
        {
            PropertyInfo[] properties = (typeof(TValue)).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                SimpleExcelColumnAttribute excelColumnAttribute = GetExcelColumnAttribute(property);
                if (excelColumnAttribute == null) continue;
                _columnProperties.Add(new ExcelColumnProperty
                {
                    AttributeName = excelColumnAttribute.Name,
                    DefaultValue = excelColumnAttribute.DefaultValue,
                    Property = property
                });
            }
        }

        public void Import(IDataReader tableReader)
        {
            foreach (ExcelColumnProperty property in _columnProperties)
            {
                property.UpdateFieldContent(tableReader);
                SetPropertyValue(property.Property, property.FieldContent, property.DefaultValue);
            }
        }
    }

    public class ReadExcelValueService<TValue, TAtttribute> : ReadFromExcelService<TValue, TAtttribute>
        where TAtttribute : SimpleExcelColumnAttribute
    {
        public ReadExcelValueService(TValue record, IDataReader tableReader) 
            : base(record)
        {
            PropertyInfo[] properties = (typeof(TValue)).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                SimpleExcelColumnAttribute excelColumnAttribute = GetExcelColumnAttribute(property);
                if (excelColumnAttribute == null) continue;
                ExcelColumnProperty columnProperty = new ExcelColumnProperty
                {
                    AttributeName = excelColumnAttribute.Name,
                    DefaultValue = excelColumnAttribute.DefaultValue,
                    Property = property
                };
                columnProperty.UpdateFieldContent(tableReader);
                _columnProperties.Add(columnProperty);
            }
        }

        public void Import()
        {
            foreach (ExcelColumnProperty property in _columnProperties)
            {
                SetPropertyValue(property.Property, property.FieldContent, property.DefaultValue);
            }
        }
    }

    public abstract class ReadFromExcelService<TValue, TAtttribute>
        where TAtttribute : SimpleExcelColumnAttribute
    {
        private readonly TValue _record;
        protected readonly List<ExcelColumnProperty> _columnProperties = new List<ExcelColumnProperty>();

        protected ReadFromExcelService(TValue record)
        {
            _record = record;
        }

        protected SimpleExcelColumnAttribute GetExcelColumnAttribute(PropertyInfo property)
        {
            Attribute attribute = Attribute.GetCustomAttribute(property, typeof(TAtttribute));
            if (attribute == null) return null;
            return attribute as SimpleExcelColumnAttribute;
        }

        protected void SetPropertyValue(PropertyInfo property, string fieldContent, string defaultValue)
        {
            switch (property.PropertyType.Name)
            {
                case "Int64":
                    property.SetValue(_record, fieldContent.ConvertToLong(
                        long.Parse(defaultValue)));
                    break;
                case "Int32":
                    property.SetValue(_record, fieldContent.ConvertToInt(
                        int.Parse(defaultValue)));
                    break;
                case "Int16":
                    property.SetValue(_record, fieldContent.ConvertToShort(
                        short.Parse(defaultValue)));
                    break;
                case "Byte":
                    property.SetValue(_record, fieldContent.ConvertToByte(
                        byte.Parse(defaultValue)));
                    break;
                case "Double":
                    property.SetValue(_record, fieldContent.ConvertToDouble(
                        double.Parse(defaultValue)));
                    break;
                case "DateTime":
                    property.SetValue(_record, fieldContent.ConvertToDateTime(
                        DateTime.Today));
                    break;
                case "String":
                    property.SetValue(_record, fieldContent);
                    break;
            }
        }
    }
}
