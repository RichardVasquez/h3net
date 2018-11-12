using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H3Net.Code;
using NUnit.Framework;

namespace h3tests
{
    class TestH3ToGeo
    {
        public static string[] FileNameStrings()
        {
            return new[]
                   {
                       "bc05r08centers.txt", "bc05r09centers.txt", "bc05r10centers.txt", "bc05r11centers.txt",
                       "bc05r12centers.txt", "bc05r13centers.txt", "bc05r14centers.txt", "bc05r15centers.txt",
                       "bc14r08centers.txt", "bc14r09centers.txt", "bc14r10centers.txt", "bc14r11centers.txt",
                       "bc14r12centers.txt", "bc14r13centers.txt", "bc14r14centers.txt", "bc14r15centers.txt",
                       "bc19r08centers.txt", "bc19r09centers.txt", "bc19r10centers.txt", "bc19r11centers.txt",
                       "bc19r12centers.txt", "bc19r13centers.txt", "bc19r14centers.txt", "bc19r15centers.txt",
                       "rand05centers.txt", "rand06centers.txt", "rand07centers.txt", "rand08centers.txt",
                       "rand09centers.txt", "rand10centers.txt", "rand11centers.txt", "rand12centers.txt",
                       "rand13centers.txt", "rand14centers.txt", "rand15centers.txt"
                   };
        }


        public void assertExpected(H3Index h1, ref GeoCoord g1)
        {
            const double epsilon = 0.0001 * Constants.M_PI_180;
            // convert H3 to lat/lon and verify
            GeoCoord g2 = new GeoCoord();
            H3Index.h3ToGeo(h1, ref g2);
            Assert.True(GeoCoord.geoAlmostEqualThreshold(g2, g1, epsilon),
                     "got expected h3ToGeo output");

            // Convert back to H3 to verify
            int res = H3Index.h3GetResolution(h1);
            H3Index h2 = H3Index.geoToH3(ref g2, res);
            Debug.WriteLine($"{res}\t{h1.value}\t=>{h2.value}");
            Assert.True(h1 == h2, "got expected geoToH3 output");
        }

        //[Test, Sequential]
        public void mainTest([ValueSource("FileNameStrings")] string filename)
        {
            char[] spacing = {' '};
            string root = "C:\\Users\\catch\\source\\repos\\h3net\\h3tests\\bin\\Debug\\input";
            Debug.WriteLine(filename);
            try
            {
                using (StreamReader sr = new StreamReader($"{root}\\{filename}"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        double latDegs, lonDegs;
                        GeoCoord coord = new GeoCoord();
                        var parts = line.Split(spacing, StringSplitOptions.RemoveEmptyEntries);

                        H3Index h3 = H3Index.stringToH3(parts[0]);
                        //H3Index h3 = 617700169518678015;
                        bool testLat = double.TryParse(parts[1], out latDegs);
                        bool testLon = double.TryParse(parts[2], out lonDegs);
                        if (testLat && testLon && H3Index.h3IsValid(h3) != 0)
                        {
                            GeoCoord.setGeoDegs(ref coord, latDegs, lonDegs);
                            assertExpected(h3, ref coord);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
