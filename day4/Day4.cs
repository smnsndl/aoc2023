using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace aoc2023
{
    internal class Day4
    {
        public static void Run()
        {
            Console.WriteLine("--- Day 4: Scratchcards ---\r\n");
            var example = @"C:\Users\simsun\source\repos\aoc2023\day4\d4_example.txt";
            var fullInput = @"C:\Users\simsun\source\repos\aoc2023\day4\d4_input.txt";
            int part1 = 0;
            int part2 = 0;
            List<ScratchCard> cards = ParseInput(fullInput);
            cards.ForEach(card => card.CheckForWinnings());

            Console.WriteLine($"Part 1 Silver: {PartOneGetScoreTotal(cards)}");

            int result = PartTwo(cards);//.OrderBy(o=>o.ActualWinningNums.Count).ToList());
            Console.WriteLine($"Part 2 Gold: {result}");               
        }

        private static List<ScratchCard> ParseInput(string path)
        {
            List<ScratchCard> cards = new();
            foreach (var line in File.ReadLines(path))
            {
                //Console.WriteLine($"parsing {line}");
                var idSep = ": ";
                var numberSep = " | ";
                var cardSplit = line.Split(idSep);
                var cardId = cardSplit[0];
                var numbersSplit = cardSplit[1].Split(numberSep);
                var winningNumbers = numbersSplit[0].Split(" ")
                    .Where(x => int.TryParse(x, out int res)).Select(int.Parse).ToList();
                var numbers = numbersSplit[1].Split(" ")
                    .Where(x => int.TryParse(x, out int res)).Select(int.Parse).ToList();
                var newCard = new ScratchCard(cardId,winningNumbers, numbers);
                cards.Add(newCard);
            }
            return cards;
        }

        private static int PartOneGetScoreTotal(List<ScratchCard> cards)
        {
            return cards.Select(x => x.Score).Sum();
        }

        private static int PartTwo(List<ScratchCard> cards)
        {
            int totalScratchCards = 0;
            List<ScratchCard> copies = new();
            for(int i = 0; i < cards.Count; i++)
            {
                var currentCard = cards[i];
                var howManyToCopy = currentCard.ActualWinningNums.Count;
                if (howManyToCopy > 0)
                {
                    foreach (var copy in cards.Skip(i + 1).Take(howManyToCopy))
                    {
                        //Console.WriteLine($"Inserting {copy} @ {i + 1}");
                        copies.Add(copy);
                    }

                    var currentCopies = copies.Where(x => x.Id == currentCard.Id).ToList(); ;
                    foreach (var copy in currentCopies)
                    {
                        cards.Skip(i + 1).Take(copy.ActualWinningNums.Count()).ToList()
                            .ForEach(copies.Add);
                    }
                }
               
            }
            //Console.Write("Original cards:");
            //cards.ForEach(Console.WriteLine);
            //Console.WriteLine("COPIES:");
            //copies.ForEach(Console.WriteLine);

            return cards.Count + copies.Count;
        }


        public class ScratchCard
        {
            public string Id { get; set; }
            public List<int> WinningNums { get; set; }
            public List<int> Numbers { get; set; }
            public List<int> ActualWinningNums { get; set; }
            public int Score { get; set; } = 0;

            public ScratchCard(string id, List<int> winningNumbers, List<int> numbers)
            {
                Id = id;
                WinningNums = winningNumbers;
                Numbers = numbers;
            }


            public void CalculateScore()
            {
                Score = IncreaseScore(1, ActualWinningNums.Count);
            }
            /// <summary>
            /// Doubles score x times 
            /// </summary>
            /// <param name="score"></param>
            /// <param name="x"></param>
            /// <returns>Doubled score</returns>
            public static int IncreaseScore(int score, int x)
            {
                // start from 1 to "skip first card" doubling
                for (int i = 1; i < x; i++)
                {
                    //Console.WriteLine($"Score:{score}, double {i}");
                    //Console.WriteLine(score);
                    score *= 2;
                }
                return score;
            }

            public void CheckForWinnings()
            {
  
                ActualWinningNums = Numbers.Intersect(WinningNums).ToList();
                if (ActualWinningNums.Any())
                    CalculateScore();
            }

            public override string ToString()
            {
                return $"{Id} - Numbers {string.Join(",",Numbers)} - WinningNumbers {string.Join(",",WinningNums)} - ActualWin({ActualWinningNums.Count}) {string.Join(",", ActualWinningNums)}, - Score {Score}";
            }

        }
    }
}
