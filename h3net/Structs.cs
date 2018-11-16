using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace H3Net {
    public static partial class Api
    {
        public struct GeoCoord
        {
            public double Latitude;
            public double Longitude;

            public GeoCoord(double lat, double lng):this()
            {
                Latitude = lat;
                Longitude = lng;
            }

            public GeoCoord(Code.GeoCoord gc)
            {
                Latitude = gc.lat;
                Longitude = gc.lon;
            }

            public static explicit operator GeoCoord(Code.GeoCoord gc)
            {
                return new GeoCoord(gc.lat, gc.lon);
            }

            public static implicit operator Code.GeoCoord(GeoCoord gc)
            {
                return new Code.GeoCoord {lat = gc.Latitude, lon = gc.Longitude};
            }
        }

        public struct GeoBoundary
        {
            public int VertexCount;
            public GeoCoord[] Vertices;

            public GeoBoundary(int count, IEnumerable<GeoCoord> gc):this()
            {
                var verts = gc.ToArray();
                if (count > verts.Length)
                {
                    throw new ArgumentException();
                }

                VertexCount = count;
                Vertices = verts;
            }

            public GeoBoundary(IEnumerable<GeoCoord> gc)
            {
                if (gc == null || !gc.Any())
                {
                    throw new ArgumentException();
                }

                Vertices = gc.ToArray();
                VertexCount = Vertices.Length;
            }

            public GeoBoundary(Code.GeoBoundary cgb)
            {
                VertexCount = cgb.numVerts;
                List<GeoCoord> lgc = new List<GeoCoord>();
                foreach (var vertex in cgb.verts)
                {
                    lgc.Add(new GeoCoord(vertex));
                }

                Vertices = lgc.ToArray();
            }
        }

        public struct Geofence
        {
            public int VertexCount;
            public GeoCoord[] Vertices;
        }

        public struct GeoPolygon
        {
            public Geofence Fence;
            public int HoleCount;
            public Geofence[] Holes;
        }

        public struct GeoMultiPolygon
        {
            public int PolygonCount;
            public GeoPolygon[] Polygons;
        }

        public class LinkedGeoCoord
        {
            public GeoCoord Vertex;
            public LinkedGeoCoord Next;

            public LinkedGeoCoord(Code.LinkedGeo.LinkedGeoCoord codeLinkedGeoCoord)
            {
                Vertex = new GeoCoord {Latitude = codeLinkedGeoCoord.vertex.lat, Longitude = codeLinkedGeoCoord.vertex.lon};
                if (codeLinkedGeoCoord.next != null)
                {
                    Next = new LinkedGeoCoord(codeLinkedGeoCoord.next);
                }
            }

            public void Clear()
            {
                if (Next == null)
                {
                    return;
                }
                Next.Clear();
                Next = null;
            }
        }

        public class LinkedGeoLoop
        {
            public LinkedGeoCoord First;
            public LinkedGeoCoord Last;
            public LinkedGeoLoop Next;

            public LinkedGeoLoop(Code.LinkedGeo.LinkedGeoLoop codeLinkedGeoLoop)
            {
                if (codeLinkedGeoLoop.first != null)
                {
                    First = new LinkedGeoCoord(codeLinkedGeoLoop.first);
                }
                if (codeLinkedGeoLoop.last != null)
                {
                    Last = new LinkedGeoCoord(codeLinkedGeoLoop.last);
                }
                if (codeLinkedGeoLoop.next != null)
                {
                    Next = new LinkedGeoLoop(codeLinkedGeoLoop.next);
                }
            }

            public void Clear()
            {
                if (First != null)
                {
                    First.Clear();
                    First = null;
                }

                if (Last != null)
                {
                    Last.Clear();
                    Last = null;
                }

                if (Next != null)
                {
                    Next.Clear();
                    Next = null;
                }
            }
        }

        public class LinkedGeoPolygon
        {
            public LinkedGeoLoop First;
            public LinkedGeoLoop Last;
            public LinkedGeoPolygon Next;

            public LinkedGeoPolygon(Code.LinkedGeo.LinkedGeoPolygon codeLinkedGeoPolygon)
            {
                if (codeLinkedGeoPolygon.first != null)
                {
                    First = new LinkedGeoLoop(codeLinkedGeoPolygon.first);
                }
                if (codeLinkedGeoPolygon.last != null)
                {
                    Last = new LinkedGeoLoop(codeLinkedGeoPolygon.last);
                }
                if (codeLinkedGeoPolygon.next != null)
                {
                    Next = new LinkedGeoPolygon(codeLinkedGeoPolygon.next);
                }
            }

            public void Clear()
            {
                if (First != null)
                {
                    First.Clear();
                    First = null;
                }

                if (Last != null)
                {
                    Last.Clear();
                    Last = null;
                }

                if (Next == null)
                {
                    return;
                }
                Next.Clear();
                Next = null;
            }
        }

        public struct HexRangeDistancesResult
        {
            public int Result;
            public HexRangeMeasurement[] Values;
        }
        public struct HexRangeMeasurement
        {
            public H3Index Index;
            public int Distance;
        }

        public struct HexRangeResult
        {
            public int Result;
            public H3Index[] Indexes;
        }
        public struct CoordIJ
        {
            public int I;
            public int J;
        }

        public struct ExperimentalIJ
        {
            public int Result;
            public CoordIJ IJ;
        }

        public struct H3Index
        {
            public ulong Value;

            public H3Index(Code.H3Index ch3)
            {
                Value = ch3.value;
            }

            public H3Index(H3Index h)
            {
                Value = h.Value;
            }

            public override string ToString()
            {
                return Value.ToString("X");
            }

            public static explicit operator H3Index(ulong u)
            {
                return new H3Index(u);
            }

            public static implicit operator Code.H3Index(H3Index h3)
            {
                return new Code.H3Index(h3.Value);
            }
        }
    }
}