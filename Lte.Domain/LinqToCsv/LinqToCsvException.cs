using System;
using System.Collections.Generic;

namespace Lte.Domain.LinqToCsv
{
    [Serializable]
    public class LinqToCsvException : Exception
    {
        protected LinqToCsvException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected LinqToCsvException(string message)
            : base(message)
        {
        }

        protected static string FileNameMessage(string fileName)
        {
            return ((fileName == null) ? "" : " Reading file \"" + fileName + "\".");
        }
    }

    public sealed class AggregatedException : LinqToCsvException
    {
        public List<Exception> m_InnerExceptionsList;
        private int m_MaximumNbrExceptions = 100;

        public AggregatedException(string typeName, string fileName, int maximumNbrExceptions) :
            base(string.Format(
                 "There were 1 or more exceptions while reading data using type \"{0}\"." +
                 FileNameMessage(fileName), typeName))
        {
            m_MaximumNbrExceptions = maximumNbrExceptions;
            m_InnerExceptionsList = new List<Exception>();


            Data["TypeName"] = typeName;
            Data["FileName"] = fileName;
            Data["InnerExceptionsList"] = m_InnerExceptionsList;
        }

        public void AddException(Exception e)
        {
            m_InnerExceptionsList.Add(e);
            if ((m_MaximumNbrExceptions != -1) &&
                (m_InnerExceptionsList.Count >= m_MaximumNbrExceptions))
            {
                throw this;
            }
        }

        public void ThrowIfExceptionsStored()
        {
            if (m_InnerExceptionsList.Count > 0)
            {
                throw this;
            }
        }
    }

    public class BadStreamException : LinqToCsvException
    {
        public BadStreamException() :
            base("Stream provided to Read is either null, or does not support Seek.")
        {
        }
    }

    public class CsvColumnAttributeRequiredException : LinqToCsvException
    {
        public CsvColumnAttributeRequiredException() :
            base(
                "CsvFileDescription.EnforceCsvColumnAttribute is false, but needs to be true because " +
                "CsvFileDescription.FirstLineHasColumnNames is false. See the description for CsvColumnAttributeRequiredException.")
        {
        }
    }

    public sealed class DuplicateFieldIndexException : LinqToCsvException
    {
        public DuplicateFieldIndexException(string typeName, string fieldName, string fieldName2, int duplicateIndex) :
            base(string.Format(
                "Fields or properties \"{0}\" and \"{1}\" of type \"{2}\" have duplicate FieldIndex {3}.",
                fieldName, fieldName2, typeName, duplicateIndex))
        {
            Data["TypeName"] = typeName;
            Data["FieldName"] = fieldName;
            Data["FieldName2"] = fieldName2;
            Data["Index"] = duplicateIndex;
        }
    }

    public sealed class MissingCsvColumnAttributeException : LinqToCsvException
    {
        public MissingCsvColumnAttributeException(string typeName, string fieldName, string fileName) :
            base(string.Format(
                     "Field \"{0}\" in type \"{1}\" does not have the CsvColumn attribute." +
                     FileNameMessage(fileName), fieldName, typeName))
        {
            Data["TypeName"] = typeName;
            Data["FieldName"] = fieldName;
            Data["FileName"] = fileName;
        }
    }

    public sealed class MissingFieldIndexException : LinqToCsvException
    {
        public MissingFieldIndexException(string typeName, int lineNbr, string fileName) :
            base(string.Format(
                 "Line {0} has more fields then there are fields or properties in type \"{1}\" with a FieldIndex." +
                 FileNameMessage(fileName), lineNbr, typeName))
        {
            Data["TypeName"] = typeName;
            Data["LineNbr"] = lineNbr;
            Data["FileName"] = fileName;
        }
    }

    public sealed class MissingRequiredFieldException : LinqToCsvException
    {
        public MissingRequiredFieldException(string typeName, string fieldName, int lineNbr, string fileName) :
            base(
                 string.Format(
                     "In line {0}, no value provided for required field or property \"{1}\" in type \"{2}\"." +
                     FileNameMessage(fileName), lineNbr, fieldName, typeName))
        {
            Data["TypeName"] = typeName;
            Data["LineNbr"] = lineNbr;
            Data["FileName"] = fileName;
            Data["FieldName"] = fieldName;
        }
    }

    public sealed class NameNotInTypeException : LinqToCsvException
    {
        public NameNotInTypeException(string typeName, string fieldName, string fileName) :
            base(string.Format(
                    "The input file has column name \"{0}\" in the first record, but there is no field or property with that name in type \"{1}\"." +
                    FileNameMessage(fileName), fieldName, typeName))
        {
            Data["TypeName"] = typeName;
            Data["FieldName"] = fieldName;
            Data["FileName"] = fileName;
        }
    }

    public sealed class RequiredButMissingFieldIndexException : LinqToCsvException
    {
        public RequiredButMissingFieldIndexException(string typeName, string fieldName) :
            base(string.Format(
                "Field or property \"{0}\" of type \"{1}\" is required, but does not have a FieldIndex. " +
                "This exception only happens for files without column names in the first record.",
                fieldName, typeName))
        {
            Data["TypeName"] = typeName;
            Data["FieldName"] = fieldName;
        }
    }

    public sealed class ToBeWrittenButMissingFieldIndexException : LinqToCsvException
    {
        public ToBeWrittenButMissingFieldIndexException(string typeName, string fieldName) :
            base(string.Format(
                "Field or property \"{0}\" of type \"{1}\" will be written to a file, but does not have a FieldIndex. " +
                "This exception only happens for input files without column names in the first record.",
                fieldName, typeName))
        {
            Data["TypeName"] = typeName;
            Data["FieldName"] = fieldName;
        }
    }

    public sealed class TooManyDataFieldsException : LinqToCsvException
    {
        public TooManyDataFieldsException(string typeName, int lineNbr, string fileName) :
            base(string.Format(
                     "Line {0} has more fields then are available in type \"{1}\"." +
                     FileNameMessage(fileName), lineNbr, typeName))
        {
            Data["TypeName"] = typeName;
            Data["LineNbr"] = lineNbr;
            Data["FileName"] = fileName;
        }
    }

    public sealed class TooManyNonCsvColumnDataFieldsException : LinqToCsvException
    {
        public TooManyNonCsvColumnDataFieldsException(string typeName, int lineNbr, string fileName) :
            base(string.Format(
                     "Line {0} has more fields then there are fields or properties in type \"{1}\" with the CsvColumn attribute set." +
                     FileNameMessage(fileName), lineNbr, typeName))
        {
            Data["TypeName"] = typeName;
            Data["LineNbr"] = lineNbr;
            Data["FileName"] = fileName;
        }
    }

    public sealed class WrongDataFormatException : LinqToCsvException
    {
        public WrongDataFormatException(string typeName, string fieldName, string fieldValue, int lineNbr, 
            string fileName, Exception innerExc) :
            base(
                 string.Format(
                     "Value \"{0}\" in line {1} has the wrong format for field or property \"{2}\" in type \"{3}\"." +
                     FileNameMessage(fileName), fieldValue, lineNbr, fieldName, typeName), innerExc)
        {
            Data["TypeName"] = typeName;
            Data["LineNbr"] = lineNbr;
            Data["FileName"] = fileName;
            Data["FieldValue"] = fieldValue;
            Data["FieldName"] = fieldName;
        }
    }

    public sealed class WrongFieldIndexException : LinqToCsvException
    {
        public WrongFieldIndexException(string typeName, int lineNbr, string fileName) :
            base(string.Format(
                 "Line {0} has less fields then the FieldIndex value is indicating in type \"{1}\" ." +
                 FileNameMessage(fileName), lineNbr, typeName))
        {
            Data["TypeName"] = typeName;
            Data["LineNbr"] = lineNbr;
            Data["FileName"] = fileName;
        }
    }
}
