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
                from line in File.ReadAllLines("Input1.txt")
                let parts = Regex.Match(line, @"([a-z\-]*)([0-9]+)\[([a-z]+)\]")
                where parts.Success
                select new 
                { 
                    Name = parts.Groups[1].Value.Replace("-",""),
                    SectorID = int.Parse(parts.Groups[2].Value),
                    Checksum = parts.Groups[3].Value
                };

            var part1 =
                from room in data
                let orderedChars = from chr in room.Name
                        group chr by chr into g
                        orderby g.Count() descending, g.Key
                        select g.Key
                let orderedString = string.Join("",orderedChars)
                where orderedString.StartsWith(room.Checksum)
                select room.SectorID;

            Console.WriteLine("Part 1: " + part1.Sum());
        }
    }
}
