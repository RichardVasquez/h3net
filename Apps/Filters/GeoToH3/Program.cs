﻿using System;
using System.Linq;
using CommandLineParser.Arguments;
using H3Lib;
using H3Lib.Extensions;

namespace GeoToH3
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            using var parser = new CommandLineParser.CommandLineParser();
            args = args.Select(s => s.ToLower()).ToArray();

            try
            {
                var argParser = new GeoToH3Parser();
                parser.ExtractArgumentAttributes(argParser);
                parser.ParseCommandLine(args);
                ProcessParser(argParser);
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to parse input.");
                parser.ShowUsage();
            }
        }

        private static void ProcessParser(GeoToH3Parser target)
        {

            var h3 = new GeoCoord(((decimal)target.Latitude).DegreesToRadians(), ((decimal)target.Longitude).DegreesToRadians())
               .ToH3Index(target.Resolution);

            Console.WriteLine(h3.ToString());
        }
    }

    public class GeoToH3Parser
    {
        [BoundedValueArgument(typeof(double), "latitude",
                              MinValue = -90.0, MaxValue = 90.0,
                              Aliases = new []{"lat"},
                              Optional = false,
                              Description = "Latitude in degrees")]
        public double Latitude;

        [BoundedValueArgument(typeof(double), "longitude",
                              MinValue = -180.0, MaxValue = 180.0,
                              Aliases = new[]{"lon"},
                              Optional = false,
                              Description = "Longitude in degrees")]
        public double Longitude;

        [BoundedValueArgument(typeof(int), 'r', "res",
                              MinValue = 0, MaxValue = 15,
                              Optional = false,
                              Description = "Resolution (0-15 inclusive)")]
        public int Resolution;
    }
}
