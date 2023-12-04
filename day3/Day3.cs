namespace aoc2023;
internal class Day3
{

    public static void Run()
    {
        Console.WriteLine("--- Day 3: Gear Ratios ---");
        var example = @"C:\Users\simsun\source\repos\aoc2023\day3\d3_example.txt";
        var fullInput = @"C:\Users\simsun\source\repos\aoc2023\day3\d3_input.txt";

        var schematicLines = ParseInput(fullInput);

        var (numbers,symbols) = BuildSchematic(schematicLines);
        var part1 = SumPartNumbers(numbers,symbols);
        var part2 = SumGearPartNumbers(numbers,symbols);
        Console.WriteLine($"Part 1 Silver: {part1}");
        Console.WriteLine($"Part 2 Gold: {part2}");


    }

    private static List<string> ParseInput(string path)
    {
        List<string> parsedSchematic = new();
        foreach(var line in File.ReadLines(path))
        {
            parsedSchematic.Add(line);
        }
        return parsedSchematic;
    }

    private static (List<Number>, List<Symbol>) BuildSchematic(List<string> lines)
    {
        char VOID = '.';
        var numbers = new List<Number>();
        var symbols = new List<Symbol>();

        for (var y = 0; y < lines.Count; y++)
        for (var x = 0; x < lines[0].Length; x++)
        {
            if (lines[y][x] == VOID) continue;

            if (!char.IsDigit(lines[y][x]))
            {
                symbols.Add(new Symbol(lines[y][x], new PositionXY(x, y)));
                continue;
            }

            List<PositionXY> numberPositions = new();
            numberPositions.Add(new PositionXY(x,y));
            int searchSpan = 1;
            while (x + searchSpan < lines[0].Length && char.IsDigit(lines[y][x + searchSpan]))
            {
                numberPositions.Add(new PositionXY(x + searchSpan, y));
                searchSpan++;
            }
            var completeValue = int.Parse(lines[y][x..(x + searchSpan)]);
            Number number = new(completeValue, numberPositions);
            numbers.Add(number);
            x += searchSpan - 1;
        }


        return (numbers, symbols);
    }


    private static int SumPartNumbers(List<Number> numbers, List<Symbol> symbols)
    {
        List<Number> adjacentValues = new();
        List<Number> notAdjacentValues = new();

        foreach(var num in numbers)
        {
            bool adjacent = false;
            bool gearAdjacent = false;
            foreach(var sym in symbols)
            {
                foreach(var numPos in num.Positions)
                {
                    adjacent = false;

                    int dy = Math.Abs(sym.Position.Y - numPos.Y);
                    int dx = Math.Abs(sym.Position.X - numPos.X);
                    //Console.WriteLine($"Checking numPos {numPos} - symbol {sym} - deltas {dx} {dy}");
                    if (dy<=1 && dx<=1)
                    {
                        adjacent = true;
                        break;
                    }

                }
                if(adjacent) break;
            }
            if(adjacent) {
                //Console.WriteLine($" adjacent {num}");
                adjacentValues.Add(num);
            }
        }

        //adjacentValues.ForEach(Console.WriteLine);
        return adjacentValues.Select(x=>x.Value).Sum();
    }


    private static int SumGearPartNumbers(List<Number> numbers, List<Symbol> symbols)
    {
        List<int> gearAdjacentMultipliedValues = new();
        char GEAR = '*';

        foreach(var sym in symbols.Where(x=>x.Value == GEAR))
        {
            HashSet<int> adjacentsGearNumbers = new();
            foreach(var num in numbers)
            {
                bool adjacent = false;
                foreach(var numPos in num.Positions)
                {
                    int dy = Math.Abs(sym.Position.Y - numPos.Y);
                    int dx = Math.Abs(sym.Position.X - numPos.X);
                    //Console.WriteLine($"Checking numPos {numPos} - symbol {sym} - deltas {dx} {dy}");
                    if (dy<=1 && dx<=1)
                    {
                        //Console.WriteLine($"ADJACENT!!");
                        adjacentsGearNumbers.Add(num.Value);
                        adjacent = true;
                    }

                }
            }


            if(adjacentsGearNumbers.Count == 2)
            {
                int val = adjacentsGearNumbers.First() * adjacentsGearNumbers.Last();
                //Console.WriteLine($"{adjacentsGearNumbers.First()} * {adjacentsGearNumbers.Last()} = {val}");
                gearAdjacentMultipliedValues.Add(val);
            }
        }

        return gearAdjacentMultipliedValues.Sum();
    }


    private class Number
    {
        public int Value {get;set;}
        public List<PositionXY> Positions {get;set;}

        public Number(int value, List<PositionXY> positions)
        {
            Value = value;
            Positions = positions;
        }

        public override string ToString()
        {
            return $"<NUMBER> {Value} Positions: {string.Join(";", Positions)}";
        }

    }

    private class Symbol
    {
        public char Value {get;set;}
        public PositionXY Position {get;set;}

        public Symbol(char value, PositionXY position)
        {
            Value = value;
            Position = position;
        }

         public override string ToString()
        {
            return $"<SYMBOL> {Value} Position: {Position}";
        }
    }

    private struct PositionXY
    {
        public int X {get;set;}
        public int Y {get;set;}
        public PositionXY(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"X:{X},Y:{Y}";
        }
    }

}
