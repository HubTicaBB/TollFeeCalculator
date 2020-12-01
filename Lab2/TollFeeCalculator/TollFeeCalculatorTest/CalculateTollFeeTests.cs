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
        public void CheckFreeDate_DayIsSaturday_ReturnTrue()
        {
            var saturdayDate = new DateTime(2020, 11, 28);

            Assert.IsTrue(Program.CheckFreeDate(saturdayDate));
        }

        [TestMethod]
        public void CheckFreeDate_DayIsSunday_ReturnTrue()
        {
            var sundayDate = new DateTime(2020, 11, 29);

            Assert.IsTrue(Program.CheckFreeDate(sundayDate));
        }

        [TestMethod]
        public void CheckFreeDate_MonthIsJuly_ReturnTrue()
        {
            var julyDate = new DateTime(2020, 7, 1);

            Assert.IsTrue(Program.CheckFreeDate(julyDate));
        }

        [TestMethod]
        public void CheckFreeDate_DayIsNotWeekend_ReturnFalse()
        {
            var nonWeekendDate = new DateTime(2020, 11, 30);

            Assert.IsFalse(Program.CheckFreeDate(nonWeekendDate));
        }

        [TestMethod]
        public void CheckFreeDate_MonthIsNotJuly_ReturnFalse()
        {
            var nonJulyMonth = new DateTime(2020, 01, 01);

            Assert.IsFalse(Program.CheckFreeDate(nonJulyMonth));
        }
    }
}
