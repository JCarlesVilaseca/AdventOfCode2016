using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var input = "cxdnnyjw";

            var md5 = MD5.Create();

            var part1 = 
                from index in Enumerable.Range(1,int.MaxValue)
                let candidate = input + index
                let hash = md5.ComputeHash(Encoding.UTF8.GetBytes(candidate)) 
                let sixth = hash[2] & 0x0F
                where hash.Take(2).All(x => x == 0) && hash[2] <= 0x0F
                select sixth.ToString("x");

            Console.WriteLine("Part 1: " + string.Join("",part1.Take(8)));

            var part2 =
                (from index in Enumerable.Range(1,int.MaxValue)
                let candidate = input + index
                let hash = md5.ComputeHash(Encoding.UTF8.GetBytes(candidate)) 
                let position = hash[2] & 0x0F
                where hash[0] == 0 && hash[1] == 0 && hash[2] <= 0x0F && position <= 7
                select new { Position = position, Chr = hash[3].ToString("x2").First() })
                .AggregateWhile(
                    "________",
                    (aa, next) => aa[next.Position] != '_'?aa:aa.Substring(0,next.Position)+next.Chr+aa.Substring(next.Position+1),
                    (aa) => aa.Any(x => x == '_')		
                );

            Console.WriteLine("Part 2: " + part2);
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
