using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
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
            ulong part1 = SolvePart1(parsed);
            Console.WriteLine($"Part 1 Silver: {part1}");

            var newRace = Merger(parsed);
            Console.WriteLine(newRace.Time);
            Console.WriteLine(newRace.Distance);
            ulong part2 = SolvePart2(newRace);
            Console.WriteLine($"Part 2 Gold: {part2}");
        }

        private static ulong SolvePart1(List<Race> parsed)
        {
            var myBoat = new Boat();
            List<ulong> AllRacesAndWaysThatICanWin = [];
            for (ulong x = 0; (int)x < parsed.Count; x++)
            {
                List<ulong> SpeedsThatICanWin = [];
                Console.WriteLine($"time {parsed[(int)x].Time} dist {parsed[(int)x].Distance}");
                ulong timeLeftRace = parsed[(int)x].Time;
                for (ulong dist = 0; dist < parsed[(int)x].Distance; dist++)
                {
                    if (timeLeftRace <= 0) { break; }
                    myBoat.ChargeButton(dist);
                    //Console.WriteLine($"Holding button and charging : {myBoat.TravelSpeed}");
                    timeLeftRace = parsed[(int)x].Time - myBoat.TravelSpeed;
                    //Console.WriteLine($"time left on race: {timeLeftRace}");
                    if (timeLeftRace * myBoat.TravelSpeed > parsed[(int)x].Distance)
                    {
                        Console.WriteLine($"\tA winning travelspeed: {myBoat.TravelSpeed}, timeleft: {timeLeftRace}!!");
                        SpeedsThatICanWin.Add(myBoat.TravelSpeed);
                    }
                }
                Console.WriteLine($"How many ways to win: {SpeedsThatICanWin.Count}");
                AllRacesAndWaysThatICanWin.Add((ulong)SpeedsThatICanWin.Count);
            }
            AllRacesAndWaysThatICanWin.ForEach(Console.WriteLine);

            Console.WriteLine("-----");
            ulong mathPow = AllRacesAndWaysThatICanWin[0];
            for (var val = 1; val < AllRacesAndWaysThatICanWin.Count; val++)
            {
                Console.WriteLine($"Math powwing {mathPow} {AllRacesAndWaysThatICanWin[val]}={mathPow * AllRacesAndWaysThatICanWin[val]}");
                mathPow = mathPow * AllRacesAndWaysThatICanWin[val];
            }
            return mathPow;
        }

        private static ulong SolvePart2(Race newRace)
        {
            ulong wins = 0;
            for(ulong i = 0; i < newRace.Time + 1; i++)
                if (CalculateDistance(i, newRace.Time - i) > newRace.Distance)
                    wins++;
            return wins;
        }

        private static ulong CalculateDistance(ulong held, ulong remaining)
        {
            return remaining * held;
        }

        private static List<Race> ParseInput(string input)
        {
            List<ulong> TimeValues = [];
            List<ulong> DistanceValues = [];
            List<Race> races = [];
            foreach(var row in File.ReadLines(input))
            {
                if(row.Contains("Time:"))
                { 
                    //Console.WriteLine("TIME ");
                    foreach(var x in row.Split(" ").ToList())
                    {
                        ulong number;
                        bool success = ulong.TryParse(x, out number);
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
                        ulong number;
                        bool success = ulong.TryParse(x, out number);
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
                for(ulong x = 0; (int) x < TimeValues.Count; x++)
                {
                    races.Add(new Race(TimeValues[(int)x], DistanceValues[(int)x]));
                }
            }
            return races;
        }

        private static Race Merger(List<Race> races)
        {
            string newTime = "";
            string newDistance = "";

            foreach (var race in races)
            {
                newTime += race.Time;
                newDistance += race.Distance;
            }
 
            return new(ulong.Parse(newTime), ulong.Parse(newDistance));
        }
    }



    internal class Race
    {
        public ulong Time {get;set;} = 0;
        public ulong Distance {get;set;} = 0;
        public Race(ulong time, ulong distance)
        {
            Time = time;
            Distance = distance;
        }
    }

    internal class Boat
    {
        public ulong TravelSpeed {get; set;} = 0;

        public ulong ChargeButton(ulong howLong)
        {
            TravelSpeed = 0;
            for(ulong x = 0; x < howLong; x++)
            {
                TravelSpeed += 1;
            };
            return TravelSpeed;
        }

    }
}
