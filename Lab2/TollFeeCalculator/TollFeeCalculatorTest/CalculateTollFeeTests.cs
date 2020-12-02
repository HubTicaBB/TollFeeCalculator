using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TollFeeCalculator;

namespace TollFeeCalculatorTest
{
    [TestClass]
    public class CalculateTollFeeTests
    {
        [TestMethod]
        public void Parse_WhenCalled_ReturnSameLength()
        {
            var inputData = "2020-06-30 00:05, 2020-06-30 06:34";

            DateTime[] actual = Program.Parse(inputData);
            DateTime[] expected = new DateTime[]
            {
                new DateTime(2020, 6, 30, 0, 5, 0),
                new DateTime(2020, 6, 30, 6, 34, 0)
            };

            Assert.AreEqual(expected.Length, actual.Length);
        }

        [TestMethod]
        public void CheckTimeOfDaySpan_WhenCalled_IsTrue()
        {
            var timeOfDay = new TimeSpan(7,15,0);
            var startTimespan = new TimeSpan(7,0,0);
            var endTimespan = new TimeSpan(7,59,0);
            var result = Program.CheckIfTimeOfDayIsInTimespan(timeOfDay, startTimespan, endTimespan);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckTimeOfDaySpan_WhenCalled_IsFalse()
        {
            var timeOfDay = new TimeSpan(7, 15, 0);
            var startTimespan = new TimeSpan(10, 0, 0);
            var endTimespan = new TimeSpan(10, 59, 0);
            var result = Program.CheckIfTimeOfDayIsInTimespan(timeOfDay, startTimespan, endTimespan);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CalculateFeePerTimespan_IsFree_IsTrue()
        {
            DateTime date = new DateTime(2020,7,1,0,0,0);
            int actualFee = Program.CalculateFeePerTimespan(date);
            int expectedFee = 0;
            Assert.AreEqual(expectedFee,actualFee);
        }

        [TestMethod]
        public void CalculateFeePerTimespan_IsFree_IsFalse()
        {
            DateTime date = new DateTime(2020, 1, 1, 7, 0, 0);
            int actualFee = Program.CalculateFeePerTimespan(date);
            int expectedFee = 18;
            Assert.AreEqual(expectedFee, actualFee);
        }

        


        [TestMethod]
        public void GetFeePerTimespan_Between6and6_29()
        {
            TimeSpan timeOfDay = new TimeSpan(6, 15, 0);
            int actualFee = Program.GetFeePerTimespan(timeOfDay);
            int expectedFee = 8;
            Assert.AreEqual(expectedFee, actualFee);
        }

        [TestMethod]
        public void GetFeePerTimespan_Between6_30and6_59()
        {
            TimeSpan timeOfDay = new TimeSpan(6, 45, 0);
            int actualFee = Program.GetFeePerTimespan(timeOfDay);
            int expectedFee = 13;
            Assert.AreEqual(expectedFee, actualFee);
        }

      
        [TestMethod]
        public void GetFeePerTimespan_Between8_30and14_59()
        {
            TimeSpan timeOfDay = new TimeSpan(13, 15, 0);
            int actualFee = Program.GetFeePerTimespan(timeOfDay);
            int expectedFee = 8;
            Assert.AreEqual(expectedFee, actualFee);
        }
    }
        
}
