using System;
using System.Diagnostics;
using System.Linq;
using H3Lib;
using H3Lib.Extensions;

namespace Polyfill
{
    internal static class Program
    {
        private static readonly decimal[,] SfVerts =
        {
            {0.659966917655m, -2.1364398519396m}, {0.6595011102219m, -2.1359434279405m},
            {0.6583348114025m, -2.1354884206045m}, {0.6581220034068m, -2.1382437718946m},
            {0.6594479998527m, -2.1384597563896m}, {0.6599990002976m, -2.1376771158464m}
        };

        private static readonly decimal[,] AlamedaVerts =
        {
            {0.6597959342671712m, -2.133241848488897m}, {0.6597959348850178m, -2.133241848495878m},
            {0.6598352639563587m, -2.1331688423977755m}, {0.6601346536539207m, -2.13270417124178m},
            {0.6601594763880223m, -2.1326680320633344m}, {0.6601512007732382m, -2.1326594176574534m},
            {0.6598535076212304m, -2.1323049630593562m}, {0.6596565748646488m, -2.132069889917591m},
            {0.6594645035394391m, -2.131843148468039m}, {0.6593438094209757m, -2.1316994860539844m},
            {0.6591174422311021m, -2.131429776816562m}, {0.658849344286881m, -2.1311111485483867m},
            {0.6588348862079956m, -2.1310988536794455m}, {0.6586273138317915m, -2.131668420800747m},
            {0.6583729538174264m, -2.132370426573979m}, {0.6582479206289285m, -2.132718691911663m},
            {0.6582322393220743m, -2.1327614200082317m}, {0.6583003647098981m, -2.132837478687196m},
            {0.6584457274847966m, -2.132827956758973m}, {0.6585526679060995m, -2.1330231566043203m},
            {0.6587379099516777m, -2.1331602726234538m}, {0.6587273684736642m, -2.1332676321559063m},
            {0.6584638025857692m, -2.133305719954319m}, {0.6583545950288919m, -2.1334323622944993m},
            {0.6584427148370682m, -2.1335885223323947m}, {0.6584715236640714m, -2.133649780409862m},
            {0.6584715242505019m, -2.133649780481421m}, {0.658474662092443m, -2.1336459234695804m},
            {0.6591666596433436m, -2.1348354004882926m}, {0.6591809355063646m, -2.1348424115474565m},
            {0.6593477498700266m, -2.1351460576998926m}, {0.6597155087395117m, -2.1351049454274m},
            {0.6597337410387994m, -2.135113899444683m}, {0.6598277083823935m, -2.1351065432309517m},
            {0.659837290351688m, -2.1350919904836627m}, {0.6598391300107502m, -2.1350911731005957m},
            {0.6598335712627461m, -2.1350732321630828m}, {0.6597162034032434m, -2.134664026354221m},
            {0.6596785831942451m, -2.134651647657116m}, {0.6596627824684727m, -2.13458880305965m},
            {0.6596785832500957m, -2.134530719130462m}, {0.6596093592822273m, -2.13428052987356m},
            {0.6596116166352313m, -2.134221493755564m}, {0.6595973199434513m, -2.134146270344056m},
            {0.6595536764042369m, -2.1340805688066653m}, {0.6594611172376618m, -2.133753252031165m},
            {0.6594829406269346m, -2.1337342082305697m}, {0.6594897134102581m, -2.1337104032834757m},
            {0.6597920983773051m, -2.1332343063312775m}, {0.6597959342671712m, -2.133241848488897m}
        };

        private static readonly decimal[,] SouthernVerts =
        {
            {0.6367481147484843m, -2.1290865397798906m}, {0.6367481152301953m, -2.129086539469222m},
            {0.6367550754426818m, -2.128887436716856m}, {0.6367816002113981m, -2.1273204058681094m},
            {0.6380814125349741m, -2.127201274803692m}, {0.6388614350074809m, -2.12552061082428m},
            {0.6393520289210095m, -2.124274316938293m}, {0.639524834205869m, -2.122168447308359m},
            {0.6405714857447717m, -2.122083222593005m}, {0.640769478635285m, -2.120979885974894m},
            {0.6418936996869471m, -2.1147667448862255m}, {0.6419094141707652m, -2.1146521242709584m},
            {0.6269997808948107m, -2.1038647304637257m}, {0.6252080524974937m, -2.1195521728170457m},
            {0.626379700264057m, -2.1203708632511162m}, {0.6282200029232767m, -2.1210412050690723m},
            {0.6283657301211779m, -2.1219496416754393m}, {0.6305651783819565m, -2.123628532238016m},
            {0.6308259852882764m, -2.124225549648211m}, {0.6317049665784865m, -2.124887756638367m},
            {0.6323403882676475m, -2.1266205835454053m}, {0.6334397909415498m, -2.1277211741619553m},
            {0.6367481147484843m, -2.1290865397798906m}
        };

        private static void Main()
        {
            var sfVerts = Enumerable.Range(0, SfVerts.GetLength(0))
                                    .Select(s => new GeoCoord(SfVerts[s, 0], SfVerts[s, 1]))
                                    .ToArray();

            var sfGeoFence = new GeoFence() {NumVerts = sfVerts.Length, Verts = sfVerts};
            var sfGeoPolygon = new GeoPolygon() {GeoFence = sfGeoFence};

            var alamedaVerts = Enumerable.Range(0, AlamedaVerts.GetLength(0))
                                         .Select(s => new GeoCoord(AlamedaVerts[s, 0], AlamedaVerts[s, 1]))
                                         .ToArray();

            var alamedaGeoFence = new GeoFence() {NumVerts = alamedaVerts.Length, Verts = alamedaVerts};
            var alamedaGeoPolygon = new GeoPolygon() {GeoFence = alamedaGeoFence};

            var southernVerts = Enumerable.Range(0, SouthernVerts.GetLength(0))
                                         .Select(s => new GeoCoord(SouthernVerts[s, 0], SouthernVerts[s, 1]))
                                         .ToArray();

            var southernGeoFence = new GeoFence() {NumVerts = southernVerts.Length, Verts = southernVerts};
            var southernGeoPolygon = new GeoPolygon() {GeoFence = southernGeoFence};

            Console.WriteLine("Starting...");
            Console.WriteLine();
            Benchmark("polyfill SF", 500, sfGeoPolygon, 9, DoPolyFill);
            Benchmark("polyfill Alameda", 500, alamedaGeoPolygon, 9, DoPolyFill);
            Benchmark("polyfill Southern", 10, southernGeoPolygon, 9, DoPolyFill);
            
        }

        private static void DoPolyFill(GeoPolygon polygon, int resolution)
        {
            var cells = polygon.Polyfill(resolution);
            cells.Clear();
        }

        private static void Benchmark(string name, int iterations, GeoPolygon polygon, int resolution, Action<GeoPolygon, int> doPolyFill)
        {
            var sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < iterations;i++)
            {
                doPolyFill(polygon,resolution);
            }

            sw.Stop();

            long frequency = Stopwatch.Frequency;
            long nanosecondsPerTick = (1000L*1000L*1000L) / frequency; 

            var elapsed = sw.Elapsed;
            long nanosecondsLength = elapsed.Ticks * nanosecondsPerTick;

            decimal average = (decimal) nanosecondsLength / (iterations * 1000);
            
            Console.WriteLine($"\t{name}\t- {iterations} iterations");
            Console.WriteLine($"\t{average} microseconds average");
            Console.WriteLine();
        }
    }
}
