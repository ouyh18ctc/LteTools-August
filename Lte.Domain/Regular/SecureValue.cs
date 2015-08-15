using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Reflection;
using Lte.Domain.LinqToExcel.Entities;

namespace Lte.Domain.Regular
{
    public static class SecureValue
    {
        public static T GetObject<T, TI>(this TI source) where T : class, TI
        {
            if (source is T)
            { return source as T; }
            throw new TypeAccessException("Error in setting value, it's type is: " + source.GetType()
                                          + ", but is expected to be: " + typeof(T) + "!");
        }

        public static string GetField(this IDataReader dataReader, string fieldName)
        {
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                if (dataReader.GetName(i).Trim() == fieldName)
                {
                    return dataReader.GetValue(i).ToString().Trim();
                }
            }
            return "";
        }

        public static void ResetProperties<T>(this T source)
        {
            PropertyInfo[] properties = (typeof(T)).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.CanWrite)
                {
                    int zero32 = 0;
                    short zero16 = 0;
                    if (property.PropertyType.Name == "DateTime")
                    {
                        property.SetValue(source, DateTime.Now, null);
                    }
                    else if (property.PropertyType.Name == "Int16")
                    {
                        property.SetValue(source, zero16, null);
                    }
                    else
                    {
                        property.SetValue(source, zero32, null);
                    }
                }
            }
        }

        public static void Increase<T>(this T source, T increment)
        { 
            PropertyInfo[] properties = (typeof(T)).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.CanWrite)
                {
                    if (property.PropertyType.Name == "Int16")
                    {
                        short value16 
                            = (short)((short)property.GetValue(increment, null) + (short)property.GetValue(source, null));
                        property.SetValue(source, value16, null);
                    }
                    else if (property.PropertyType.Name == "Int32")
                    {
                        property.SetValue(source,
                            (int)property.GetValue(increment, null) + (int)property.GetValue(source, null),
                            null);
                    }
                    else if (property.PropertyType.Name == "Int64")
                    {
                        property.SetValue(source,
                            (long)property.GetValue(increment, null) + (long)property.GetValue(source, null),
                            null);
                    }
                    else if (property.PropertyType.Name == "Double")
                    {
                        property.SetValue(source,
                            (double)property.GetValue(increment, null) + (double)property.GetValue(source, null),
                            null);
                    }
                    
                }
            }
        }

        public static void DividedBy<T>(this T source, int scalor)
        {
            PropertyInfo[] properties = (typeof(T)).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.CanWrite)
                {
                    if (property.PropertyType.Name == "Int16")
                    {
                        short value16 = (short)((short)property.GetValue(source, null) / scalor);
                        property.SetValue(source, value16, null);
                    }
                    else if (property.PropertyType.Name == "Int32")
                    {
                        property.SetValue(source, (int)property.GetValue(source, null) / scalor, null);
                    }
                    else if (property.PropertyType.Name == "Int64")
                    {
                        property.SetValue(source, (long)property.GetValue(source, null) / scalor, null);
                    }
                    else if (property.PropertyType.Name == "Double")
                    {
                        property.SetValue(source, (double)property.GetValue(source, null) / scalor, null);
                    }
                }
            }
        }

        public static T Average<T>(this IEnumerable<T> sourceList)
            where T : class, new()
        {
            IEnumerable<T> enumerable = sourceList as T[] ?? sourceList.ToArray();
            T result = enumerable.ArraySum();
            result.DividedBy(enumerable.Count());
            return result;
        }

        public static T ArraySum<T>(this IEnumerable<T> sourceList) where T : class, new()
        {
            T result = new T();
            IEnumerable<T> enumerable = sourceList as T[] ?? sourceList.ToArray();
            enumerable.ElementAt(0).CloneDateTimeValue(result);
            foreach (T item in enumerable)
            {
                result.Increase(item);
            }
            return result;
        }

        private static void CloneDateTimeValue<T>(this T source, T destination)
            where T : class, new()
        {
            PropertyInfo[] properties = (typeof(T)).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (!(property.CanRead && property.CanWrite)) { continue; }
                if (property.PropertyType.Name == "DateTime")
                {
                    property.SetValue(destination, property.GetValue(source, null), null);
                }
            }
        }

        public static void CloneProperties<T>(this T source, T destination, bool ignoreId = true)
            where T : class
        {
            PropertyInfo[] properties = (typeof(T)).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (!(property.CanRead && property.CanWrite)) { continue; }
                if (!(property.Name == "Id" && ignoreId))
                {
                    property.SetValue(destination, property.GetValue(source, null), null);
                }
            }
        }

        public static void CloneProperties<TSource, TDest>(this TSource source, TDest dest, bool protectionConsidered = false)
            where TSource : class, new()
            where TDest : class, new()
        {
            PropertyInfo[] srcProperties = (typeof(TSource)).GetProperties();
            PropertyInfo[] destProperties = (typeof(TDest)).GetProperties();

            foreach (PropertyInfo srcProperty in srcProperties)
            {
                if (!srcProperty.CanRead) { continue; }
                if (protectionConsidered)
                {
                    CloneProtectionAttribute cfa = (CloneProtectionAttribute)Attribute.GetCustomAttribute(
                        srcProperty, typeof(CloneProtectionAttribute));
                    if (cfa != null) { continue; }
                }
                foreach (PropertyInfo destProperty in destProperties)
                {
                    if (destProperty.Name == srcProperty.Name && destProperty.CanWrite
                        && (destProperty.PropertyType == srcProperty.PropertyType
                        || (destProperty.PropertyType.Name == "Int64"
                        && (srcProperty.PropertyType.Name == "Int32"
                        || srcProperty.PropertyType.Name == "Int16"))))
                    {
                        destProperty.SetValue(dest, srcProperty.GetValue(source, null), null);
                    }
                }
            }
        }

        public static void ConvertProperties<TString, TValue>(this TString source, TValue dest)
        { 
            PropertyInfo[] srcProperties = (typeof(TString)).GetProperties();
            PropertyInfo[] destProperties = (typeof(TValue)).GetProperties();

            foreach (PropertyInfo srcProperty in srcProperties.Where(x => x.PropertyType.Name == "String"))
            {
                if (!srcProperty.CanRead) { continue; }
                string propertyName = srcProperty.Name;
                foreach (PropertyInfo destProperty in destProperties.Where(
                    x => (x.Name == propertyName) && x.CanWrite))
                {
                    string typeName = destProperty.PropertyType.Name;
                    switch (typeName)
                    { 
                        case "Byte":
                            destProperty.SetValue(dest, srcProperty.GetValue(source).ToString().ConvertToByte(0));
                            break;
                        case "Int16":
                            destProperty.SetValue(dest, srcProperty.GetValue(source).ToString().ConvertToShort(0));
                            break;
                        case "Int32":
                            destProperty.SetValue(dest, srcProperty.GetValue(source).ToString().ConvertToInt(0));
                            break;
                        case "Int64":
                            destProperty.SetValue(dest, srcProperty.GetValue(source).ToString().ConvertToLong(0));
                            break;
                        case "Double":
                            destProperty.SetValue(dest, srcProperty.GetValue(source).ToString().ConvertToDouble(0));
                            break;
                    }
                }
            }
        }
    }
}
