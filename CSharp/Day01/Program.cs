using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var data =
                from part in //File
                        //.ReadAllText("Input1.txt")
                        "R8, R4, R4, R8"
                        .Split(',')
                let mov = Regex.Match(part,"([LR])([0-9]*)")
                where mov.Success
                select new { 
                    Direction = mov.Groups[1].Value == "R"?-1:1,
                    Blocks = int.Parse(mov.Groups[2].Value)
                };

            var part1 = 
                data
                .Aggregate(
                    new 
                    { 
                        IncX = 0, IncY = 1, // Starting North
                        Distance = 0 
                    }, 
                    (acum, next) => 
                        new 
                        { 
                            IncX = - acum.IncY * next.Direction, 
                            IncY = acum.IncX * next.Direction, 
                            Distance = acum.Distance + next.Direction * next.Blocks * (acum.IncX - acum.IncY) 
                        }
                    );

            Console.WriteLine("Part 1: " + Math.Abs(part1.Distance));

			// (Unfinished)
            var part2 = 
                data
                .Aggregate(
                    new
                    {
                        Segments = (IEnumerable<Segment>)new [] { new Segment() },
                        IncX = 0, IncY = 1
                    },
                    (acum, next) => 
                        {
                            var last = acum.Segments.Last();

                            var newPosition = new Segment
                            {
                                startX = last.endX, startY = last.endY,
                                endX = last.endX - acum.IncY * next.Direction * next.Blocks,
                                endY = last.endY + acum.IncX * next.Direction * next.Blocks
                            };
                            return new 
                            { 
                                Segments = acum.Segments.Concat(new[] { newPosition }), 
                                IncX = - acum.IncY * next.Direction, 
                                IncY = acum.IncX * next.Direction                                 
                            };
                        }
                );

            foreach(var point in part2.Segments)
                Console.WriteLine(string.Format("({0},{1}) - ({2},{3})",point.startX,point.startY,point.endX,point.endY));
        }
    }

    class Segment
    {
        public int startX { get; set; }
        public int startY { get; set; }
        public int endX { get; set; }
        public int endY { get; set; }
    }
}
