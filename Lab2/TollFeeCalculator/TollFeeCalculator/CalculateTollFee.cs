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
            Console.Write("The total fee for the inputfile is: " + CalculateTotalFee(datesFromSameDay));
        }

        public static DateTime[] GetDatesFromSameDay(DateTime[] dates)
        {
            var initialDate = dates[0];
            var datesFromSameDay = new DateTime[dates.Length];
            datesFromSameDay = dates.Where(d => d.Date == initialDate.Date).ToArray();
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

        public static int CalculateTotalFee(DateTime[] dates) 
        {
            int totalFeePerDay = 0;
            int maxFeePerDay = 60; //bugg magic number
            int multiPassageIntervalInMinutes = 60; //bugg magic number

            DateTime initialDate = dates[0];
                       
            foreach (var date in dates)
            {
                /*int differenceInMinutes = (date - initialInvervalDate).Minutes;*/ //bugg 
                double differenceInMinutes = GetDifferenceInMinutes(initialDate, date);
                if (differenceInMinutes > multiPassageIntervalInMinutes)
                {
                    totalFeePerDay += CalculateFeePerTimespan(date);
                    initialDate = date;

                    var temp = CalculateFeePerTimespan(date);
                    Console.WriteLine($"Total fee for {date} is {totalFeePerDay}. Fee/pass: {temp}");
                }
                else
                {
                    totalFeePerDay -= CalculateFeePerTimespan(initialDate); // remove previous fee
                    totalFeePerDay += Math.Max(CalculateFeePerTimespan(date), CalculateFeePerTimespan(initialDate)); //bugg
                    initialDate = date; //saknades

                    var temp2 = Math.Max(CalculateFeePerTimespan(date), CalculateFeePerTimespan(initialDate));
                    Console.WriteLine($"Total fee for (else case) {date} is {totalFeePerDay}. Fee/pass:{temp2}");
                }
            }

            if (CheckIfTotalFeeIsBiggerThenMaxFee(totalFeePerDay,maxFeePerDay))
            { 
                totalFeePerDay = maxFeePerDay;
            }

            return totalFeePerDay; //return Math.Max(fee, 60); //bugg
        }

        public static double GetDifferenceInMinutes(DateTime initialDate, DateTime date)
        {
            return (date - initialDate).TotalMinutes;
        }

        public static bool CheckIfTotalFeeIsBiggerThenMaxFee(int totalFee, int maxFee)
        {
            return totalFee > maxFee;
        }

        public static int CalculateFeePerTimespan(DateTime date)
        {
            TimeSpan timeOfDay = date.TimeOfDay;
            int feePerTimespan;

            if (CheckFreeDate(date))
            {
                feePerTimespan = 0;
            }
            else
            {
                feePerTimespan = GetFeePerTimespan(timeOfDay);
            }

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

        public static bool CheckFreeDate(DateTime date) 
        {
            int july = 7;
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || date.Month == july; //bugg friday , saturday instead of staurday sunday
        }
    }
}
