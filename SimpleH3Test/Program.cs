using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using h3net.API;

namespace SimpleH3Test
{
    class Program
    {
        static void Main(string[] args)
        {
            List<H3Index> cells =
                new List<H3Index>
                {
                    0x8CA021560C543FF, 0x8CA02182E2C8BFF, 0x8CA0218E3A247FF, 0x8CA0219984AE5FF, 0x8CA021D40AC9BFF,
                    0x8CA021D42382BFF, 0x8CA021D514443FF, 0x8CA021D554AABFF, 0x8CA021D7331D9FF, 0x8CA0250EDA74BFF,
                    0x8CA02C85390E3FF, 0x8CA02C91058DBFF, 0x8CA02C911D0B3FF, 0x8CA02CB4488B3FF, 0x8CA02CD62CE27FF,
                    0x8CA02DB2DD6BDFF, 0x8CA06482420A7FF, 0x8CA064824953DFF, 0x8CA0648261B31FF, 0x8CA06482DAAB3FF,
                    0x8CA064834A6A5FF
                };


            foreach (var cell in cells)
            {
                var gb = new GeoBoundary();
                H3Index.h3ToGeoBoundary(cell, ref gb);
                string s = "";
                H3Index.h3ToString(cell, ref s, 17);
                Console.WriteLine(s);
                for(int i = 0; i < gb.numVerts;i++)
                {
                    var vert = gb.verts[i];
                    Console.WriteLine
                        (
                         "\t{0} {1}",
                         GeoCoord.radsToDegs(vert.lat),
                         GeoCoord.radsToDegs(vert.lon)
                        );
                }
            }

            Console.ReadLine();
        }
    }
}
