using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var data =
                from part in File.ReadAllLines("Input1.txt")
                let sides = Regex.Match(part, @"\s*([0-9]*)\s*([0-9]*)\s*([0-9]*)")
                where sides.Success
                select new { 
                    X = int.Parse(sides.Groups[1].Value),
                    Y = int.Parse(sides.Groups[2].Value),
                    Z = int.Parse(sides.Groups[3].Value)
                };

            Func<int, int, int, bool> isTriangle = (int x, int y, int z) => 
                x + y > z
                && x + z > y
                && y + z > x; 

            var part1 = 
                from tri in data
                where isTriangle(tri.X, tri.Y, tri.Z)
                select tri;

            Console.WriteLine("Part1: " + part1.Count());

            var part2 = (
                from block in data.Select( (p, i) => new { Triangle = p, Index = i})
                group block.Triangle by block.Index / 3 into lines
                let firstLine = lines.ElementAt(0)
                let secondLine = lines.ElementAt(1)
                let thirdLine = lines.ElementAt(2)
                select new[] { 
                    new { x = firstLine.X, y = secondLine.X, z = thirdLine.X },
                    new { x = firstLine.Y, y = secondLine.Y, z = thirdLine.Y },
                    new { x = firstLine.Z, y = secondLine.Z, z = thirdLine.Z },
                })
                .SelectMany(t => t)
                .Where(t => isTriangle(t.x, t.y, t.z));

            Console.WriteLine("Part2: " + part2.Count()); 
        }
    }
}
