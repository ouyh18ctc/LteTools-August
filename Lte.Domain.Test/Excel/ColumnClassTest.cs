using Lte.Domain.Regular;
using System;
using Lte.Domain.TypeDefs;
using System.Reflection;
using NUnit.Framework;

namespace Lte.Domain.Test.Excel
{
    [TestFixture]
    public class ColumnClassTest
    {
        readonly MemberInfo[] members = (typeof(ColumnClass)).GetMembers();

        [Test]
        public void TestColumnClass_Members()
        {
            Assert.AreEqual(members.Length, 14);
            Assert.AreEqual(members[0].Name, "get_FirstField");
            Assert.AreEqual(members[1].Name, "set_FirstField");
            Assert.AreEqual(members[2].Name, "get_SecondField");
            Assert.AreEqual(members[3].Name, "set_SecondField");
            Assert.AreEqual(members[4].Name, "get_NoAttributeField");
            Assert.AreEqual(members[5].Name, "set_NoAttributeField");
            Assert.AreEqual(members[6].Name, "ToString");
            Assert.AreEqual(members[7].Name, "Equals");
            Assert.AreEqual(members[8].Name, "GetHashCode");
            Assert.AreEqual(members[9].Name, "GetType");
            Assert.AreEqual(members[10].Name, ".ctor");
            Assert.AreEqual(members[11].Name, "FirstField");
            Assert.AreEqual(members[12].Name, "SecondField");
            Assert.AreEqual(members[13].Name, "NoAttributeField");
        }

        [Test]
        public void TestColumnClass_PropertyMemebes()
        {
            PropertyInfo[] properties = (typeof(ColumnClass)).GetProperties();
            Assert.AreEqual(properties.Length, 3);
            Assert.AreEqual(properties[0].Name, "FirstField");
            Assert.AreEqual(properties[0].PropertyType.Name, "Int32");
            Attribute attribute = Attribute.GetCustomAttribute(properties[0], typeof(ColumnAttribute));
            Assert.IsNotNull(attribute);
            Assert.AreEqual((attribute as ColumnAttribute).Name, "First Field");
            Assert.IsTrue((attribute as ColumnAttribute).CanBeNull);
            attribute = Attribute.GetCustomAttribute(properties[1], typeof(ColumnAttribute));
            Assert.IsNotNull(attribute);
            attribute = Attribute.GetCustomAttribute(properties[2], typeof(ColumnAttribute));
            Assert.IsNull(attribute);
        }

        [Test]
        public void TestColumnClass_FirstField()
        {
            MemberInfo firstFieldMember = (typeof(ColumnClass)).GetMember("FirstField")[0];
            Assert.AreEqual(firstFieldMember.Name, "FirstField");
            Attribute attribute = Attribute.GetCustomAttribute(firstFieldMember, typeof(ColumnAttribute));
            Assert.IsNotNull(attribute);
            Assert.AreEqual((attribute as ColumnAttribute).Name, "First Field");
            Assert.IsTrue((attribute as ColumnAttribute).CanBeNull);
        }

        [Test]
        public void TestColumnClass_FirstField_InvokeMethods()
        {
            PropertyInfo firstFieldProperty = (typeof(ColumnClass)).GetProperty("FirstField");
            MethodInfo getFirstFieldProperty = (typeof(ColumnClass)).GetMethod("get_FirstField");
            ColumnClass instance = new ColumnClass() { FirstField = 2 };
            int result = (int)getFirstFieldProperty.Invoke(instance, new object[] { });
            Assert.AreEqual(result, 2);
            result = (int)firstFieldProperty.GetValue(instance, null);
            Assert.AreEqual(result, 2);
            object result2 = firstFieldProperty.GetValue(instance, null);
            Assert.AreEqual(result2, 2);
        }

        [Test]
        public void TestColumnClass_SetFirstField_InvokeMethods()
        {
            PropertyInfo firstFieldProperty = (typeof(ColumnClass)).GetProperty("FirstField");
            ColumnClass instance = new ColumnClass();
            firstFieldProperty.SetValue(instance, 2, null);
            Assert.AreEqual(instance.FirstField, 2);
            MethodInfo parseMethod = firstFieldProperty.PropertyType.GetParseMethod();
            firstFieldProperty.SetValue(instance, parseMethod.Invoke(
                firstFieldProperty.PropertyType, new object[] { "3" }), null);
            Assert.AreEqual(instance.FirstField, 3);
        }

        [Test]
        public void TestColumnClass_Methods()
        {
            MethodInfo[] methods = (typeof(ColumnClass)).GetMethods();
            Assert.AreEqual(methods.Length, 10);
        }

        [Test]
        public void TestColumnClass_FirstField_Attributes()
        {
            MethodInfo firstFieldMethod = (typeof(ColumnClass)).GetMethod("get_FirstField");
            Assert.AreEqual(firstFieldMethod.Name, "get_FirstField");
            Attribute attribute = Attribute.GetCustomAttribute(firstFieldMethod, typeof(ColumnAttribute));
            Assert.IsNull(attribute);
        }
    }
}
