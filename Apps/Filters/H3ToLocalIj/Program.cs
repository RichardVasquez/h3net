using System;
using System.Linq;
using CommandLineParser.Arguments;
using H3Lib;
using H3Lib.Extensions;

namespace H3ToLocalIj
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
                var argParser = new H3ToLocalIjArguments();
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

        private static void ProcessArguments(H3ToLocalIjArguments argParser)
        {
            var origin = new H3Index(argParser.OriginH3);
            var index = new H3Index(argParser.IndexH3);

            if (!origin.IsValid())
            {
                Console.WriteLine("Origin is invalid.");
                return;
            }

            (int status, var result) = origin.ToLocalIjExperimental(index);

            Console.WriteLine
                (
                 status != 0
                     ? "NA"
                     : $"{result.I} {result.J}"
                );
        }
    }
    
    
    
    public class H3ToLocalIjArguments
    {
        [BoundedValueArgument(typeof(ulong), 'o', "origin", Optional = false, Description = "Origin H3Index")]
        public ulong OriginH3;

        [BoundedValueArgument(typeof(ulong), 'i', "index",  Optional = false, Description = "Index H3Index")]
        public ulong IndexH3;
    }

}
