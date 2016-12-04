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
                let parts = Regex.Match(line, @"([a-z\-]*)\-([0-9]+)\[([a-z]+)\]")
                where parts.Success
                select new 
                { 
                    Name = parts.Groups[1].Value,
                    SectorID = int.Parse(parts.Groups[2].Value),
                    Checksum = parts.Groups[3].Value
                };

            var part1 =
                from room in data
                let orderedChars = 
                    from chr in room.Name.Replace("-","")
                    group chr by chr into g
                    orderby g.Count() descending, g.Key
                    select g.Key
                let orderedString = string.Join("",orderedChars)
                where orderedString.StartsWith(room.Checksum)
                select room.SectorID;

            Console.WriteLine("Part 1: " + part1.Sum());

            var part2 =
                from room in data
                let decryptedChars = 
                    from chr in room.Name
                    select chr=='-'?' ':Convert.ToChar(((((int)chr - (int)'a') + room.SectorID) % 26) + (int)'a')
                let decrypted = string.Join("",decryptedChars)
                where decrypted == "northpole object storage"
                select room.SectorID;

            Console.WriteLine("Part2: " + part2.FirstOrDefault());
        }
    }
}
