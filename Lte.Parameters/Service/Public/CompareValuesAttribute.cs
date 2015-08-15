using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Lte.Parameters.Service.Public
{
    /// <summary>
    /// Specifies that the field must compare favourably with the named field, if objects to check are not of the same type
    /// false will be return
    /// </summary>
    public class CompareValuesAttribute : ValidationAttribute
    {
        /// <summary>
        /// The other property to compare to
        /// </summary>
        public string OtherProperty { get; set; }

        public CompareValues Criteria { get; set; }

        /// <summary>
        /// Creates the attribute
        /// </summary>
        /// <param name="otherProperty">The other property to compare to</param>
        /// <param name="criteria">The criteria</param>
        public CompareValuesAttribute(string otherProperty, CompareValues criteria)
        {
            if (otherProperty == null)
                throw new ArgumentNullException("otherProperty");

            OtherProperty = otherProperty;
            Criteria = criteria;
        }

        /// <summary>
        /// Determines whether the specified value of the object is valid.  For this to be the case, the objects must be of the same type
        /// and satisfy the comparison criteria. Null values will return false in all cases except when both
        /// objects are null.  The objects will need to implement IComparable for the GreaterThan,LessThan,GreatThanOrEqualTo and LessThanOrEqualTo instances
        /// </summary>
        /// <param name="value">The value of the object to validate</param>
        /// <param name="validationContext">The validation context</param>
        /// <returns>A validation result if the object is invalid, null if the object is valid</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // the the other property
            var property = validationContext.ObjectType.GetProperty(OtherProperty);

            // check it is not null
            if (property == null)
                return new ValidationResult(String.Format("Unknown property: {0}.", OtherProperty));

            // check types
            var memberName = validationContext.ObjectType.GetProperties().Where(
                p => p.GetCustomAttributes(false).OfType<DisplayAttribute>().Any(
                    a => a.Name == validationContext.DisplayName)).Select(p => p.Name).FirstOrDefault() ??
                             validationContext.DisplayName;
            if (validationContext.ObjectType.GetProperty(memberName).PropertyType != property.PropertyType)
                return new ValidationResult(String.Format("The types of {0} and {1} must be the same.", 
                    memberName, OtherProperty));

            // get the other value
            var other = property.GetValue(validationContext.ObjectInstance, null);

            // equals to comparison,
            if (Criteria == CompareValues.EqualTo)
            {
                if (Equals(value, other))
                    return null;
            }
            else if (Criteria == CompareValues.NotEqualTo)
            {
                if (!Equals(value, other))
                    return null;
            }
            else
            {
                // check that both objects are IComparables
                if (!(value is IComparable) || !(other is IComparable))
                    return new ValidationResult(String.Format("{0} and {1} must both implement IComparable", 
                        validationContext.DisplayName, OtherProperty));

                // compare the objects
                var result = Comparer.Default.Compare(value, other);

                switch (Criteria)
                {
                    case CompareValues.GreaterThan:
                        if (result > 0)
                            return null;
                        break;
                    case CompareValues.LessThan:
                        if (result < 0)
                            return null;
                        break;
                    case CompareValues.GreatThanOrEqualTo:
                        if (result >= 0)
                            return null;
                        break;
                    case CompareValues.LessThanOrEqualTo:
                        if (result <= 0)
                            return null;
                        break;
                }
            }

            // got this far must mean the items don't meet the comparison criteria
            return new ValidationResult(ErrorMessage);
        }
    }

    /// <summary>
    /// Indicates a comparison criteria used by the CompareValues attribute
    /// </summary>
    public enum CompareValues
    {
        EqualTo,
        NotEqualTo,
        GreaterThan,
        LessThan,
        GreatThanOrEqualTo,
        LessThanOrEqualTo
    }
}
