using System;
using System.IO;
using System.Linq;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var data = 
				(from direction in "ULLRRDD"
				select direction)
				.Aggregate(
					1,
					(acum, next) => {
						switch(next)
						{
							case 'U': return acum-3 < 1?acum:acum-3;
							case 'D': return acum+3 > 9?acum:acum+3;
							case 'U': return acum-3 < 1?acum:acum-3;
							case 'D': return acum+3 > 9?acum:acum+3;
						}
					}
				);

        }
    }
}
