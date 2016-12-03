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
            var data = 
                from line in File.ReadAllLines("Input1.txt")
                select line;

            var part1 =
                data.Aggregate(
                    (IEnumerable<int>)new[] { 4 },
                    (list,line) => {
                        var current = list.Last();
                        var next = 
                            line.Aggregate(
                                current,
                                (acum, direction) => {
                                    switch(direction)
                                    {
                                        case 'U': return acum-3 < 0?acum:acum-3;
                                        case 'D': return acum+3 > 8?acum:acum+3;
                                        case 'L': return (acum%3)==0?acum:acum-1;
                                        case 'R': return ((acum+1)%3)==0?acum:acum+1;
                                    }
                                    
                                    return acum;
                                }
                            );
                        return list.Concat(new [] { next });
                    })
                    .Select(x => x + 1) // My grid is base(0)
                    .Skip(1); // Skip code seed
                    
            Console.WriteLine("Part 1: " +  string.Join("",part1)); 
        }
    }
}
