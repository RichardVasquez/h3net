using System;
using System.Diagnostics;
using h3net.API;

namespace h3net.Types
{
    [DebuggerDisplay("N: {North} S: {South} E: {East} W: {West}")]
    public class Bbox : IEquatable<Bbox>
    {
        public double North { get; }
        public double South { get; }
        public double East { get; }
        public double West { get; }
        public bool IsTransmeridian => East < West;
        public GeoCoord Center { get; }

        public Bbox(double n, double s, double e, double w)
        {
            North = n;
            South = s;
            East = e;
            West = w;

            double lat = (North + South) / 2.0;
            var tempEast = IsTransmeridian
                               ? East + Constants.M_2PI
                               : East;
            double lon = GeoCoord.constrainLng((tempEast + West) / 2.0);
            Center = new GeoCoord(lat, lon);
        }

        public bool Contains(GeoCoord point)
        {
            return
                point.lat >= South &&
                point.lat <= North &&
                (
                    IsTransmeridian
                        // transmeridian case
                        ? point.lon >= West || point.lon <= East
                        // standard case
                        : point.lon >= West && point.lon <= East
                );
        }

        public int HexRadius(int hexResolution)
        {
            return 0;
        }

        public bool Equals(Bbox other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return North.Equals(other.North) &&
                   South.Equals(other.South) &&
                   East.Equals(other.East) &&
                   West.Equals(other.West);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == typeof(Bbox) &&
                   Equals((Bbox) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = North.GetHashCode();
                hashCode = (hashCode * 397) ^ South.GetHashCode();
                hashCode = (hashCode * 367) ^ East.GetHashCode();
                hashCode = (hashCode * 347) ^ West.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Bbox b1, Bbox b2)
        {
            return
                b1 != null && b2 != null &&
                Math.Abs(b1.North - b2.North) < Constants.EPSILON &&
                Math.Abs(b1.South - b2.South) < Constants.EPSILON &&
                Math.Abs(b1.East - b2.East) < Constants.EPSILON &&
                Math.Abs(b1.West - b2.West) < Constants.EPSILON;
        }
    

        public static bool operator !=(Bbox b1, Bbox b2)
        {
            return !(b1 == b2);
        }
    }
}
