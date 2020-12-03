using System;
using System.IO;
using System.Linq;

namespace TollFeeCalculator
{
    public class Program
    {
        static void Main()
        {
            string inputFilePath = "../../../../testData.txt";
            Run(Environment.CurrentDirectory + inputFilePath);
        }
        //Write method to check that all dates are from the same day

        static void Run(string path)
        {
            string inputData = File.ReadAllText(path); //bug System.IO
            DateTime[] dates = Parse(inputData);
            DateTime[] datesFromSameDay = GetDatesFromSameDay(dates);
            int totalDailyFee = CalculateTotalFee(datesFromSameDay);
            Console.Write("The total fee for the inputfile is: " + TotalToPay(totalDailyFee));
        }

        public static DateTime[] GetDatesFromSameDay(DateTime[] dates)
        {
            var initialDate = dates[0];
            var datesFromSameDay = new DateTime[dates.Length];
            datesFromSameDay = dates.Where(d => d.Date == initialDate.Date).OrderBy(d => d.Date).ToArray();
            return datesFromSameDay;
        }

        public static DateTime[] Parse(string inputData)
        {
            string[] datesCSV = inputData.Split(',');
            DateTime[] dates = new DateTime[datesCSV.Length]; //bugg

            for (int i = 0; i < dates.Length; i++)
            {
                dates[i] = DateTime.Parse(datesCSV[i]);
            }

            return dates;
        }

        public static int TotalToPay(int calculatedDailyFee)
        {
            const int maxDailyFee = 60;
            return Math.Min(calculatedDailyFee, maxDailyFee);
        }

        public static int CalculateTotalFee(DateTime[] dates)
        {
            int totalDailyFee = 0;
            DateTime intervalStart = dates[0];
            int intervalTotalFee = 0;
            int intervalHighestFee = 0;
            foreach (var date in dates)
            {
                var currentPassageFee = CalculateFeePerTimespan(date);
                if (IsNewInterval(intervalStart, date))
                {
                    totalDailyFee += intervalTotalFee;
                    intervalStart = date;              
                    intervalHighestFee = currentPassageFee;
                    intervalTotalFee = currentPassageFee;
                }
                else
                {
                    intervalTotalFee = RecalculateIntervalFee(intervalTotalFee, intervalHighestFee, currentPassageFee);
                    intervalHighestFee = GetHigher(intervalHighestFee, currentPassageFee);                    
                }
            }
            totalDailyFee += intervalTotalFee;
            return totalDailyFee;
        }

        public static bool IsNewInterval(DateTime intervalStartTime, DateTime currentPassageTime)
        {
            const int multiPassageIntervalMinutes = 60;
            var isIntervalOver = (currentPassageTime - intervalStartTime).TotalMinutes > multiPassageIntervalMinutes;
            var isFirstPassage = intervalStartTime == currentPassageTime;

            return isIntervalOver || isFirstPassage;
        }

        public static int RecalculateIntervalFee(int totalIntervalFee, int highestFee, int currentFee)
        {
            var recalculatedIntervalFee = totalIntervalFee;
            if (currentFee > highestFee)
            {
                recalculatedIntervalFee = totalIntervalFee - highestFee + currentFee;
            }

            return recalculatedIntervalFee;
        }

        public static int GetHigher(int intervalHighestFee, int currentPassageFee)
        {
            return Math.Max(intervalHighestFee, currentPassageFee);
        }

        public static bool CheckIfTotalFeeIsBiggerThenMaxFee(int totalFee, int maxFee)
        {
            return totalFee > maxFee;
        }

        public static int CalculateFeePerTimespan(DateTime date)
        {
            TimeSpan timeOfDay = date.TimeOfDay;
            int feePerTimespan;

            //if (CheckFreeDate(date))
            //{
            //    feePerTimespan = 0;
            //}
            //else
            //{
            //    feePerTimespan = GetFeePerTimespan(timeOfDay);
            //}

            feePerTimespan = (CheckFreeDate(date)) ? 0 : GetFeePerTimespan(timeOfDay);

            return feePerTimespan;
        }

        public static int GetFeePerTimespan(TimeSpan timeOfDay)
        {
            int feePerTimespan;
            if (CheckIfTimeOfDayIsInTimespan(timeOfDay, new TimeSpan(6, 0, 0), new TimeSpan(6, 29, 0)))
                feePerTimespan = 8;
            else if (CheckIfTimeOfDayIsInTimespan(timeOfDay, new TimeSpan(6, 30, 0), new TimeSpan(6, 59, 0)))
                feePerTimespan = 13;
            else if (CheckIfTimeOfDayIsInTimespan(timeOfDay, new TimeSpan(7, 0, 0), new TimeSpan(7, 59, 0)))
                feePerTimespan = 18;
            else if (CheckIfTimeOfDayIsInTimespan(timeOfDay, new TimeSpan(8, 0, 0), new TimeSpan(8, 29, 0)))
                feePerTimespan = 13;
            else if (CheckIfTimeOfDayIsInTimespan(timeOfDay, new TimeSpan(8, 30, 0), new TimeSpan(14, 59, 0))) //bugg
                feePerTimespan = 8;
            else if (CheckIfTimeOfDayIsInTimespan(timeOfDay, new TimeSpan(15, 0, 0), new TimeSpan(15, 29, 0)))
                feePerTimespan = 13;
            else if (CheckIfTimeOfDayIsInTimespan(timeOfDay, new TimeSpan(15, 30, 0), new TimeSpan(16, 59, 0)))
                feePerTimespan = 18;
            else if (CheckIfTimeOfDayIsInTimespan(timeOfDay, new TimeSpan(17, 0, 0), new TimeSpan(17, 59, 0)))
                feePerTimespan = 13;
            else if (CheckIfTimeOfDayIsInTimespan(timeOfDay, new TimeSpan(18, 0, 0), new TimeSpan(18, 29, 0)))
                feePerTimespan = 8;
            else
                feePerTimespan = 0;
            return feePerTimespan;
        }

        public static bool CheckIfTimeOfDayIsInTimespan(TimeSpan timeOfDay, TimeSpan startTime, TimeSpan endTime)
        {
            bool isTimeOfDayInTimespan = false;

            if(timeOfDay >= startTime && timeOfDay <= endTime)
            {
                isTimeOfDayInTimespan= true;
            }

            return isTimeOfDayInTimespan;
        }

        static bool CheckFreeDate(DateTime date) 
        {
            int july = 7;
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || date.Month == july; //bugg friday , saturday instead of staurday sunday
        }
    }
}
