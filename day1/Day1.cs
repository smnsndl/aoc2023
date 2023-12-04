using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace aoc2023
{
    internal class Day1
    {
        private static readonly string[] Day1DemoRows = [
            "1abc2",
            "pqr3stu8vwx",
            "a1b2c3d4e5f",
            "treb7uchet"];

        public static void Run()
        {
            Console.WriteLine("--- Day 1: Trebuchet?! ---");
            string input = "C:\\Users\\simsun\\source\\repos\\aoc2023\\day1\\d1_input.txt";
            int part1 = SolvePart1(input);
            int part2 = SolvePart2(input);
            Console.WriteLine($"Part 1 Silver : {part1}");
            Console.WriteLine($"Part 2 Gold : {part2}");

        }


        static int SolvePart1(string d1_input)
        {
            List<int> firstAndLastInts = [];
            foreach (string row in File.ReadLines(d1_input))
            {
                var myFoundNumbers = GetNumbersFromRow(row);
                int combo = GetFirstAndLastAsInt(myFoundNumbers);
                firstAndLastInts.Add(combo);
            }
            //firstAndLastInts.ForEach(val => Console.Write($"{val},"));
            return firstAndLastInts.Sum();
        }

        static int SolvePart2(string d1_input)
        {
            List<int> firstAndLastInts = [];
            foreach (string row in File.ReadLines(d1_input))
            {
                var myFoundNumbers = GetNumbersFromRow(ConvertStringsToInt(row));
                int combo = GetFirstAndLastAsInt(myFoundNumbers);
                firstAndLastInts.Add(combo);
            }
            //firstAndLastInts.ForEach(val => Console.Write($"{val},"));
            return firstAndLastInts.Sum();
        }


        static string ConvertStringsToInt(string row)
        {
            return row
                .Replace("one", "o1e")
                .Replace("two", "t2o")
                .Replace("three", "t3e")
                .Replace("four", "f4r")
                .Replace("five", "f5e")
                .Replace("six", "s6x")
                .Replace("seven", "s7n")
                .Replace("eight", "e8t")
                .Replace("nine", "n9e");
        }


        static List<int> GetNumbersFromRow(string row)
        {
            List<int> myFoundNumbers = [];
            foreach (char ch in row)
            {
                if (int.TryParse(ch.ToString(), out int chInt))
                {
                    myFoundNumbers.Add(chInt);
                }
            }
            return myFoundNumbers;
        }

        static int GetFirstAndLastAsInt(List<int> numbers)
        {
            int.TryParse($"{numbers.First()}{numbers.Last()}", out int combinedInt);
            return combinedInt;

        }

    }
}
