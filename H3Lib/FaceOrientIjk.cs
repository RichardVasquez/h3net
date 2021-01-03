namespace H3Lib
{
    public readonly struct FaceOrientIjk
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

    }
}
