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
            var result = Program.CheckIfTimeOfDayIsInTimespan(timeOfDay, (7,0),(7,59));
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckTimeOfDaySpan_WhenCalled_IsFalse()
        {
            var timeOfDay = new TimeSpan(7, 15, 0);
            var result = Program.CheckIfTimeOfDayIsInTimespan(timeOfDay, (10, 0), (10, 59));
            Assert.IsFalse(result);
        }
    }
}
