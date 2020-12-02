using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
    }
}
