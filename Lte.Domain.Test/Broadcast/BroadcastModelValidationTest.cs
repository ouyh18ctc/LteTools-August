using System;
using NUnit.Framework;

namespace Lte.Domain.Test.Broadcast
{
    [TestFixture]
    public class BroadcastModelValidationTest
    {
        private static double CalculatePathLoss(StubValidationBroadcastModel model,
            double distanceInKilometer, double baseHeight, double mobileHeight = 1.5)
        {
            return model.CalculatePathLoss(distanceInKilometer, baseHeight, mobileHeight);
        }

        StubValidationBroadcastModel model;

        [SetUp]
        public void TestInitialize()
        {
            model = new StubValidationBroadcastModel();
            
        }

        [Test]
        public void TestValidation_Stub()
        {
            double x = CalculatePathLoss(model, 1, 1);
            Assert.AreEqual(x, 0);
           
        }

        [Test]
        public void TestValidation_Stub1()
        {  
            TestModelValidation(model, p1: 0);
            TestModelValidation(model, p2: 0);
        }

        [Test]
        public void TestValidation_Stub2()
        { 
            TestModelValidation(model, p2:1E-6);
            TestModelValidation(model, p3: 1E-6);
        }

        [Test]
        public void TestValidation_Stub3()
        {
            TestModelValidation(model, p2:1E-5);
            TestModelValidation(model, p3: 1E-5);
            TestModelValidation(model, p2: 1E-5, p3:2);
        }

        private static void TestModelValidation(StubValidationBroadcastModel model, double p1 = 1, double p2 =1, double p3 =1)
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
    }
}
