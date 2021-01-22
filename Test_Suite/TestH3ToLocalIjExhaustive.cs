using NUnit.Framework;
using H3Lib;

namespace TestSuite
{
    [TestFixture]
    public class TestH3ToLocalIjExhaustive
    {
        private readonly int[] MAX_DISTANCES = {1, 2, 5, 12, 19, 26};

        private readonly CoordIj[] DIRECTIONS =
        {
            new CoordIj(0, 1),
            new CoordIj(-1, 0),
            new CoordIj(-1, -1),
            new CoordIj(0, -1),
            new CoordIj(1, 0),
            new CoordIj(1, 1)
        };

        private readonly CoordIj NEXT_RING_DIRECTION = new CoordIj(1, 0);
        
        

    }
}
