namespace aoc2023;
using System.Text.RegularExpressions;
using System.Collections.Generic;
class Day2
{

    public static Dictionary<string, int> limits = new Dictionary<string, int>(){
        {"red", 12},
        {"green", 13},
        {"blue", 14}
    };

    public static void Run()
    {
        Console.WriteLine("Hello, AOC2023 Day 2!");

        int testResult = Part1("/home/simon/Dokument/codespace/aoc2023/d2p1_demo.txt");
        Console.WriteLine(testResult);
        int realInputResult = Part1("/home/simon/Dokument/codespace/aoc2023/d2p1_input.txt");
        Console.WriteLine(realInputResult);

        int test2Result = Part2("/home/simon/Dokument/codespace/aoc2023/d2p1_demo.txt");
        Console.WriteLine(test2Result);
        test2Result = Part2("/home/simon/Dokument/codespace/aoc2023/d2p1_input.txt");
        Console.WriteLine(test2Result);


    }

    private static List<GameSet> GetGameSets(string path)
    {
        List<GameSet> games = new();
        foreach(var game in File.ReadLines(path))
        {
            //Console.WriteLine(game);
            string gameId = game.Split(": ")[0];
            int id = int.Parse(gameId.Replace("Game ", ""));
            //Console.WriteLine(id);
            string[] sets = game.Replace(gameId + ": ", "").Split(";");
            foreach(var set in sets)
            {
                int red = 0;
                int green = 0;
                int blue = 0;
                foreach(var color in set.Trim().Split(", "))
                {
                    //Console.WriteLine($"color: {color.Split(" ")[0]}");
                    if(color.Contains("red")) red = int.Parse(color.Split(" ")[0]);
                    if(color.Contains("blue")) blue = int.Parse(color.Split(" ")[0]);
                    if(color.Contains("green")) green = int.Parse(color.Split(" ")[0]);
                }
                GameSet newSet = new(id, red, green, blue);
                games.Add(newSet);
            }
        }
        return games;
    }

    private static int Part1(string path)
    {
        List<GameSet> games = GetGameSets(path);
        var possibleSets = new HashSet<int>();
        int previousGameSetFail = 0;
        foreach(var set in games)
        {
            if (set.Id == previousGameSetFail) continue;
            if(set.Red > limits["red"] || set.Green > limits["green"] || set.Blue > limits["blue"])
            {
                previousGameSetFail = set.Id;
                Console.WriteLine($"Game ID set {set.Id} outside limits" );
                possibleSets.Remove(set.Id);
            }
            else
            {
                Console.WriteLine($"Game ID set {set.Id} inside limits" );
                possibleSets.Add(set.Id);
            }
        }

        Console.WriteLine(string.Join(",", possibleSets));
        Console.WriteLine();
        return possibleSets.Sum();
    }

    private static int Part2(string path)
    {
        List<GameSet> games = GetGameSets(path);
        Console.WriteLine(games.Count);

        List<GameSet> maxGames = new();
        int previousId = 1;
        int maxRed = 0;
        int maxGreen = 0;
        int maxBlue = 0;

        foreach(var set in games.OrderBy(q => q.Id).ToList())
        {
            Console.WriteLine($"Current {set.Id} {set.Red} {set.Green} {set.Blue}");
            if(previousId != set.Id) {
                GameSet max = new(previousId, maxRed, maxGreen, maxBlue);
                maxGames.Add(max);
                maxRed=0;
                maxGreen=0;
                maxBlue=0;
            }

            if(set.Red >= maxRed) maxRed = set.Red;
            if(set.Green >= maxGreen) maxGreen = set.Green;
            if(set.Blue >= maxBlue) maxBlue = set.Blue;

            previousId = set.Id;

            if (set.Equals(games.Last()))
            {
                GameSet max = new(previousId, maxRed, maxGreen, maxBlue);
                maxGames.Add(max);
            }
        }

        List<int> powers = new();
        foreach(var game in maxGames)
        {
            int power = game.Red * game.Green * game.Blue;
            Console.WriteLine($"{game.Id} {game.Red} {game.Green} {game.Blue} = {power}");
            powers.Add(power);

        }
        return powers.Sum();
    }


    private class GameSet
    {
        public int Id {get;set;}
        public int Red {get;set;}
        public int Green {get;set;}
        public int Blue {get;set;}

        public GameSet(int id, int red, int green, int blue)
        {
            Id = id;
            Red = red;
            Green = green;
            Blue = blue;
        }
    }
}
