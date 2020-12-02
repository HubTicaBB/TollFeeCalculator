using System;
using System.IO;

namespace TollFeeCalculator
{
    public class Program
    {
        static void Main()
        {
            string inputFilePath = "../../../../testData.txt";
            Run(Environment.CurrentDirectory + inputFilePath);
        }
        //Write method to check that all dates are drom the same day

        static void Run(string path)
        {
            string inputData = File.ReadAllText(path);
            DateTime[] dates = Parse(inputData);
            Console.Write("The total fee for the inputfile is: " + CalculateTotalFee(dates));
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

        static int CalculateTotalFee(DateTime[] dates) 
        {
            int fee = 0;
            int multiPassageIntervalInMinutes = 60;
            DateTime initialInvervalDate = dates[0];
            foreach (var date in dates)
            {
                int differenceInMinutes = (date - initialInvervalDate).Minutes;
                if(differenceInMinutes > multiPassageIntervalInMinutes) {
                    fee += CalculateFeePerTimespan(date);
                    var temp = CalculateFeePerTimespan(date);
                    initialInvervalDate = date;
                    Console.WriteLine($"Total fee for {date} is {fee}. Fee/pass: {temp}" );
                } else {
                    fee += Math.Max(CalculateFeePerTimespan(date), CalculateFeePerTimespan(initialInvervalDate));
                    var temp2 = Math.Max(CalculateFeePerTimespan(date), CalculateFeePerTimespan(initialInvervalDate));
                    Console.WriteLine($"Total fee for (else case) {date.Hour}:{date.Minute} is {fee}. Fee/pass:{temp2}");
                }
            }
            return Math.Max(fee, 60); //bugg
        }

        public static int CalculateFeePerTimespan(DateTime date)
        {
            TimeSpan timeOfDay = date.TimeOfDay;
            int feePerTimespan;

            if (CheckFreeDate(date))
                feePerTimespan = 0;
                       
            if (CheckIfTimeOfDayIsInTimespan(timeOfDay, new TimeSpan(6, 0, 0), new TimeSpan(6, 29, 0)))
                feePerTimespan= 8;
            else if (CheckIfTimeOfDayIsInTimespan(timeOfDay, new TimeSpan(6, 30, 0), new TimeSpan(6, 59, 0)))
                feePerTimespan = 13;
            else if (CheckIfTimeOfDayIsInTimespan(timeOfDay, new TimeSpan(7, 0, 0), new TimeSpan(7, 59, 0)))
                feePerTimespan = 18;
            else if (CheckIfTimeOfDayIsInTimespan(timeOfDay, new TimeSpan(8, 0, 0), new TimeSpan(8, 29, 0)))
                feePerTimespan = 13;
            else if (CheckIfTimeOfDayIsInTimespan(timeOfDay, new TimeSpan(8, 30, 0), new TimeSpan(14, 59, 0)))
                feePerTimespan= 8;
            else if (CheckIfTimeOfDayIsInTimespan(timeOfDay, new TimeSpan(15, 0, 0), new TimeSpan(15, 29, 0)))
                feePerTimespan= 13;
            else if (CheckIfTimeOfDayIsInTimespan(timeOfDay, new TimeSpan(15, 30, 0), new TimeSpan(16, 59, 0)))
                feePerTimespan= 18;
            else if (CheckIfTimeOfDayIsInTimespan(timeOfDay, new TimeSpan(17, 0, 0), new TimeSpan(17, 59, 0)))
                feePerTimespan= 13;
            else if (CheckIfTimeOfDayIsInTimespan(timeOfDay, new TimeSpan(18, 0, 0), new TimeSpan(18, 29, 0)))
                feePerTimespan= 8;
            else
                feePerTimespan= 0;

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
            int saturday = 6;
            int sunday = 0;
            int july = 7;
            return (int)date.DayOfWeek == saturday || (int)date.DayOfWeek == sunday || date.Month == july; //bugg 
        }
    }
}
