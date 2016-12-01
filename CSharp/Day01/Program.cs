using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var result = from part in File
                                        .ReadAllText("Input1.txt")
                                        .Split(',')
                                        .Select(part => part.Trim())
                            let left = Regex.Match(part,"L([0-9]*)")
                            let right = Regex.Match(part,"R([0-9]*)")
                            select new { 
                                Left = left.Success?int.Parse(left.Groups[1].Value):0,
                                Right = right.Success?int.Parse(right.Groups[1].Value):0
                            };

            var position = result.Aggregate(
                new { IncX = 0, IncY = 0 }, 
                (dir, src) => 
                {
                    return new { IncX = 1, IncY = 0 };
                });

            Console.WriteLine(position);
        }
    }
}
