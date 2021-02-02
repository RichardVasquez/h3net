using System;
using System.Linq;
using CommandLineParser.Arguments;
using H3Lib;
using H3Lib.Extensions;

namespace HexRange
{
    class Program
    {
        static void Main(string[] args)
        {
            using var parser = new CommandLineParser.CommandLineParser();

            args = args.Select(s => s.ToLower()).ToArray();

            try
            {
                var argParser = new HexRangeArguments();
                parser.ExtractArgumentAttributes(argParser);
                parser.ParseCommandLine(args);
                ProcessArguments(argParser);
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to parse input.");
                parser.ShowUsage();
            }
        }

        private static void ProcessArguments(HexRangeArguments argParser)
        {
            var radius = argParser.Kradius;
            var origin = new H3Index(argParser.OriginH3);

            if (!origin.IsValid())
            {
                Console.WriteLine("Origin is invalid.");
                return;
            }

            (int status, var values) = origin.HexRange(radius);

            if (status != 0)
            {
                Console.WriteLine("0");
                return;
            }

            foreach (var value in values)
            {
                Console.WriteLine(value.ToString());
            }
        }
    }

    public class HexRangeArguments
    {
        [BoundedValueArgument(typeof(int), 'k', Optional = false, Description = "k radius")]
        public int Kradius;

        [BoundedValueArgument(typeof(ulong), 'o', "origin",  Optional = false, Description = "Origin H3Index")]
        public ulong OriginH3;
    }

}
