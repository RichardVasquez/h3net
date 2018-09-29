namespace h3net.API
{
    public static class Constants
    {
        public const double M_PI = 3.14159265358979323846;
        public const double M_PI_2 = 1.5707963267948966;
        public const double M_2PI = 6.28318530717958647692528676655900576839433;
        public const double M_PI_180 = 0.0174532925199432957692369076848861271111;
        public const double M_180_PI = 57.29577951308232087679815481410517033240547;
        public const double EPSILON = 0.0000000000000001;
        public const double M_SQRT3_2 = 0.8660254037844386467637231707529361834714;
        public const double M_SIN60 = M_SQRT3_2;
        public const double M_AP7_ROT_RADS = 0.333473172251832115336090755351601070065900389;
        public const double M_SIN_AP7_ROT = 0.3273268353539885718950318;
        public const double M_COS_AP7_ROT = 0.9449111825230680680167902;
        public const double EARTH_RADIUS_KM = 6371.007180918475;
        public const double RES0_U_GNOMONIC = 0.38196601125010500003;
        public const int MAX_H3_RES = 15;
        public const int NUM_ICOSA_FACES = 20;
        public const int NUM_BASE_CELLS = 122;
        public const int NUM_HEX_VERTS = 6;
        public const int NUM_PENT_VERTS = 5;
        public const int H3_HEXAGON_MODE = 1;
        public const int H3_UNIEDGE_MODE = 2;
        public const double EPSILON_DEG = 0.000000001;
        public const double EPSILON_RAD = EPSILON_DEG * M_PI_180;
        public const int MAX_CELL_BNDRY_VERTS = 10;
        public const int HEX_RANGE_SUCCESS = 0;
        public const int HEX_RANGE_PENTAGON = 1;
        public const int HEX_RANGE_K_SUBSEQUENCE = 1;

        public const double DBL_EPSILON = 2.2204460492503131e-16;
        /**
         * Direction used for traversing to the next outward hexagonal ring.
        */
        public const Direction NEXT_RING_DIRECTION = Direction.I_AXES_DIGIT;
    }
}

