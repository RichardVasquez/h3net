using System;
using System.Linq;
using System.Text;
using CommandLineParser.Arguments;
using H3Lib;
using H3Lib.Extensions;

namespace H3ToComponents
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser =
                new CommandLineParser.CommandLineParser();

            args = args.Select(s => s.ToLower()).ToArray();

            SwitchArgument verbose = new SwitchArgument('v', "verbose", "Show verbose debugging info", false);
            verbose.Optional = true;

            ValueArgument<H3Index> h3 = new ValueArgument<H3Index>
                ('h', "h3index", "H3Index (in hexadecimal) to examine");

            h3.ConvertValueHandler = value => value.ToH3Index();
            
            parser.Arguments.Add(verbose);
            parser.Arguments.Add(h3);

            try
            {
                var argParser = new H3ToComponentsParser();
                parser.ExtractArgumentAttributes(argParser);
                parser.ParseCommandLine(args);

                if (h3.Parsed)
                {
                    var data = new H3ToComponentsParser {Verbose = verbose.Value, H3 = h3.Value};
                    ProcessData(data);
                }
                else
                {
                    Console.WriteLine("Unable to parse input.");
                    parser.ShowUsage();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to parse input.");
                parser.ShowUsage();
            }
        }

        private static void ProcessData(H3ToComponentsParser data)
        {
            if (data.Verbose)
            {
                FancyDump(data.H3);
            }
            else
            {
                SimpleDump(data.H3);
            }
        }

        public static char ResDigitToChar(int d)
        {
            if (d < 0 || d > 7)
            {
                return 'x';
            }

            return (char) ('0' + d);
        }

        private static void SimpleDump(H3Index h3)
        {
            var sb = new StringBuilder();
            switch (h3.Mode)
            {
                case H3Mode.Hexagon:
                    sb.Append($"{(int)h3.Mode}:{h3.Resolution}:{h3.BaseCell}:");
                    for (int i = 1; i <= h3.Resolution;i++)
                    {
                        sb.Append(ResDigitToChar((int) h3.GetIndexDigit(i)));
                    }

                    break;
                case H3Mode.UniEdge:
                    sb.Append($"{(int)h3.Mode}:{h3.ReservedBits}:{h3.Resolution}:{h3.BaseCell}:");
                    for (int i = 1; i <= h3.Resolution;i++)
                    {
                        sb.Append(ResDigitToChar((int) h3.GetIndexDigit(i)));
                    }

                    break;
                default:
                    sb.Append("INVALID INDEX");
                    break;
            }
            
            Console.WriteLine(sb.ToString());
        }

        private static void FancyDump(H3Index h3)
        {
            var modes = new[]
                        {
                            "RESERVED", "Hexagon", "Unidirectional Edge", "Invalid",
                            "Invalid", "Invalid", "Invalid", "Invalid",
                            "Invalid", "Invalid", "Invalid", "Invalid",
                            "Invalid", "Invalid", "Invalid", "Invalid"
                        };
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("╔════════════╗");
            sb.AppendLine($"║ H3Index    ║ {h3.ToString()}");
            sb.AppendLine("╠════════════╣");
            sb.AppendLine($"║ Mode       ║ {modes[(int) h3.Mode]}, {(int) h3.Mode}");
            sb.AppendLine($"║ Resolution ║ {h3.Resolution}");
            if (h3.Mode == H3Mode.UniEdge)
            {
                sb.AppendLine($"║ Edge       ║ {h3.ReservedBits}");
            }
            sb.AppendLine($"║ Base Cell  ║ {h3.BaseCell}");
            for (int i = 1; i <= h3.Resolution; i++)
            {
                sb.AppendLine($"║ {i,2} Child   ║ {ResDigitToChar((int)h3.GetIndexDigit(i))}");
            }

            sb.AppendLine("╚════════════╝").AppendLine();

            Console.WriteLine(sb.ToString());
        }
    }
    
    public class H3ToComponentsParser
    {
        public bool Verbose;

        public H3Index H3;
    }
}
