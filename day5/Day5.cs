using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace aoc2023
{
    internal class Day5
    {
        public static void Run()
        {
            Console.WriteLine("--- Day 5: If You Give A Seed A Fertilizer ---");
            var example = @"./day5/d5_example.txt";
            var fullInput = @"./day5/d5_input.txt";
            var parsed = ParseInput(example);
            Console.WriteLine("Printing Parsed Input");
            Console.WriteLine($"Seeds {string.Join(",",parsed.Seeds)}");
            foreach(KeyValuePair<string, List<Map>> map in parsed.Maps) 
            {
                Console.WriteLine($"map.key: {map.Key}\nvalues:\n\t{string.Join("\n\t",map.Value)}");
            }
            Console.WriteLine("-----");
            long part1 = Solve(parsed);
            int part2 = 0;
            Console.WriteLine($"Part 1 Silver: {part1}");
            Console.WriteLine($"Part 2 Gold: {part2}");               
        }

        private static Almanac ParseInput(string path)
        {
            List<long> seeds = [];
            Dictionary<string, List<Map>> myMap = new();
            var rows = File.ReadAllLines(path);
            for (var rowId = 0; rowId < rows.Count(); rowId++)
            {
                var line = rows[rowId];
                if(line.Contains("seeds: "))
                {
                    seeds = line.Replace("seeds: ","")
                        .Split(" ")
                        .Where(x => long.TryParse(x, out long res))
                        .Select(long.Parse)
                        .ToList();
                }

                if(line.Contains("map:"))
                {
                    Console.WriteLine("Found a map:");
                    var currentMapName = line.Replace(" map:","");
                    List<Map> currentMapValues = [];
                    int span = 1;
                    while (rowId + span < rows.Length && !String.IsNullOrEmpty(rows[rowId+span]))
                    {
                        var mapValues = rows[rowId+span].Split(" ")
                            .Where(x => long.TryParse(x, out long res))
                            .Select(long.Parse)
                            .ToList();
                        currentMapValues.Add(
                            new Map()
                            {
                                DestinationRangeStart = mapValues[0],
                                SourceRangeStart = mapValues[1],
                                RangeLength = mapValues[2]
                            }
                            );
                        span++;
                    }
                    myMap.Add(currentMapName, currentMapValues);
                    rowId += span;
                }
            }
            return new Almanac(seeds, myMap);
        }

        private static long Solve(Almanac input) 
        {
            List<List<long>> values = new();
            foreach(var seed in input.Seeds)
            {          
                long currentValue = seed;
                List<long> currentSeedValues = [seed];
                Console.WriteLine($"Current seed {seed}");
                foreach(KeyValuePair<string, List<Map>> map in input.Maps) 
                {
                    bool mapped = false;
                    Console.WriteLine($"\tmapping {map.Key} - value {currentValue}, original seed {seed}");
                    foreach(var currentMap in map.Value)
                    {
                        Console.WriteLine($"\tcurrentmap {currentMap}");
                        var destStart = currentMap.DestinationRangeStart;
                        var sourceStart = currentMap.SourceRangeStart;
                        Console.WriteLine($"\t\tCalculated destStart {destStart} - sourceStart {sourceStart}");
                        for(int x = 0; x < currentMap.RangeLength; x++)
                        {
                            Console.WriteLine($"\t\tSOURCE: {sourceStart} => DEST {destStart}");
                            if (currentValue == sourceStart)
                            {
                                Console.WriteLine($"\t\tSeed: {seed} mapped to {map.Key}: {destStart}");
                                currentSeedValues.Add(destStart);
                                currentValue = destStart;
                                mapped = true;
                                break;
                            }
                            destStart+=1;
                            sourceStart+=1;
                        }
                    }
                    if(!mapped) { Console.WriteLine($"\t\tNot mapped adding {currentValue}"); currentSeedValues.Add(currentValue); }

                }
                values.Add(currentSeedValues);
            }
            values.ForEach(val => Console.WriteLine(string.Join(",",val)));
            

            List<long> lastValues = [];
            values.ForEach(val => lastValues.Add(val.Last()));
            return lastValues.Min();
        }
    }

    internal class Almanac 
    {
        public List<long> Seeds { get; set; }
        public Dictionary<string, List<Map>> Maps { get; set; }
        public Almanac(List<long> seeds, Dictionary<string, List<Map>> maps)
        {
            this.Seeds = seeds;
            this.Maps = maps;
        }
    }

    internal struct Map
    {
        public long DestinationRangeStart {get;set;}
        public long SourceRangeStart {get;set;}
        public long RangeLength {get;set;}

        public Map(long drs, long srs, long rl)
        {
            DestinationRangeStart = drs;
            SourceRangeStart = srs;
            RangeLength = rl;
        }

        public override string ToString() => $"{DestinationRangeStart},{SourceRangeStart},{RangeLength}";
    }
    
}
