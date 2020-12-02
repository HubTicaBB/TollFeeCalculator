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
                    initialInvervalDate = date;
                    Console.WriteLine($"fee for {date} is {fee}" );
                } else {
                    fee += Math.Max(CalculateFeePerTimespan(date), CalculateFeePerTimespan(initialInvervalDate));
                    Console.WriteLine($"fee for (else) {date.Hour}:{date.Minute} is {fee}");
                }
            }
            return Math.Max(fee, 60); //bugg
        }

        static int CalculateFeePerTimespan(DateTime date)
        {
            if (CheckFreeDate(date)) 
                return 0; //bugg

            TimeSpan timeOfDay = date.TimeOfDay;
           
            if (CheckIfTimeOfDayIsInTimespan(timeOfDay, (6, 0), (6, 29)))
                return 8;
            else if (CheckIfTimeOfDayIsInTimespan(timeOfDay, (6, 30), (6, 59)))
                return 13;
            else if (CheckIfTimeOfDayIsInTimespan(timeOfDay, (7, 0), (7, 59)))
                return 18;
            else if (CheckIfTimeOfDayIsInTimespan(timeOfDay, (8, 0), (8, 29)))
                return 13;
            else if (CheckIfTimeOfDayIsInTimespan(timeOfDay, (8, 30), (14, 59)))
                return 8;
            else if (CheckIfTimeOfDayIsInTimespan(timeOfDay, (15, 0), (15, 29)))
                return 13;
            else if (CheckIfTimeOfDayIsInTimespan(timeOfDay, (15, 30), (16, 59)))
                return 18;
            else if (CheckIfTimeOfDayIsInTimespan(timeOfDay, (17, 0), (17, 59)))
                return 13;
            else if (CheckIfTimeOfDayIsInTimespan(timeOfDay, (18, 0), (18, 29)))
                return 8;
            else 
                return 0;
        }

         public static bool CheckIfTimeOfDayIsInTimespan(TimeSpan timeOfDay, (int hour, int minute) startTimespan, (int hour, int minute) endTimespan)
        {
            var startTime = new TimeSpan(startTimespan.hour, startTimespan.minute,0);
            var endTime = new TimeSpan(endTimespan.hour, endTimespan.minute, 0);

            if(timeOfDay>= startTime && timeOfDay<=endTime)
            {
                return true;
            }
            else
            {
                return false;
            }
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
