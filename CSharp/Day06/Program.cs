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
			var data = File.ReadAllLines("Input1.txt");

            var part1 = 
				from position in Enumerable.Range(0,data.First().Length)
				select 
					(from chr in (
						from line in data
						select line[position])
					group chr by chr into g
					orderby g.Count() descending
					select g.Key)
					.First();
					 
			Console.WriteLine("Part 1: " + string.Join("",part1));

            var part2 = 
				from position in Enumerable.Range(0,data.First().Length)
				select 
					(from chr in (
						from line in data
						select line[position])
					group chr by chr into g
					orderby g.Count() descending
					select g.Key)
					.Last();
			
			Console.WriteLine("Part 2: " + string.Join("",part2));
        }
    }
}
