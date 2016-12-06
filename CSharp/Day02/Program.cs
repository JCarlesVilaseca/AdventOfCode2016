using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var lines = File.ReadAllLines("Input1.txt");

            Func<int, char, int> movementPart1 = (position, direction) => {
                switch(direction)
                {
                    case 'U': return position-3 < 0?position:position-3;
                    case 'D': return position+3 > 8?position:position+3;
                    case 'L': return (position%3)==0?position:position-1;
                    case 'R': return ((position+1)%3)==0?position:position+1;
                }
                
                return position;
            };

            Func<IEnumerable<string>, Func<int, char, int>, IEnumerable<int>> solve = (data, movement) =>

                data.Aggregate(
                    (IEnumerable<int>)new[] { 4 },
                    (list,line) => {
                        var current = list.Last();
                        var next = 
                            line.Aggregate(
                                current,
                                movementPart1
                            );
                        return list.Concat(new [] { next });
                    })
                    .Select(x => x + 1) // My grid is base(0)
                    .Skip(1); // Skip code seed
                    
            var part1 = solve(lines, movementPart1);

            Console.WriteLine("Part 1: " +  string.Join("",part1)); 

            // (Unfinished)
            Func<int, char, int> movementPart2 = (position, direction) => {
                /*
                    0
                  1 2 3
                4 5 6 7 8
                  9 A B
                    C

                */
                switch(direction)
                {
                    case 'U': return position;
                    case 'D': return position;
                    case 'L': return position;
                    case 'R': return position;
                }
                
                return position;
            };

            var part2 = solve(lines, movementPart2);

            Console.WriteLine("Part 2: " +  string.Join("",part2.Select(x => x.ToString("X")))); 
        }
    }
}
