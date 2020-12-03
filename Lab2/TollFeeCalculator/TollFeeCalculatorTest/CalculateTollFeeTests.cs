using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Program program = new Program();
            DateTime[] actual = program.Parse(inputData);
            DateTime[] expected = new DateTime[]
            {
                new DateTime(2020, 6, 30, 0, 5, 0),
                new DateTime(2020, 6, 30, 6, 34, 0)
            };

            Assert.AreEqual(expected.Length, actual.Length);
        }

        private static IEnumerable<object[]> GetDateObjects()
        {
            yield return new object[]
            {
                 "2020-06-30 00:05, 2020-06-30 06:34"
                 ,
                new DateTime[]
                {
                    new DateTime(2020, 6, 30, 0, 5, 0),
                    new DateTime(2020, 6, 30, 6, 34, 0)
                }
            };
        }

        [TestMethod]
        [DynamicData(nameof(GetDateObjects), DynamicDataSourceType.Method)]
        public void ParseTest(string dates, DateTime[] expectedDates)
        {
            Program program = new Program();
            var actualDates = program.Parse(dates);
            CollectionAssert.AreEqual(expectedDates, actualDates);
        }

        [TestMethod]
        public void CheckTimeOfDaySpan_WhenCalled_IsTrue()
        {
            Program program = new Program();
            var timeOfDay = new TimeSpan(7,15,0);
            var startTimespan = new TimeSpan(7,0,0);
            var endTimespan = new TimeSpan(7,59,0);
            var result = program.CheckIfTimeOfDayIsInTimespan(timeOfDay, startTimespan, endTimespan);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckTimeOfDaySpan_WhenCalled_IsFalse()
        {
            Program program = new Program();
            var timeOfDay = new TimeSpan(7, 15, 0);
            var startTimespan = new TimeSpan(10, 0, 0);
            var endTimespan = new TimeSpan(10, 59, 0);
            var result = program.CheckIfTimeOfDayIsInTimespan(timeOfDay, startTimespan, endTimespan);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CalculateFeePerTimespan_IsFree_ReturnFeeIs0()
        {
            Program program = new Program();
            DateTime date = new DateTime(2020,7,1,0,0,0);
            int actualFee = program.CalculateFeePerTimespan(date);
            int expectedFee = 0;
            Assert.AreEqual(expectedFee,actualFee);
        }

        [TestMethod]
        public void CalculateFeePerTimespan_IsNotFree_ReturnFee18()
        {
            Program program = new Program();
            DateTime date = new DateTime(2020, 1, 1, 7, 0, 0);
            int actualFee = program.CalculateFeePerTimespan(date);
            int expectedFee = 18;
            Assert.AreEqual(expectedFee, actualFee);
        }

        public static IEnumerable<object[]> GetTimespanObjects()
        {
            yield return new object[] { new TimeSpan(6, 0, 0), 8 };
            yield return new object[] { new TimeSpan(6, 30, 0), 13 };
            yield return new object[] { new TimeSpan(7, 0, 0), 18 };
            yield return new object[] { new TimeSpan(8, 0, 0), 13 };
            yield return new object[] { new TimeSpan(8, 30, 0), 8 };
            yield return new object[] { new TimeSpan(15, 0, 0), 13 };
            yield return new object[] { new TimeSpan(15, 30, 0), 18 };
            yield return new object[] { new TimeSpan(17, 0, 0), 13 };
            yield return new object[] { new TimeSpan(18, 0, 0), 8 };
            yield return new object[] { new TimeSpan(18, 30, 0), 0 };
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTimespanObjects), DynamicDataSourceType.Method)]
        public void GetFeePerTimespan_IsInTimespan_ReturnFee(TimeSpan timeOfDay, int expectedFee)
        {
            Program program = new Program();
            int actualFee = program.GetFeePerTimespan(timeOfDay);
            Assert.AreEqual(expectedFee, actualFee);
        }
        
        public static IEnumerable<object[]> GetDateTimeObjects()
        {
            yield return new object[]
            {   new DateTime[]
                {
                    new DateTime(2020,1,1,8,0,0),
                    new DateTime(2020,1,1,9,0,0),
                    new DateTime(2020,1,1,10,0,0),
                    new DateTime(2020,1,2,10,0,0)
                } ,
                new DateTime[]
                {
                    new DateTime(2020,1,1,8,0,0),
                    new DateTime(2020,1,1,9,0,0),
                    new DateTime(2020,1,1,10,0,0)
                }
            };
        }

        [DataTestMethod]
        [DynamicData(nameof(GetDateTimeObjects), DynamicDataSourceType.Method)]
        public void GetDatesFromSameDay_WhenCalled_FiltersDaysFromSameDay(DateTime[] dates, DateTime[] expectedDatesFromSameDay)
        {
            Program program = new Program();
            var actualDates = program.GetDatesFromSameDay(dates);
            CollectionAssert.AreEquivalent(actualDates, expectedDatesFromSameDay);
        }

        public static IEnumerable<object[]> GetDatesAndFee()
        {
            yield return new object[] 
            {
                new DateTime[]
                {
                    new DateTime(2020, 6, 30, 0, 5, 0),
                    new DateTime(2020, 6, 30, 6, 34, 0)
                }
                , 13 
            };
            yield return new object[]
            {
                new DateTime[]
                {
                    new DateTime(2020, 6, 30, 8, 0, 0),
                    new DateTime(2020, 6, 30, 8, 30, 0)
                }
                , 13
            };
            yield return new object[]
            {
                new DateTime[]
                {
                    new DateTime(2020, 6, 30, 8, 0, 0),
                }
                , 13
            };
            yield return new object[]
            {
                new DateTime[]
                {
                    new DateTime(2020, 6, 30, 10, 13, 0),
                    new DateTime(2020, 6, 30, 10, 25, 0),
                    new DateTime(2020, 6, 30, 11, 04, 0),                    
                }
                , 8
            };
        }

        [DataTestMethod]
        [DynamicData(nameof(GetDatesAndFee), DynamicDataSourceType.Method)]
        public void CalculateTotalFee_ReturnFee(DateTime[] dates, int expectedFee)
        {
            Program program = new Program();
            int actualFee = program.CalculateTotalFee(dates);
            Assert.AreEqual(expectedFee, actualFee);
        }

        [TestMethod]
        public void TotalToPay_TotalIsGreaterThanMax_ReturnsSixty()
        {
            Program program = new Program();
            var expected = 60;
            var actual = program.TotalToPay(61);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TotalToPay_TotalIsLessThanMax_ReturnsTotal()
        {
            Program program = new Program();
            var expected = 59;
            var actual = program.TotalToPay(59);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckInterval_SameInterval_ReturnsFalse()
        {
            Program program = new Program();
            var intervalStartTime = new DateTime(2020, 1, 1, 6, 0, 0);
            var currentPassageTime = new DateTime(2020, 1, 1, 7, 0, 0);

            var newInterval = program.IsNewInterval(intervalStartTime, currentPassageTime);

            Assert.IsFalse(newInterval);
        }

        [TestMethod]
        public void CheckInterval_NewInterval_ReturnsTrue()
        {
            Program program = new Program();
            var intervalStartTime = new DateTime(2020, 1, 1, 6, 0, 0);
            var currentPassageTime = new DateTime(2020, 1, 1, 7, 1, 0);

            var newInterval = program.IsNewInterval(intervalStartTime, currentPassageTime);
            
            Assert.IsTrue(newInterval);
        }

        [TestMethod]
        public void RecalculateFee_CurrentFeeIsHigherThanPrevious_ReturnsNewTotal()
        {
            Program program = new Program();
            var oldtotal = 10;
            var previous = 1;
            var current = 2;
            var expectedNewTotal = 11;

            var actualTotal = program.RecalculateIntervalFee(oldtotal, previous, current);

            Assert.AreEqual(expectedNewTotal, actualTotal);
        }

        [TestMethod]
        public void RecalculateFee_CurrentFeeIsNotHigherThanPrevious_ReturnsOldTotal()
        {
            Program program = new Program();
            var oldtotal = 10;
            var previous = 1;
            var current = 1;

            var actualTotal = program.RecalculateIntervalFee(oldtotal, previous, current);

            Assert.AreEqual(oldtotal, actualTotal);
        }

        [TestMethod]
        public void GetHigher_PreviousIsHigher_ReturnsPrevious()
        {
            Program program = new Program();
            var previous = 2;
            var current = 1;

            var actualHigher = program.GetHigher(previous, current);

            Assert.AreEqual(previous, actualHigher);
        }

        [TestMethod]
        public void GetHigher_CurrentIsHigher_ReturnsHigher()
        {
            Program program = new Program();
            var previous = 1;
            var current = 2;

            var actualHigher = program.GetHigher(previous, current);

            Assert.AreEqual(current, actualHigher);
        }

        //public static IEnumerable<object[]> GetFreeDate()
        //{
        //    yield return new object[] { new DateTime(2020, 7, 1)};
        //    yield return new object[] { new DateTime(2020, 12, 5)};
        //    yield return new object[] { new DateTime(2020, 12, 6)};
        //}

        //[DataTestMethod]
        //[DynamicData(nameof(GetFreeDate), DynamicDataSourceType.Method)]
        //public void CheckFreeDates_ReturnsTrue(DateTime date)
        //{
        //    Program program = new Program();
        //    bool result = program.CheckFreeDates(date);
        //    Assert.IsTrue(result);
        //}

    }
        
}
