using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var lines =
                from part in File.ReadAllLines("Input1.txt")
                select part.Split(' ');
            
            var vm = new VM(lines.Count());

            var instructions = 
                lines.SelectMany((instr,index) => 
                new [] {
                    (Expression)Expression.Label(vm.labels[index]), 
                    (Expression)vm.instructions[instr[0]](index,instr[1],instr.Length>2?instr[2]:null) });

            var part1 = Expression.Block(
                vm.registers.Values,
                instructions
                    .Concat(new [] { vm.registers["a"] })
                );
            
            Console.WriteLine("Part1: " + Expression.Lambda<Func<int>>(part1).Compile().Invoke());

            var part2 = Expression.Block(
                vm.registers.Values,
                new[] 
                { 
                    Expression.Assign(vm.registers["c"],Expression.Constant(1)) 
                }
                    .Concat(instructions)
                    .Concat(new [] { vm.registers["a"] })
                );
            
            Console.WriteLine("Part2: " + Expression.Lambda<Func<int>>(part2).Compile().Invoke());
        }
    }
    class VM
    {
        public Dictionary<string,Func<int,string,string,Expression>> instructions;
        public Dictionary<string,ParameterExpression> registers = new Dictionary<string,ParameterExpression>
        {
            { "a", Expression.Variable(typeof(int), "a") },
            { "b", Expression.Variable(typeof(int), "b") },
            { "c", Expression.Variable(typeof(int), "c") },
            { "d", Expression.Variable(typeof(int), "d") }
        };

        public LabelTarget[] labels;

        public VM(int length) {
            instructions = new Dictionary<string,Func<int,string,string,Expression>>
            {
                { "cpy", cpy }, { "dec", dec }, { "inc", inc}, {"jnz", jnz}
            };
            labels = Enumerable.Range(0,length).Select(x => Expression.Label()).ToArray();
        }

        Expression ValueOrRegister(string param) =>
            "abcd".Any(x => x == param[0])?(Expression)registers[param]:Expression.Constant(int.Parse(param));
        Expression cpy(int IP, string org, string dst) => 
            Expression.Assign( registers[dst], ValueOrRegister(org));
        
        Expression inc(int IP, string org, string _) => 
            Expression.AddAssign( registers[org], Expression.Constant(1));

        Expression dec(int IP, string org, string _) => 
            Expression.SubtractAssign( registers[org], Expression.Constant(1));

        Expression jnz(int IP, string value, string jump) => 
            Expression.Condition(
                Expression.MakeBinary(ExpressionType.NotEqual,ValueOrRegister(value),Expression.Constant(0)),
                Expression.Goto(labels[IP + int.Parse(jump)]),
                Expression.Empty()
            );
    }
}
