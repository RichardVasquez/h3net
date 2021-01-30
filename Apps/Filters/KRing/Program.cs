using System;
using System.Linq;
using System.Text;
using CommandLineParser.Arguments;
using H3Lib;
using H3Lib.Extensions;

namespace KRing
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser =
                new CommandLineParser.CommandLineParser();

            args = args.Select(s => s.ToLower()).ToArray();

            try
            {
                var argParser = new HexRangeArguments();
                parser.ExtractArgumentAttributes(argParser);
                parser.ParseCommandLine(args);
                ProcessArguments(argParser);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to parse input.");
                parser.ShowUsage();
            }
        }

        private static void ProcessArguments(HexRangeArguments argParser)
        {
            var radius = argParser.KRadius;
            var origin = new H3Index(argParser.Origin);
            var showDistance = argParser.Print;
            
            if (!origin.IsValid())
            {
                Console.WriteLine("0");
                return;
            }

            var lookup = origin.KRingDistances(radius);

            StringBuilder sb = new StringBuilder();
            foreach (var pair in lookup)
            {
                sb.Clear();
                sb.Append(pair.Key.ToString());
                if (showDistance)
                {
                    sb.Append($"   {pair.Value}");
                }
                Console.WriteLine(sb.ToString());
            }
        }
    }

    public class HexRangeArguments
    {
        [SwitchArgument("print-distances", false, Optional = true, Description = "Print Distances")]
        public bool Print;

        [BoundedValueArgument(typeof(int), 'k', Optional = false, Description = "K Ring radius")]
        public int KRadius;
        
        [BoundedValueArgument(typeof(ulong), 'o', "origin", Optional = false)]
        public ulong Origin;

    }

}
