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
            int part1 = 0;
            int part2 = 0;
            var parsed = ParseInput(example);
            Console.WriteLine("Printing Parsed Input");
            Console.WriteLine($"Seeds {string.Join(",",parsed.Seeds)}");
            foreach(KeyValuePair<string, List<Map>> map in parsed.Maps) 
            {
                Console.WriteLine($"map.key: {map.Key}\nvalues:\n\t{string.Join("\n\t",map.Value)}");
            }
            Console.WriteLine($"Part 1 Silver: {part1}");
            Console.WriteLine($"Part 2 Gold: {part2}");               
        }

        private static Almanac ParseInput(string path)
        {
            List<int> seeds = [];
            Dictionary<string, List<Map>> myMap = new();
            var rows = File.ReadAllLines(path);
            for (var rowId = 0; rowId < rows.Count(); rowId++)
            {
                var line = rows[rowId];
                if(line.Contains("seeds: "))
                {
                    seeds = line.Replace("seeds: ","")
                        .Split(" ")
                        .Where(x => int.TryParse(x, out int res))
                        .Select(int.Parse)
                        .ToList();
                }

                if(line.Contains("map:"))
                {
                    Console.WriteLine("Found a map:");
                    var currentMapName = line.Replace(" map:","");
                    List<Map> currentMapValues = [];
                    int span = 1;
                    while (rowId + span < rows.Count() && !String.IsNullOrEmpty(rows[rowId+span]))
                    {
                        var mapValues = rows[rowId+span].Split(" ")
                            .Where(x => int.TryParse(x, out int res))
                            .Select(int.Parse)
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


    }

    internal class Almanac 
    {
        public List<int> Seeds { get; set; }
        public Dictionary<string, List<Map>> Maps { get; set; }
        public Almanac(List<int> seeds, Dictionary<string, List<Map>> maps)
        {
            this.Seeds = seeds;
            this.Maps = maps;
        }
    }

    internal struct Map
    {
        public int DestinationRangeStart {get;set;}
        public int SourceRangeStart {get;set;}
        public int RangeLength {get;set;}

        public Map(int drs, int srs, int rl)
        {
            DestinationRangeStart = drs;
            SourceRangeStart = srs;
            RangeLength = rl;
        }

        public override string ToString() => $"{DestinationRangeStart},{SourceRangeStart},{RangeLength}";
    }
    
}
