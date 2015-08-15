using System;
using System.Collections.Generic;
using System.Reflection;
using Lte.Domain.LinqToCsv.Description;

namespace Lte.Domain.LinqToCsv.Mapper
{
    public class FieldMapper<T>
    {
        protected FieldIndexInfo fieldIndexInfo;

        public FieldIndexInfo FieldIndexInfo
        {
            get { return fieldIndexInfo; }
        }

        protected Dictionary<string, TypeFieldInfo> nameToInfo = null;

        public Dictionary<string, TypeFieldInfo> NameToInfo
        {
            get { return nameToInfo; }
        }

        protected CsvFileDescription FileDescription;

        // Only used when throwing an exception
        protected string FileName;

        private TypeFieldInfo AnalyzeTypeField(
                                MemberInfo mi,
                                bool allRequiredFieldsMustHaveFieldIndex,
                                bool allCsvColumnFieldsMustHaveFieldIndex)
        {
            TypeFieldInfo tfi = new TypeFieldInfo();
            tfi.MemberInfo = mi;
            tfi.UpdateParseParameters(FileDescription.UseOutputFormatForParsingCsvValue);
            tfi.UpdateAttributes();
            tfi.ValidateAttributes<T>(allCsvColumnFieldsMustHaveFieldIndex, allRequiredFieldsMustHaveFieldIndex);

            return tfi;
        }

        protected void AnalyzeType(
                        Type type,
                        bool allRequiredFieldsMustHaveFieldIndex,
                        bool allCsvColumnFieldsMustHaveFieldIndex)
        {  
            InitializeNameToInfo(type, allRequiredFieldsMustHaveFieldIndex, allCsvColumnFieldsMustHaveFieldIndex);

            // -------
            // Initialize IndexToInfo
            fieldIndexInfo = new FieldIndexInfo(nameToInfo);

            // ----------
            // Make sure there are no duplicate FieldIndices.
            // However, allow gaps in the FieldIndex range, to make it easier to later insert
            // fields in the range.
            int lastFieldIndex = Int32.MinValue;
            string lastName = "";
            foreach (TypeFieldInfo tfi in fieldIndexInfo.IndexToInfo)
            {
                lastName = tfi.UpdateLastName<T>(lastName, ref lastFieldIndex);
            }
        }

        private void InitializeNameToInfo(Type type, bool allRequiredFieldsMustHaveFieldIndex, bool allCsvColumnFieldsMustHaveFieldIndex)
        {
            nameToInfo.Clear();

            foreach (MemberInfo mi in type.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            {
                // Only process field and property members.
                if ((mi.MemberType == MemberTypes.Field) ||
                    (mi.MemberType == MemberTypes.Property))
                {
                    // Note that the compiler does not allow fields and/or properties
                    // with the same name as some other field or property.
                    TypeFieldInfo tfi =
                        AnalyzeTypeField(mi,
                                allRequiredFieldsMustHaveFieldIndex,
                                allCsvColumnFieldsMustHaveFieldIndex);
                    nameToInfo[tfi.Name] = tfi;
                }
            }
        }

        public FieldMapper(CsvFileDescription fileDescription, string fileName, bool writingFile)
        {
            if ((!fileDescription.FirstLineHasColumnNames) &&
                (!fileDescription.EnforceCsvColumnAttribute))
            {
                throw new CsvColumnAttributeRequiredException();
            }

            FileDescription = fileDescription;
            FileName = fileName;
            nameToInfo = new Dictionary<string, TypeFieldInfo>();

            AnalyzeType(
                typeof(T),
                !fileDescription.FirstLineHasColumnNames,
                writingFile && !fileDescription.FirstLineHasColumnNames);
        }

        /// <summary>
        /// Writes the field names given in T to row.
        /// </summary>
        /// 
        public void WriteNames(List<string> row)
        {
            row.Clear();

            for (int i = 0; i < fieldIndexInfo.IndexToInfo.Length; i++)
            {
                TypeFieldInfo tfi = fieldIndexInfo.IndexToInfo[i];

                if (FileDescription.EnforceCsvColumnAttribute &&
                        (!tfi.HasColumnAttribute))
                {
                    continue;
                }

                row.Add(tfi.Name);
            }
        }

        public void WriteObject(T obj, List<string> row)
        {
            row.Clear();
            
            for (int i = 0; i < fieldIndexInfo.IndexToInfo.Length; i++)
            {
                TypeFieldInfo tfi = fieldIndexInfo.IndexToInfo[i];

                if (FileDescription.EnforceCsvColumnAttribute &&
                        (!tfi.HasColumnAttribute))
                {
                    continue;
                }

                Object objValue;


                if (tfi.MemberInfo is PropertyInfo)
                {
                    objValue =
                        ((PropertyInfo)tfi.MemberInfo).GetValue(obj, null);
                }
                else
                {
                    objValue =
                        ((FieldInfo)tfi.MemberInfo).GetValue(obj);
                }

                string resultString = null;
                if (objValue != null)
                {
                    if ((objValue is IFormattable))
                    {
                        resultString =
                            ((IFormattable)objValue).ToString(
                                tfi.OutputFormat,
                                FileDescription.FileCultureInfo);
                    }
                    else
                    {
                        resultString = objValue.ToString();
                    }
                }

                row.Add(resultString);
            }
        }
    }

}
