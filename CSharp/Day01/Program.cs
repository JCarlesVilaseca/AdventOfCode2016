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
                Math.Abs(
                    data
                    .Aggregate(
                        new 
                        { 
                            IncX = 0, IncY = 1, // North
                            Distance = 0 
                        }, 
                        (acum, next) => 
                            new 
                            { 
                                IncX = - acum.IncY * next.Direction, 
                                IncY = acum.IncX * next.Direction, 
                                Distance = acum.Distance + next.Direction * next.Blocks * (acum.IncX - acum.IncY) 
                            }
                        )
                    .Distance);

            Console.WriteLine(part1);

            var part2 = 
                data
                .Aggregate(
                    new
                    {
                        Segments = new List<Point>(),
                        Position = new Point { X = 0, Y = 0 },
                        IncX = 0, IncY = 1
                    },
                    (acum, next) => 
                        {
                            var newPosition = new Point
                            {
                                X = acum.Position.X - acum.IncY * next.Direction * next.Blocks,
                                Y = acum.Position.Y + acum.IncX * next.Direction * next.Blocks
                            };
                            acum.Segments.Add(newPosition);
                            return new 
                            { 
                                Segments = acum.Segments, 
                                Position = newPosition,
                                IncX = - acum.IncY * next.Direction, 
                                IncY = acum.IncX * next.Direction                                 
                            };
                        }
                );

            foreach(var point in part2.Segments)
                Console.WriteLine(point);
        }
    }

    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
