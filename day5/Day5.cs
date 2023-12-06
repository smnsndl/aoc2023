using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace aoc2023
{
    internal class Day5
    {
        public static void Run()
        {
            Console.WriteLine("--- Day 5: If You Give A Seed A Fertilizer ---");
            var example = @"./day5/d5_example.txt";
            var fullInput = @"./day5/d5_input.txt";
            var parsed = ParseInput(fullInput);

            Console.WriteLine("Printing Parsed Input");
            Console.WriteLine($"Seeds {string.Join(",", parsed.Seeds)}");
            foreach (KeyValuePair<string, List<Map>> map in parsed.Maps)
            {
                Console.WriteLine($"map.key: {map.Key}\nvalues:\n\t{string.Join("\n\t", map.Value)}");
            }
            Console.WriteLine("-----");
            ulong part1 = 0;// SolvePart1(parsed);
            ulong part2 = SolvePart2(parsed);
            Console.WriteLine($"Part 1 Silver: {part1}");
            Console.WriteLine($"Part 2 Gold: {part2}");
        }

        private static Almanac ParseInput(string path)
        {
            List<ulong> seeds = [];
            Dictionary<string, List<Map>> myMap = new();
            var rows = File.ReadAllLines(path);
            for (var rowId = 0; rowId < rows.Length; rowId++)
            {
                var line = rows[rowId];
                if (line.Contains("seeds: "))
                {
                    seeds = line.Replace("seeds: ", "")
                        .Split(" ")
                        .Where(x => ulong.TryParse(x, out ulong res))
                        .Select(ulong.Parse)
                        .ToList();
                }

                if (line.Contains("map:"))
                {
                    //Console.WriteLine("Found a map:");
                    var currentMapName = line.Replace(" map:", "");
                    List<Map> currentMapValues = [];
                    int span = 1;
                    while (rowId + span < rows.Length && !String.IsNullOrEmpty(rows[rowId + span]))
                    {
                        var mapValues = rows[rowId + span].Split(" ")
                            .Where(x => ulong.TryParse(x, out ulong res))
                            .Select(ulong.Parse)
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

        private static ulong SolvePart1(Almanac input)
        {
            List<List<ulong>> values = new();
            foreach (var seed in input.Seeds)
            {
                ulong currentValue = seed;
                List<ulong> currentSeedValues = [seed];
                Console.WriteLine($"Current seed {seed}");
                foreach (KeyValuePair<string, List<Map>> map in input.Maps)
                {
                    bool mapped = false;
                    Console.WriteLine($"\tmapping {map.Key} - value {currentValue}, original seed {seed}");
                    foreach (var currentMap in map.Value)
                    {
                        Console.WriteLine($"\tcurrentmap {currentMap}");
                        var (within, newValue) = GetNewSeedIfWithinRange(currentValue, currentMap);
                        if (within)
                        {
                            currentSeedValues.Add(newValue);
                            currentValue = newValue;
                            mapped = true;
                            break;
                        }
                    }
                    if (!mapped) { Console.WriteLine($"\t\tNot mapped adding {currentValue}"); currentSeedValues.Add(currentValue); }

                }
                values.Add(currentSeedValues);
            }
            values.ForEach(val => Console.WriteLine(string.Join(",", val)));


            List<ulong> lastValues = [];
            values.ForEach(val => lastValues.Add(val.Last()));
            return lastValues.Min();
        }

        private static ulong SolvePart2(Almanac input)
        {
            Console.WriteLine("Trying to solve part2");

            List<List<ulong>> values = new();
            ulong smallestLocationValue = ulong.MaxValue;

            foreach (var seeds in input.Seeds.Chunk(2))
            {
                Console.WriteLine(string.Join(",", seeds));
                ulong startId = seeds[0];
                for (ulong seed = startId; seed < startId + seeds[1]; seed++)
                {
                  //  Console.WriteLine($"Trying to map current seed {seed}");
                    ulong currentValue = seed;
                    List<ulong> currentSeedValues = [seed];

                    foreach (KeyValuePair<string, List<Map>> map in input.Maps)
                    {
                        bool mapped = false;
                        //Console.WriteLine($"\tmapping {map.Key} - value {currentValue}, original seed {seed}");
                        foreach (var currentMap in map.Value)
                        {
                          //  Console.WriteLine($"\tcurrentmap {currentMap}");
                            var (within, newValue) = GetNewSeedIfWithinRange(currentValue, currentMap);
                            if (within)
                            {
                             //   Console.WriteLine($"\tMapped {map.Key} - value {currentValue}, to {newValue}");
                                //currentSeedValues.Add(newValue);
                                currentValue = newValue;
                                mapped = true;
                                break;
                            }
                        }
                        /*if (!mapped)
                        {
                            continue;
                            Console.WriteLine($"\t\tNot mapped adding {currentValue}");
                            currentSeedValues.Add(currentValue);
                        }*/

                    }
                   // Console.WriteLine($"adding {currentValue}");
                   if(currentValue <= smallestLocationValue)
                   {
                        smallestLocationValue = currentValue;
                   }
                }
                // Console.WriteLine("---");
                // values.Add(currentSeedValues);
            }
            //Console.WriteLine(string.Join(";", lastValues));
            //values.ForEach(val => lastValues.Add(val.Last()));
            return smallestLocationValue;
        }

    /// <summary>
    /// Checks if seed is within range, if true returns 
    /// </summary>
    /// <param name="currentValue"></param>
    /// <param name="currentSeedValues"></param>
    /// <param name="mapped"></param>
    /// <param name="currentMap"></param>
    /// <returns></returns>
    private static (bool, ulong) GetNewSeedIfWithinRange(ulong currentValue, Map currentMap)
    {
        bool within = false;
        var destStart = currentMap.DestinationRangeStart;
        var sourceStart = currentMap.SourceRangeStart;
        var sourceEnd = sourceStart + currentMap.RangeLength;
        var newValue = currentValue;
        if (sourceStart <= currentValue && currentValue <= sourceEnd)
        {
            // Console.WriteLine($"\t\t*Currentvlaue {currentValue} within range {sourceStart} - {sourceEnd}");
            var spaces = currentValue - sourceStart;
            newValue = destStart + spaces;
            // Console.WriteLine($"\t\t*How much? {spaces}, destvalue: {newValue}");
            within = true;
        }
        return (within, newValue);
    }
}

internal class Almanac
{
    public List<ulong> Seeds { get; set; }
    public Dictionary<string, List<Map>> Maps { get; set; }
    public Almanac(List<ulong> seeds, Dictionary<string, List<Map>> maps)
    {
        this.Seeds = seeds;
        this.Maps = maps;
    }
}

internal struct Map
{
    public ulong DestinationRangeStart { get; set; }
    public ulong SourceRangeStart { get; set; }
    public ulong RangeLength { get; set; }

    public Map(ulong drs, ulong srs, ulong rl)
    {
        DestinationRangeStart = drs;
        SourceRangeStart = srs;
        RangeLength = rl;
    }

    public override string ToString() => $"{DestinationRangeStart},{SourceRangeStart},{RangeLength}";
}
    
}
