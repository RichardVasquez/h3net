using System;

namespace H3Lib
{
    public readonly struct FaceOrientIjk:IEquatable<FaceOrientIjk>
    {
        public readonly int Face;
        public readonly CoordIjk Translate;
        public readonly int Ccw60Rotations;

        public FaceOrientIjk(int f, int i, int j, int k, int c)
        {
            Face = f;
            Translate = new CoordIjk(i, j, k);
            Ccw60Rotations = c;
        }

        public FaceOrientIjk(int f, CoordIjk translate, int c)
        {
            Face = f;
            Translate = translate;
            Ccw60Rotations = c;
        }

        public bool Equals(FaceOrientIjk other)
        {
            return Face == other.Face &&
                   Translate.Equals(other.Translate) &&
                   Ccw60Rotations == other.Ccw60Rotations;
        }

        public override bool Equals(object obj)
        {
            return obj is FaceOrientIjk other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Face, Translate, Ccw60Rotations);
        }

        public static bool operator ==(FaceOrientIjk left, FaceOrientIjk right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(FaceOrientIjk left, FaceOrientIjk right)
        {
            return !left.Equals(right);
        }
    }
}

