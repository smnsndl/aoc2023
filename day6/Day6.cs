using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace aoc2023
{
    internal class Day6
    {
        public static void Run()
        {
            Console.WriteLine("--- Day 6: Wait For It ---");
            var example = @"./day6/d6_example.txt";
            var fullInput = @"./day6/d6_input.txt";
            var parsed = ParseInput(fullInput);
            Console.WriteLine("Printing Parsed Input");

            Console.WriteLine($"TIME {string.Join(",", parsed)}");

            var myBoat = new Boat(); 
            List<int> AllRacesAndWaysThatICanWin= [];
            for(int x = 0; x < parsed.Count; x++)
            {
                List<int> SpeedsThatICanWin= [];
                Console.WriteLine($"time {parsed[x].Time} dist {parsed[x].Distance}");
                int timeLeftRace = parsed[x].Time;
                for(int dist = 0; dist < parsed[x].Distance; dist++)
                {
                    if(timeLeftRace <= 0) { break; }
                    myBoat.ChargeButton(dist);
                    //Console.WriteLine($"Holding button and charging : {myBoat.TravelSpeed}");
                    timeLeftRace = parsed[x].Time - myBoat.TravelSpeed;
                    //Console.WriteLine($"time left on race: {timeLeftRace}");
                    if(myBoat.TravelSpeed*timeLeftRace > parsed[x].Distance)
                    {
                        Console.WriteLine($"\tA winning travelspeed: {myBoat.TravelSpeed}, timeleft: {timeLeftRace}!!");
                        SpeedsThatICanWin.Add(myBoat.TravelSpeed);
                    }
                }
                Console.WriteLine($"How many ways to win: {SpeedsThatICanWin.Count}");
                AllRacesAndWaysThatICanWin.Add(SpeedsThatICanWin.Count);
            }
            AllRacesAndWaysThatICanWin.ForEach(Console.WriteLine); 
            
            Console.WriteLine("-----");
            double mathPow = AllRacesAndWaysThatICanWin[0];
            for(var val = 1; val < AllRacesAndWaysThatICanWin.Count; val++)
            {
                Console.WriteLine($"Math powwing {mathPow} {AllRacesAndWaysThatICanWin[val]}={mathPow*AllRacesAndWaysThatICanWin[val]}");
                mathPow = mathPow*AllRacesAndWaysThatICanWin[val];
            }

            double part1 = mathPow;// SolvePart1(parsed);
            Console.WriteLine($"Part 1 Silver: {part1}");


            ulong part2 = 0;
            Console.WriteLine($"Part 2 Gold: {part2}");
        }

        private static List<Race> ParseInput(string input)
        {
            List<int> TimeValues = [];
            List<int> DistanceValues = [];
            List<Race> races = [];
            foreach(var row in File.ReadLines(input))
            {
                if(row.Contains("Time:"))
                { 
                    //Console.WriteLine("TIME ");
                    foreach(var x in row.Split(" ").ToList())
                    {
                        int number;
                        bool success = int.TryParse(x, out number);
                        if (success)
                        {
                            //Console.WriteLine($"Converted '{x}' to {number}.");
                            TimeValues.Add(number);
                        }
                    }
                }
                if(row.Contains("Distance: "))
                {
                    //Console.WriteLine("DISTANCE ");
                    foreach(var x in row.Split(" ").ToList())
                    {
                        int number;
                        bool success = int.TryParse(x, out number);
                        if (success)
                        {
                            //Console.WriteLine($"Converted '{x}' to {number}.");
                            DistanceValues.Add(number);
                        }
                    }
                }
            }
            if(TimeValues.Count == DistanceValues.Count)
            {
                for(int x = 0; x < TimeValues.Count; x++)
                {
                    races.Add(new Race(TimeValues[x], DistanceValues[x]));
                }
            }
            return races;
        }
    }

    internal class Race
    {
        public int Time {get;set;} = 0;
        public int Distance {get;set;} = 0;
        public Race(int time, int distance)
        {
            Time = time;
            Distance = distance;
        }
    }

    internal class Boat
    {
        public int TravelSpeed {get;set;} = 0;

        public int ChargeButton(int howLong)
        {
            TravelSpeed = 0;
            for(int x = 0; x < howLong; x++)
            {
                TravelSpeed += 1;
            };
            return TravelSpeed;
        }
    }
}
