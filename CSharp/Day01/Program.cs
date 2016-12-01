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
            var result = 
				Math.Abs(
					(from part in File
						.ReadAllText("Input1.txt")
						.Split(',')
					let mov = Regex.Match(part,"([LR])([0-9]*)")
					where mov.Success
					select new { 
						Right = mov.Groups[1].Value == "R"?-1:1,
						Blocks = int.Parse(mov.Groups[2].Value)
					})
					.Aggregate(
						new 
						{ 
							IncX = 0, IncY = 1, // North
							Distance = 0 
						}, 
						(acum, next) => 
							new 
							{ 
								IncX = - acum.IncY * next.Right, 
								IncY = acum.IncX * next.Right, 
								Distance = acum.Distance + next.Right * next.Blocks * (acum.IncX - acum.IncY) 
							}
						)
					.Distance);

            Console.WriteLine(result);
        }
    }
}
