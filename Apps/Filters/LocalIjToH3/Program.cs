using System;
using System.Linq;
using System.Text;
using CommandLineParser.Arguments;
using H3Lib;
using H3Lib.Extensions;

namespace LocalIjToH3
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
            var origin = new H3Index(argParser.Origin);

            var ij = new CoordIj(argParser.I, argParser.J);

            var (status, cell) = ij.ToH3Experimental(origin);

            Console.WriteLine
                (
                 status != 0
                     ? "NA"
                     : cell.ToString()
                );
        }
    }

    public class HexRangeArguments
    {
        [BoundedValueArgument(typeof (int),'i', Optional = false, Description = "I index")]
        public int I;

        [BoundedValueArgument(typeof(int), 'j', Optional = false, Description = "J index")]
        public int J;
        
        [BoundedValueArgument(typeof(ulong), 'o', "origin", Optional = false)]
        public ulong Origin;

    }

}
