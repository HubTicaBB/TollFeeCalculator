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

            DateTime[] actual = Program.Parse(inputData);
            DateTime[] expected = new DateTime[]
            {
                new DateTime(2020, 6, 30, 0, 5, 0),
                new DateTime(2020, 6, 30, 6, 34, 0)
            };

            Assert.AreEqual(expected.Length, actual.Length);
        }

        //private static IEnumerable<object[]> GetDateObjects()
        //{
        //    yield return new object[]
        //    {
        //         "2020-06-30 00:05, 2020-06-30 06:34"
        //         ,
        //        new DateTime[]
        //        {
        //            new DateTime(2020, 6, 30, 0, 5, 0),
        //            new DateTime(2020, 6, 30, 6, 34, 0)
        //        }
        //    };
        //}

        //[TestMethod]
        //[DynamicData(nameof(GetDateObjects), DynamicDataSourceType.Method)]
        //public void ParseTest(string dates, DateTime[] expectedDates)
        //{
        //    var actualDates = Program.Parse(dates);
        //    Assert.AreEqual(expectedDates, actualDates);
        //}

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
        public void CalculateFeePerTimespan_IsFree_ReturnFeeIs0()
        {
            DateTime date = new DateTime(2020,7,1,0,0,0);
            int actualFee = Program.CalculateFeePerTimespan(date);
            int expectedFee = 0;
            Assert.AreEqual(expectedFee,actualFee);
        }

        [TestMethod]
        public void CalculateFeePerTimespan_IsNotFree_ReturnFee18()
        {
            DateTime date = new DateTime(2020, 1, 1, 7, 0, 0);
            int actualFee = Program.CalculateFeePerTimespan(date);
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
            int actualFee = Program.GetFeePerTimespan(timeOfDay);
            Assert.AreEqual(expectedFee, actualFee);
        }
              
        [TestMethod]
        public void CheckIfTotalFeeIsBiggerThenMaxFee_IsTrue()
        {
            int maxFee = 60;
            int totalFee = 70;
            var result = Program.CheckIfTotalFeeIsBiggerThenMaxFee(totalFee,maxFee);
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void CheckIfTotalFeeIsBiggerThenMaxFee_IsFalse()
        {
            int maxFee = 60;
            int totalFee = 0;
            var result = Program.CheckIfTotalFeeIsBiggerThenMaxFee(totalFee, maxFee);
            Assert.IsFalse(result);
        }


        //public static IEnumerable<object[]> GetDateTimeObjects()
        //{
        //    yield return new object[]
        //    {   new DateTime[]
        //        {
        //            new DateTime(2020,1,1,8,0,0),
        //            new DateTime(2020,1,1,9,0,0),
        //            new DateTime(2020,1,1,10,0,0)
        //        } ,
        //        new DateTime[]
        //        {
        //            new DateTime(2020,1,1,8,0,0),
        //            new DateTime(2020,1,1,9,0,0),
        //            new DateTime(2020,1,1,10,0,0)
        //        }
        //    };
        //}

        //[DataTestMethod]
        //[DynamicData(nameof(GetDateTimeObjects), DynamicDataSourceType.Method)]
        //public void GetDatesFromSameDay_WhenCalled_FiltersDaysFromSameDay(DateTime[] dates, DateTime[] expectedDatesFromSameDay)
        //{
        //    var actualDates = Program.GetDatesFromSameDay(dates);
        //    Assert.AreEqual(expectedDatesFromSameDay, actualDates);
        //}

        [TestMethod]
        public void GetDifferenceInMinutes_WhenCalled_ReturnsNumberOfMinutes()
        {
            DateTime initialDate = new DateTime(2020, 1, 1, 0, 0, 0);
            DateTime date = new DateTime(2020, 1, 1, 1, 0, 0);
            double actualMinutes = Program.GetDifferenceInMinutes(initialDate,date);
            int expecteMinutes = 60;
            Assert.AreEqual(expecteMinutes, actualMinutes);
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

        }

        [DataTestMethod]
        [DynamicData(nameof(GetDatesAndFee), DynamicDataSourceType.Method)]
        public void CalculateTotalFee_ReturnFee(DateTime[] dates, int expectedFee)
        {
            int actualFee = Program.CalculateTotalFee(dates);
            Assert.AreEqual(expectedFee, actualFee);
        }

    }
        
}
