using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.LinqToCsv.Description;

namespace Lte.Domain.LinqToCsv.Mapper
{
    public class FieldIndexInfo
    {
        // IndexToInfo is used to quickly translate the index of a field
        // to its TypeFieldInfo.
        private readonly TypeFieldInfo[] _indexToInfo;

        public TypeFieldInfo[] IndexToInfo
        {
            get { return _indexToInfo; }
        }

        /// <summary>
        /// Contains a mapping between the CSV column indexes that will read and the property indexes in the business object.
        /// </summary>
        protected IDictionary<int, int> MappingIndexes = new Dictionary<int, int>();

        protected Dictionary<string, TypeFieldInfo> NameToInfo;

        public FieldIndexInfo(Dictionary<string, TypeFieldInfo> nameToInfo)
        {
            NameToInfo = nameToInfo;
            int nbrTypeFields = nameToInfo.Keys.Count;
            _indexToInfo = new TypeFieldInfo[nbrTypeFields];
            MappingIndexes = new Dictionary<int, int>();

            int i = 0;
            foreach (KeyValuePair<string, TypeFieldInfo> kvp in NameToInfo)
            {
                _indexToInfo[i++] = kvp.Value;
            }

            // Sort by FieldIndex. Fields without FieldIndex will 
            // be sorted towards the back, because their FieldIndex
            // is Int32.MaxValue.
            //
            // The sort order is important when reading a file that 
            // doesn't have the field names in the first line, and when
            // writing a file. 
            //
            // Note that for reading from a file with field names in the 
            // first line, method ReadNames reworks IndexToInfo.
            Array.Sort(_indexToInfo);
        }

        public void AddMappingIndex(int i, int currentNameIndex)
        {
            MappingIndexes.Add(i, currentNameIndex);
        }

        public void UpdateIndexToInfo<T>(IDataRow row, Dictionary<string, TypeFieldInfo> nameToInfo,
            bool enforceCsvColumnAttribute, string fileName)
        {
            for (int i = 0; i < row.Count; i++)
            {
                if (!MappingIndexes.ContainsKey(i))
                {
                    continue;
                }

                _indexToInfo[MappingIndexes[i]] = nameToInfo[row[i].Value];
                if (enforceCsvColumnAttribute && (!_indexToInfo[i].HasColumnAttribute))
                {
                    // enforcing column attr, but this field/prop has no column attr.
                    throw new MissingCsvColumnAttributeException(typeof(T).ToString(), row[i].Value, fileName);
                }
            }
        }

        public TypeFieldInfo QueryTypeFieldInfo(bool ignoreUnknownColumns, int i)
        {
            //If there is some index mapping generated and the IgnoreUnknownColums is `true`
            if (ignoreUnknownColumns && MappingIndexes.Count > 0)
            {
                if (!MappingIndexes.ContainsKey(i))
                {
                    return null;
                }
                return _indexToInfo[MappingIndexes[i]];
            }
            return _indexToInfo[i];
        }

        public List<int> GetCharLengthList()
        {
            return _indexToInfo.Select(e => e.CharLength).ToList();
        }

        public int GetMaxRowCount(int defaultRowCount)
        {
            return MappingIndexes.Count > 0 ? defaultRowCount : Math.Min(defaultRowCount, _indexToInfo.Length);
        }
    }
}
