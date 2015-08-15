using System.Collections.Generic;
using Lte.Evaluations.Entities;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Entities
{
    [TestFixture]
    public class StatValueFieldRepositoryAccessTest : StatValueFieldTestConfig
    {
        [SetUp]
        public void TestInitialize()
        {
            Initialize();
            Repository = new StatValueFieldRepository
            {
                FieldList = statValueFieldList
            };
        }

        [Test]
        public void TestStatValueFieldRepository_GetIntervalList()
        {
            Assert.AreEqual(Repository["field1"].IntervalList.Count, 3);
            Assert.AreEqual(Repository["field1"].IntervalList[1].IntervalUpLevel, 2);
            Assert.AreEqual(Repository["field2"].IntervalList[0].Color.ColorG, 255);
            Assert.AreEqual(Repository["field2"].IntervalList[1].IntervalLowLevel, 4);
        }

        [Test]
        public void TestStatValueFieldRepository_ModifyIntervalListUpLevel()
        {
            Assert.IsNotNull(Repository["field1"]);
            Repository["field1"].UpdateIntervalUpLevel(1, 2.5);
            Assert.AreEqual(Repository.FieldDoc.ToString().Replace("\r\n", "\n"), (@"<Setting>
  <Field ID=""field1"">
    <Interval>
      <LowLevel>0</LowLevel>
      <UpLevel>1</UpLevel>
      <A>255</A>
      <B>255</B>
      <R>255</R>
      <G>255</G>
    </Interval>
    <Interval>
      <LowLevel>1</LowLevel>
      <UpLevel>2.5</UpLevel>
      <A>5</A>
      <B>255</B>
      <R>255</R>
      <G>5</G>
    </Interval>
    <Interval>
      <LowLevel>2.5</LowLevel>
      <UpLevel>3</UpLevel>
      <A>255</A>
      <B>25</B>
      <R>25</R>
      <G>255</G>
    </Interval>
  </Field>
  <Field ID=""field2"">
    <Interval>
      <LowLevel>2</LowLevel>
      <UpLevel>3</UpLevel>
      <A>155</A>
      <B>155</B>
      <R>5</R>
      <G>255</G>
    </Interval>
    <Interval>
      <LowLevel>4</LowLevel>
      <UpLevel>5</UpLevel>
      <A>5</A>
      <B>35</B>
      <R>35</R>
      <G>5</G>
    </Interval>
  </Field>
</Setting>").Replace("\r\n", "\n"));
        }

        [Test]
        public void TestStatValueFieldRepository_ModifyIntervalListLowLevel()
        {
            Assert.IsNotNull(Repository["field2"]);
            Repository["field2"].UpdateIntervalLowLevel(1, 4.5);
            Assert.AreEqual(Repository.FieldDoc.ToString().Replace("\r\n", "\n"), (@"<Setting>
  <Field ID=""field1"">
    <Interval>
      <LowLevel>0</LowLevel>
      <UpLevel>1</UpLevel>
      <A>255</A>
      <B>255</B>
      <R>255</R>
      <G>255</G>
    </Interval>
    <Interval>
      <LowLevel>1</LowLevel>
      <UpLevel>2</UpLevel>
      <A>5</A>
      <B>255</B>
      <R>255</R>
      <G>5</G>
    </Interval>
    <Interval>
      <LowLevel>2</LowLevel>
      <UpLevel>3</UpLevel>
      <A>255</A>
      <B>25</B>
      <R>25</R>
      <G>255</G>
    </Interval>
  </Field>
  <Field ID=""field2"">
    <Interval>
      <LowLevel>2</LowLevel>
      <UpLevel>4.5</UpLevel>
      <A>155</A>
      <B>155</B>
      <R>5</R>
      <G>255</G>
    </Interval>
    <Interval>
      <LowLevel>4.5</LowLevel>
      <UpLevel>5</UpLevel>
      <A>5</A>
      <B>35</B>
      <R>35</R>
      <G>5</G>
    </Interval>
  </Field>
</Setting>").Replace("\r\n", "\n"));
        }

        [Test]
        public void TestStatValueFieldRepository_ModifyColor()
        {
            Repository["field1"].IntervalList[2].Color.ColorB = 17;
            Assert.AreEqual(Repository.FieldDoc.ToString().Replace("\r\n", "\n"), (@"<Setting>
  <Field ID=""field1"">
    <Interval>
      <LowLevel>0</LowLevel>
      <UpLevel>1</UpLevel>
      <A>255</A>
      <B>255</B>
      <R>255</R>
      <G>255</G>
    </Interval>
    <Interval>
      <LowLevel>1</LowLevel>
      <UpLevel>2</UpLevel>
      <A>5</A>
      <B>255</B>
      <R>255</R>
      <G>5</G>
    </Interval>
    <Interval>
      <LowLevel>2</LowLevel>
      <UpLevel>3</UpLevel>
      <A>255</A>
      <B>17</B>
      <R>25</R>
      <G>255</G>
    </Interval>
  </Field>
  <Field ID=""field2"">
    <Interval>
      <LowLevel>2</LowLevel>
      <UpLevel>3</UpLevel>
      <A>155</A>
      <B>155</B>
      <R>5</R>
      <G>255</G>
    </Interval>
    <Interval>
      <LowLevel>4</LowLevel>
      <UpLevel>5</UpLevel>
      <A>5</A>
      <B>35</B>
      <R>35</R>
      <G>5</G>
    </Interval>
  </Field>
</Setting>").Replace("\r\n", "\n"));
        }

        [Test]
        public void TestStatValueFieldRepository_ReplaceList()
        {
            Repository["field1"].IntervalList = new List<StatValueInterval>
            {
                            new StatValueInterval
                            {
                                IntervalLowLevel = 2,
                                IntervalUpLevel = 3,
                                Color = new Color
                                {
                                    ColorA = 175,
                                    ColorB = 125,
                                    ColorG = 45,
                                    ColorR = 5
                                }
                            }
            };
            Assert.AreEqual(Repository.FieldDoc.ToString().Replace("\r\n", "\n"), (@"<Setting>
  <Field ID=""field1"">
    <Interval>
      <LowLevel>2</LowLevel>
      <UpLevel>3</UpLevel>
      <A>175</A>
      <B>125</B>
      <R>5</R>
      <G>45</G>
    </Interval>
  </Field>
  <Field ID=""field2"">
    <Interval>
      <LowLevel>2</LowLevel>
      <UpLevel>3</UpLevel>
      <A>155</A>
      <B>155</B>
      <R>5</R>
      <G>255</G>
    </Interval>
    <Interval>
      <LowLevel>4</LowLevel>
      <UpLevel>5</UpLevel>
      <A>5</A>
      <B>35</B>
      <R>35</R>
      <G>5</G>
    </Interval>
  </Field>
</Setting>").Replace("\r\n", "\n"));
        }
    }
}
