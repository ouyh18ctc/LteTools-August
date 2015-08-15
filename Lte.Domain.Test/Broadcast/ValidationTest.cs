using System;
using Lte.Domain.Measure;
using NUnit.Framework;

namespace Lte.Domain.Test.Broadcast
{
    [TestFixture]
    public class ValidationTest
    {
        BroadcastModel model;

        private static double CalculatePathLoss(IBroadcastModel model,
           double distanceInKilometer, double baseHeight, double mobileHeight = 1.5)
        {
            return model.CalculatePathLoss(distanceInKilometer, baseHeight, mobileHeight);
        }

        private static void TestModelValidation(IBroadcastModel model, double p1 = 1, double p2 = 1, double p3 = 1)
        {
            try
            {
                double x = CalculatePathLoss(model, p1, p2, p3);

            }
            catch (Exception e)
            {
                if (e is ArgumentOutOfRangeException) { Assert.AreEqual(1, 1, "exception!"); }
                return;
            }
            Assert.AreEqual(0, 1, "The validation is invalid!");
        }

        [SetUp]
        public void TestInitialize()
        {
            model = new BroadcastModel();
        }

        [Test]
        public void TestValidation_Industry()
        {
            
            double x = CalculatePathLoss(model, 1, 1);
            Assert.AreEqual(x, 159, 1);

        }

        [Test]
        public void TestValidation_Industry1()
        {
            TestModelValidation(model, p1: 0);
            TestModelValidation(model, p2: 0);
        }

        [Test]
        public void TestValidation_Industry2()
        {
            TestModelValidation(model, p2: 1E-6);
            TestModelValidation(model, p3: 1E-6);
        }

        [Test]
        public void TestValidation_Industry3()
        {
            TestModelValidation(model, p2: 1E-5);
            TestModelValidation(model, p3: 1E-5);
            TestModelValidation(model, p2: 1E-5, p3: 2);
        }

    }
}
