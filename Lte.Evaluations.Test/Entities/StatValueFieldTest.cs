using System.Collections.Generic;
using NUnit.Framework;
using Lte.Evaluations.Entities;

namespace Lte.Evaluations.Test.Entities
{
    [TestFixture]
    public class StatValueFieldTest
    {
        private StatValueField _statValueField;

        [SetUp]
        public void TestInitialize()
        {
            _statValueField = new StatValueField
            {
                IntervalList = new List<StatValueInterval>
                {
                    new StatValueInterval
                    {
                        IntervalLowLevel = 0,
                        IntervalUpLevel = 1
                    },
                    new StatValueInterval
                    {
                        IntervalLowLevel = 1,
                        IntervalUpLevel = 2
                    },
                    new StatValueInterval
                    {
                        IntervalLowLevel = 2,
                        IntervalUpLevel = 3
                    }
                }
            };
        }

        [TestCase(0, -1, -1, 1)]
        [TestCase(0, 1.1, 0.99, 1)]
        public void Test_UpdateLowLevel(int index, double low, double lowInterval, double upInterval)
        {
            _statValueField.UpdateIntervalLowLevel(0, -1);
            Assert.AreEqual(_statValueField.IntervalList[0].IntervalLowLevel, -1);
            Assert.AreEqual(_statValueField.IntervalList[0].IntervalUpLevel, 1);
        }

        [TestCase(0, 0, 0, 0.01, 0.01)]
        [TestCase(0, 1.9, 0, 1.9, 1.9)]
        [TestCase(0, 2, 0, 1.99, 1.99)]
        public void Test_UpdateUpLevel_TooSmall(int index, double low, 
            double lowInterval1, double upInterval1, double lowInterval2)
        {
            _statValueField.UpdateIntervalUpLevel(0, 0);
            Assert.AreEqual(_statValueField.IntervalList[0].IntervalLowLevel, 0);
            Assert.AreEqual(_statValueField.IntervalList[0].IntervalUpLevel, 0.01);
            Assert.AreEqual(_statValueField.IntervalList[1].IntervalLowLevel, 0.01);
        }

        [Test]
        public void TestStatValueInterval_CurrentSelectedIndex1_UpdateLowLevel_TooSmall()
        {
            _statValueField.UpdateIntervalLowLevel(1, 0);
            Assert.AreEqual(_statValueField.IntervalList[0].IntervalUpLevel, 0.01);
            Assert.AreEqual(_statValueField.IntervalList[1].IntervalLowLevel, 0.01);
            Assert.AreEqual(_statValueField.IntervalList[1].IntervalUpLevel, 2);
        }

        [Test]
        public void TestStatValueInterval_CurrentSelectedIndex1_UpdateLowLevel_Normal()
        {
            _statValueField.UpdateIntervalLowLevel(1, 1.1);
            Assert.AreEqual(_statValueField.IntervalList[0].IntervalUpLevel, 1.1);
            Assert.AreEqual(_statValueField.IntervalList[1].IntervalLowLevel, 1.1);
            Assert.AreEqual(_statValueField.IntervalList[1].IntervalUpLevel, 2);
        }

        [Test]
        public void TestStatValueInterval_CurrentSelectedIndex1_UpdateLowLevel_TooLarge()
        {
            _statValueField.UpdateIntervalLowLevel(1, 2.1);
            Assert.AreEqual(_statValueField.IntervalList[0].IntervalUpLevel, 1.99);
            Assert.AreEqual(_statValueField.IntervalList[1].IntervalLowLevel, 1.99);
            Assert.AreEqual(_statValueField.IntervalList[1].IntervalUpLevel, 2);
        }

        [Test]
        public void TestStatValueInterval_CurrentSelectedIndex1_UpdateUpLevel_TooSmall()
        {
            _statValueField.UpdateIntervalUpLevel(1, 1);
            Assert.AreEqual(_statValueField.IntervalList[1].IntervalLowLevel, 1);
            Assert.AreEqual(_statValueField.IntervalList[1].IntervalUpLevel, 1.01);
            Assert.AreEqual(_statValueField.IntervalList[2].IntervalLowLevel, 1.01);
        }

        [Test]
        public void TestStatValueInterval_CurrentSelectedIndex1_UpdateUpLevel_Normal()
        {
            _statValueField.UpdateIntervalUpLevel(1, 2.1);
            Assert.AreEqual(_statValueField.IntervalList[1].IntervalLowLevel, 1);
            Assert.AreEqual(_statValueField.IntervalList[1].IntervalUpLevel, 2.1);
            Assert.AreEqual(_statValueField.IntervalList[2].IntervalLowLevel, 2.1);
        }

        [Test]
        public void TestStatValueInterval_CurrentSelectedIndex1_UpdateUpLevel_TooLarge()
        {
            _statValueField.UpdateIntervalUpLevel(1, 3);
            Assert.AreEqual(_statValueField.IntervalList[1].IntervalLowLevel, 1);
            Assert.AreEqual(_statValueField.IntervalList[1].IntervalUpLevel, 2.99);
            Assert.AreEqual(_statValueField.IntervalList[2].IntervalLowLevel, 2.99);
        }

        [Test]
        public void TestStatValueInterval_CurrentSelectedIndex2_UpdateLowLevel_TooSmall()
        {
            _statValueField.UpdateIntervalLowLevel(2, 1);
            Assert.AreEqual(_statValueField.IntervalList[1].IntervalUpLevel, 1.01);
            Assert.AreEqual(_statValueField.IntervalList[2].IntervalLowLevel, 1.01);
            Assert.AreEqual(_statValueField.IntervalList[2].IntervalUpLevel, 3);
        }

        [Test]
        public void TestStatValueInterval_CurrentSelectedIndex2_UpdateLowLevel_Normal()
        {
            _statValueField.UpdateIntervalLowLevel(2, 2.1);
            Assert.AreEqual(_statValueField.IntervalList[1].IntervalUpLevel, 2.1);
            Assert.AreEqual(_statValueField.IntervalList[2].IntervalLowLevel, 2.1);
            Assert.AreEqual(_statValueField.IntervalList[2].IntervalUpLevel, 3);
        }

        [Test]
        public void TestStatValueInterval_CurrentSelectedIndex2_UpdateLowLevel_TooLarge()
        {
            _statValueField.UpdateIntervalLowLevel(2, 3.1);
            Assert.AreEqual(_statValueField.IntervalList[1].IntervalUpLevel, 2.99);
            Assert.AreEqual(_statValueField.IntervalList[2].IntervalLowLevel, 2.99);
            Assert.AreEqual(_statValueField.IntervalList[2].IntervalUpLevel, 3);
        }

        [Test]
        public void TestStatValueInterval_CurrentSelectedIndex2_UpdateUpLevel_TooSmall()
        {
            _statValueField.UpdateIntervalUpLevel(2, 2);
            Assert.AreEqual(_statValueField.IntervalList[2].IntervalLowLevel, 2);
            Assert.AreEqual(_statValueField.IntervalList[2].IntervalUpLevel, 2.01);
        }

        [Test]
        public void TestStatValueInterval_CurrentSelectedIndex2_UpdateUpLevel_Normal()
        {
            _statValueField.UpdateIntervalUpLevel(2, 4.1);
            Assert.AreEqual(_statValueField.IntervalList[2].IntervalLowLevel, 2);
            Assert.AreEqual(_statValueField.IntervalList[2].IntervalUpLevel, 4.1);
        }
    }
}
