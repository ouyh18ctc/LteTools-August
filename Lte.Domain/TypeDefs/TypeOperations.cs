using System;
using System.Collections.Generic;
using System.Reflection;
using System.Globalization;
using Lte.Domain.Regular;

namespace Lte.Domain.TypeDefs
{
    public static class TypeOperations
    {
        public static MethodInfo GetParseNumberMethod(this Type t)
        {
            return t.GetMethod("Parse",
                new[] { typeof(String), typeof(NumberStyles), typeof(IFormatProvider) });
        }

        public static MethodInfo GetParseMethod(this Type t)
        {
            return t.GetMethod("Parse", new[] { typeof(String) });
        }

        public static MethodInfo GetParseExactMethod(this Type t)
        {
            return t.GetMethod("ParseExact",
                new[] { typeof(string), typeof(string), typeof(IFormatProvider) });
        }

        public static Dictionary<string, string> GetFieldTextList<T>(this string line) where T : class
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            RowAttribute attribute = Attribute.GetCustomAttribute((typeof(T)), typeof(RowAttribute)) 
                as RowAttribute;
            if (attribute != null)
            {
                char interSplitter = attribute.InterColumnSplitter;
                char intraSplitter = attribute.IntraColumnSplitter;
                string[] fields = line.GetSplittedFields(interSplitter);
                foreach (string field in fields)
                {
                    string[] intraFields = field.GetSplittedFields(intraSplitter);
                    result.Add(intraFields[0], intraFields[1]);
                }
            }
            return result;
        }

        public static T GenerateOneRowFromText<T>(this string line) where T : class,new()
        {
            Dictionary<string, string> fieldTextList = line.GetFieldTextList<T>();
            PropertyInfo[] properties = (typeof(T)).GetProperties();
            T result = new T();
            foreach (PropertyInfo property in properties)
            {
                ColumnAttribute columnAttribute = 
                    Attribute.GetCustomAttribute(property, typeof(ColumnAttribute)) as ColumnAttribute;
                if (columnAttribute != null)
                {
                    string propertyName
                        = columnAttribute.Name;
                    string valueText = fieldTextList[propertyName];
                    result.SetValueByText(property, valueText);
                }
            }
            return result;
        }

        public static void SetValueByText<T>(this T result, PropertyInfo property, string valueText)
            where T : class, new()
        {
            Type propertyType = property.PropertyType;
            ColumnAttribute columnAttribute = Attribute.GetCustomAttribute(property, typeof(ColumnAttribute))
                as ColumnAttribute;
            if (columnAttribute != null)
            {
                string dateTimeFormat = columnAttribute.DateTimeFormat;
                property.SetValue(result,
                    (propertyType == typeof(string) ? valueText
                        : (propertyType == typeof(DateTime) && !string.IsNullOrEmpty(dateTimeFormat) ?
                            DateTime.ParseExact(valueText, dateTimeFormat, CultureInfo.CurrentCulture, DateTimeStyles.None)
                            : propertyType.GetParseMethod().Invoke(propertyType,
                                new object[] { valueText }))),
                    null);
            }
        }
    }
}
