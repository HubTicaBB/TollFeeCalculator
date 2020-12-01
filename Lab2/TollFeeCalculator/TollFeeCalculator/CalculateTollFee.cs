using System;
using System.IO;

namespace TollFeeCalculator
{
    class Program
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

        private static DateTime[] Parse(string inputData)
        {
            string[] datesCSV = inputData.Split(", ");
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
            if (CheckFreeDate(date)) return 0; //bugg

            int hour = date.Hour;
            int minute = date.Minute;
            if (hour == 6 && minute >= 0 && minute <= 29) 
                return 8;
            else if (hour == 6 && minute >= 30 && minute <= 59) 
                return 13;
            else if (hour == 7 && minute >= 0 && minute <= 59) 
                return 18;
            else if (hour == 8 && minute >= 0 && minute <= 29) 
                return 13;
            else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) 
                return 8;
            else if (hour == 15 && minute >= 0 && minute <= 29) 
                return 13;
            else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) 
                return 18; //bugg
            else if (hour == 17 && minute >= 0 && minute <= 59) 
                return 13;
            else if (hour == 18 && minute >= 0 && minute <= 29) 
                return 8;
            else return 0;
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
