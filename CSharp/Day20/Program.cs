using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var data =
                from part in File.ReadAllLines("Input1.txt")
								//new[] { "0-10" , "5-20", "21-50", "60-70", "60-90", "92-94", "96-100"}
                let range = part.Split('-')
				let low = ulong.Parse(range[0])
				orderby low
                select new {
					Low = low,
					High = ulong.Parse(range[1]) 
                };

			var part1 = data.AggregateWhile(
				new { Max = (ulong)0, Result = (ulong)0 }, 
				(acum, next) => 
				{
					if (next.Low <= acum.Max + 1)
						return new { Max = Math.Max(acum.Max, next.High), Result = (ulong)0 };
					else
						return new { Max = acum.Max, Result = acum.Max + 1 };
				},
				(acum) => acum.Result >= 0 
				);

            Console.WriteLine("Part1: " + part1.Result);

			var part2 = data.Aggregate(
				new { Max = (ulong)0, Result = (ulong)0 }, 
				(acum, next) => 
				{
					Console.Write(next.Low + "-" + next.High);

					if (next.Low <= acum.Max + 1) {
						Console.WriteLine(" Overlap");
						return new 
						{ 
							Max = Math.Max(acum.Max, next.High), 
							Result = acum.Result
						};
					}
					else {
						Console.WriteLine(" Acum: " + (next.Low - acum.Max - 1) + " Total: " + (acum.Result + (next.Low - acum.Max - 1)));
						return new 
						{ 
							Max = next.High, 
							Result = acum.Result + next.Low - acum.Max - 1
						};
					}
				} 
				);

			var result = part2.Result + (uint.MaxValue - part2.Max - (uint)1);
            Console.WriteLine("Part2: " + result );
        }

    }
	public static class LinqExtensions
	{
		public static T1 AggregateWhile<T1,T2>(this IEnumerable<T2> sequence, T1 acum, Func<T1, T2, T1> aggregate, Func<T1, bool> predicate)
		{
			foreach(var value in sequence)
			{
			acum = aggregate(acum, value);
			if (!predicate(acum)) break;
			}
			return acum;
		}

	}
}
