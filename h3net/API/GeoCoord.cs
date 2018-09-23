using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace h3net.API
{
    public static class Constants
    {
        public const int MAX_CELL_BNDRY_VERTS = 10;
    }

    public struct GeoCoord
    {
        public double lat;
        public double lon;
    }

    public unsafe struct GeoBoundary
    {
        public int numVerts;
        GeoCoord* verts;
    }

    public struct GeoFence
    {
        public int numVerts;
        public unsafe GeoCoord* verts;

    }

    public struct GeoPolygon
    {
        private GeoFence geofence;
        private int numHoles;
        private unsafe GeoFence* holes;
    }

    public struct GeoMultiPolygon
    {
        private int numPolygons;
        private unsafe GeoPolygon* polygons;
    }

    public struct LinkedGeoCoord
    {
        private GeoCoord vertex;
        private unsafe LinkedGeoCoord* next;
    }

    public struct LinkedGeoLoop
    {
        private unsafe LinkedGeoCoord* first;
        private unsafe LinkedGeoCoord* last;
        private unsafe LinkedGeoLoop* next;
    }

    public struct LinkedGeoPolygon
    {
        private unsafe LinkedGeoLoop* first;
        private unsafe LinkedGeoLoop* last;
        private unsafe LinkedGeoPolygon* next;
    }

    public unsafe class Api
    {
        public ulong geoToH3(GeoCoord* g, int res)
        {
            return 0;
        }

        public void h3ToGeo(ulong h3, GeoCoord* g)
        {
        }

        public unsafe void h3ToGeoBoundary(ulong h3,  GeoBoundary* gp)
        {
        }

        public int maxKringSize(int k)
        {
            return 0;
        }

        public int hexRange(ulong origin, int k, ulong* outhex)
        {
            return 0;
        }

        public int hexRangeDistances(ulong origin, int k, ulong* outindex, int* distances)
        {
            return 0;
        }

        public int hexRanges(ulong* h3set, int length, int k, ulong* outhex)
        {
            return 0;
        }

        public void kRing(ulong* origin, int k, ulong* outhex)
        {

        }

        public void kRingDistances(ulong origin, int k, ulong* outhex, int* distances)
        {

        }

        public int hexRing(ulong origin, int k, ulong* outhex)
        {
            return 0;
        }

        public int maxPolyfillSize(GeoPolygon* geoPolygon, int res)
        {
            return 0;
        }

        public void polyfill(GeoPolygon* geoPolygon, int res, ulong* outhex)
        {
        }

        public void h3SetToLinkedGeo(ulong* h3Set, int numHexes, LinkedGeoPolygon* outhex)
        {
        }

        public void destroyLinkedPolygon(LinkedGeoPolygon* polygon)
        {
        }

        public double degsToRads(double degrees)
        {
            return 0.0;
        }

        public double radsToDegs(double radians)
        {
            return 0.0;}

        public double hexAreaKm2(int res)
        {
            return 0.0;}

        public double hexAreaM2(int res)
        {
            return 0.0;
        }

        public double edgeLengthKm(int res)
        {
            return 0.0;
        }

        public double edgeLengthM(int res)
        {
            return 0.0;}

        public long numHexagons(int res)
        {
            return 0;
        }
        public int h3GetResolution(ulong h)
        {
            return 0;
        }

        public int h3GetBaseCell(ulong h)
        {
            return 0;
        }

        public ulong stringToH3(char* instr)
        {
            return 0;
        }

        public void h3ToString(ulong h, char* str, UIntPtr sz)
        {
        }

        public int h3IsValid(ulong h)
        {
            return 0;

        }

        public ulong h3ToParent(ulong h, int parentRes)
        {
            return 0;

        }

        public int maxH3ToChildrenSize(ulong h, int childRes)
        {
            return 0;

        }

        public void h3ToChildren(ulong h, int childRes, ulong* children)
        {
        }

        public int compact(ulong *h3Set, ulong *compactedSet, int numHexes)
        {
            return 0;
        }

        public int maxUncompactSize(ulong* compactedSet, int numHexes,int res)
        {
            return 0;
        }

        public int uncompact(ulong* compactedSet, int numHexes, ulong* h3Set, int maxHexes, int res)
        {
            return 0;
        }

        public int h3IsResClassIII(ulong h)
        {
            return 0;
        }

        public int h3IsPentagon(ulong h)
        {
            return 0;
        }

        public int h3IndexesAreNeighbors(ulong origin, ulong destination)
        {
            return 0;
        }

        public ulong getH3UnidirectionalEdge(ulong origin, ulong destination)
        {
            return 0;
        }

        public int h3UnidirectionalEdgeIsValid(ulong edge)
        {
            return 0;
        }

        public ulong getOriginH3IndexFromUnidirectionalEdge(ulong edge)
        {
            return 0;
        }

        public ulong getDestinationH3IndexFromUnidirectionalEdge(ulong edge){return 0;}

        public void getH3IndexesFromUnidirectionalEdge(ulong edge, ulong* originDestination)
        {
        }

        public void getH3UnidirectionalEdgesFromHexagon(ulong origin, ulong* edges)
        {
        }

        public void getH3UnidirectionalEdgeBoundary(ulong edge, GeoBoundary* gb)
        {
        }

        public int h3Distance(ulong origin, ulong h3)
        {
            return 0;
        }
    }


    public static class H3Constants
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
    }

    public class H3GeoCoord
    {
        public const double EPSILON_DEG = 0.000000001;
        public const double EPSILON_RAD = EPSILON_DEG * H3Constants.M_PI_180;

        public unsafe void setGeoDegs(GeoCoord* p, double latDegs, double LonDegs)
        {

        }

        public double constrainLat(double lat)
        {
            return 0.0;
        }

        public double ConstrainLng(double lng)
        {
            return 0.0;
        }

        public unsafe bool geoAlmostEqual(GeoCoord* p1,  GeoCoord* p2)
        {
            return false;
        }

        public unsafe bool geoAlmostEqualThreshold(GeoCoord* p1,  GeoCoord* p2, double threshold)
        {
            return false;
        }

        public double _postAngleRads(double rads)
        {
            return 0.0;
        }

        public unsafe double _setGeoRads(GeoCoord* p, double latRads, double lonRads)
        {
            return 0.0;
        }

        public unsafe double _geoDistRads(GeoCoord* p1, GeoCoord* p2)
        {
            return 0.0;
        }

        public unsafe double _geoDistKm(GeoCoord* p1, GeoCoord* p2)
        {
            return 0.0;
        }
        public unsafe double _geoAzimuthRads(GeoCoord* p1, GeoCoord* p2)
        {
            return 0.0;
        }

        public unsafe void _geoAzDistanceRads(GeoCoord* p1, double az, double distance, GeoCoord* p2)
        {

        }
    }

    public struct BBox
    {
        public double north;
        public double south;
        public double east;
        public double west;
    }

    public class H3Bbox
    {
        public unsafe bool bboxIsTransmeridian(BBox* bbox)
        {
            return false;
        }

        public unsafe void bboxCenter(BBox* bbox, GeoCoord* center)
        {
        }

        public unsafe bool bboxContains(BBox* bbox, GeoCoord* point)
        {
            return false;
        }

        public unsafe bool bboxEquals(BBox* b1, BBox* b2)
        {
            return false;
        }

        public unsafe int bboxHexRadius(BBox* bbox, int res)
        {
            return 0;
        }
    }

    public struct VertexNode
    {
        public GeoCoord from;
        public GeoCoord to;
        public unsafe VertexNode* next;
    }

    public struct VertexGraph
    {
        public unsafe VertexNode** buckets;
        public int numBuckets;
        public int size;
        public int res;
    }


    public class H3VertexGraph
    {
        public unsafe void initVertexGraph(VertexGraph* graph, int numBuckets, int res)
        {
        }

        public unsafe void destroyVertexGraph(VertexGraph* graph)
        {
        }

        public unsafe VertexNode* addVertexNode(VertexGraph* graph, GeoCoord* fromVtx, GeoCoord* toVtx)
        {
            return null;
        }

        public unsafe int removeVertexNode(VertexGraph* graph, VertexNode* node)
        {
            return 0;
        }

        public unsafe VertexNode* findNodeForEdge(VertexGraph* graph, GeoCoord* fromVtx, GeoCoord* toVtx)
        {
            return null;
        }

        public unsafe VertexNode* findNodeForVertex(VertexGraph* graph, GeoCoord* fromVtx)
        {
            return null;
        }

        public unsafe VertexNode* firstVertexNode(VertexGraph* graph)
        {
            return null;
        }

// Internal functions
        public unsafe uint _hashVertex(GeoCoord* vertex, int res, int numBuckets)
        {
            return 0;
        }

        public unsafe void _initVertexNode(VertexNode* node, GeoCoord* fromVtx, GeoCoord* toVtx)
        {
        }
    }

    public struct Vec2d
    {
        public double x;
        public double y;
    }

    public class H3Vec2D
    {
        public unsafe double _v2dMag(Vec2d* v)
        {
            return 0.0;
        }

        public unsafe void _v2dIntersect(Vec2d* p0, Vec2d* p1, Vec2d* p2, Vec2d* p3, Vec2d* inter)
        {
        }

        public unsafe bool _v2dEquals(Vec2d* p0, Vec2d* p1)
        {
            return false;
        }
    }

    public enum Direction
    {
        CENTER_DIGIT = 0,
        K_AXES_DIGIT = 1,
        J_AXES_DIGIT = 2,
        JK_AXES_DIGIT = J_AXES_DIGIT|K_AXES_DIGIT,
        I_AXES_DIGIT = 4,
        IK_AXES_DIGIT = I_AXES_DIGIT|K_AXES_DIGIT,
        IJ_AXES_DIGIT = I_AXES_DIGIT|J_AXES_DIGIT,
        INVALID_DIGIT = 7,
        NUM_DIGITS = INVALID_DIGIT
    }

    public struct CoordIJK
    {
        public int i;
        public int j;
        public int k;

        public CoordIJK(int a, int b, int c)
        {
            i = a;
            j = b;
            k = c;
        }
    }
    public class H3CoordIJK
    {
        public readonly CoordIJK[] UNIT_VECS = {
            new CoordIJK(0, 0, 0),
            new CoordIJK(0, 0, 1),
            new CoordIJK(0, 1, 0),
            new CoordIJK(0, 1, 1),
            new CoordIJK(1, 0, 0),
            new CoordIJK(1, 0, 1),
            new CoordIJK(1, 1, 0)
        };

        public unsafe void _setIJK(CoordIJK* ijk, int i, int j, int k)
        {
        }

        public unsafe void _hex2dToCoordIJK(Vec2d* v, CoordIJK* h)
        {
        }

        public unsafe void _ijkToHex2d(CoordIJK* h, Vec2d* v)
        {
        }

        public unsafe int _ijkMatches(CoordIJK* c1, CoordIJK* c2)
        {
            return 0;
        }

        public unsafe void _ijkAdd(CoordIJK* h1, CoordIJK* h2, CoordIJK* sum)
        {
        }

        public unsafe void _ijkSub(CoordIJK* h1, CoordIJK* h2, CoordIJK* diff)
        {
        }

        public unsafe void _ijkScale(CoordIJK* c, int factor)
        {
        }

        public unsafe void _ijkNormalize(CoordIJK* c)
        {
        }

        public unsafe Direction _unitIjkToDigit(CoordIJK* ijk)
        {
            return Direction.INVALID_DIGIT;
        }

        public unsafe void _upAp7(CoordIJK* ijk)
        {
        }

        public unsafe void _upAp7r(CoordIJK* ijk)
        {
        }

        public unsafe void _downAp7(CoordIJK* ijk)
        {
        }

        public unsafe void _downAp7r(CoordIJK* ijk)
        {
        }

        public unsafe void _downAp3(CoordIJK* ijk)
        {
        }

        public unsafe void _downAp3r(CoordIJK* ijk)
        {
        }

        public unsafe void _neighbor(CoordIJK* ijk, Direction digit)
        {
        }

        public unsafe void _ijkRotate60ccw(CoordIJK* ijk)
        {
        }

        public unsafe void _ijkRotate60cw(CoordIJK* ijk)
        {
        }

        public Direction _rotate60ccw(Direction digit)
        {
            return Direction.INVALID_DIGIT;
        }

        public Direction _rotate60cw(Direction digit)
        {
            return Direction.INVALID_DIGIT;
        }

        public unsafe int ijkDistance(CoordIJK* a, CoordIJK* b)
        {
            return 0;
        }

    }

    public struct FaceIJK
    {
        public int face;
        public CoordIJK coord;
    }

    public struct FaceOrientIJK
    {
        private int face;
        public CoordIJK translate;
        public int ccwRot60;
    }

    public class H3Faceijk
    {
        public GeoCoord[] faceCenterGeo = new GeoCoord[H3Constants.NUM_ICOSA_FACES];
        public const int INVALID = -1;
        public const int CENTER = 0;
        public const int IJ = 1;
        public const int KI = 2;
        public const int JK = 3;

        public unsafe void _geoToFaceIjk(GeoCoord* g, int res, FaceIJK* h)
        {
        }

        public unsafe void _geoToHex2d(GeoCoord* g, int res, int* face, Vec2d* v)
        {
        }
        public unsafe void _faceIjkToGeo(FaceIJK* h, int res, GeoCoord* g)
        {
        }

        public unsafe void _faceIjkToGeoBoundary(FaceIJK* h, int res, int isPentagon,
            GeoBoundary* g)
        {
        }

        public unsafe void _faceIjkPentToGeoBoundary(FaceIJK* h, int res, GeoBoundary* g)
        {
        }

        public unsafe void _hex2dToGeo(Vec2d* v, int face, int res, int substrate, GeoCoord* g)
        {
        }

        public unsafe int _adjustOverageClassII(FaceIJK* fijk, int res, int pentLeading4,
            int substrate)
        {
            return 0;
        }
    }

    public class h3LinkedGeo
    {
        public const int NORMALIZATION_SUCCESS = 0;
        public const int NORMALIZATION_ERR_MULTIPLE_POLYGONS = 1;
        public const int NORMALIZATION_ERR_UNASSIGNED_HOLES = 2;

// Macros for use with polygonAlgos.h
///** Macro: Init iteration vars for LinkedGeoLoop */
//#define INIT_ITERATION_LINKED_LOOP       \
//        LinkedGeoCoord* currentCoord = NULL; \
//        LinkedGeoCoord* nextCoord = NULL
//
///** Macro: Get the next coord in a linked loop, wrapping if needed */
//#define GET_NEXT_COORD(loop, coordToCheck) \
//        coordToCheck == NULL ? loop->first : currentCoord->next
//
///** Macro: Increment LinkedGeoLoop iteration, or break if done. */
//#define ITERATE_LINKED_LOOP(loop, vertexA, vertexB)       \
//            currentCoord = GET_NEXT_COORD(loop, currentCoord);    \
//            if (currentCoord == NULL) break;                      \
//        vertexA = currentCoord->vertex;                       \
//        nextCoord = GET_NEXT_COORD(loop, currentCoord->next); \
//        vertexB = nextCoord->vertex
//
        ///** Macro: Whether a LinkedGeoLoop is empty */
//#define IS_EMPTY_LINKED_LOOP(loop) loop->first == NULL
//         */

        public unsafe int normalizeMultiPolygon(LinkedGeoPolygon* root)
        {
            return 0;
        }
        public unsafe LinkedGeoPolygon* addNewLinkedPolygon(LinkedGeoPolygon* polygon)
        {
            return null;
        }

        public unsafe LinkedGeoLoop* addNewLinkedLoop(LinkedGeoPolygon* polygon)
        {
            return null;
        }

        public unsafe LinkedGeoLoop* addLinkedLoop(LinkedGeoPolygon* polygon, LinkedGeoLoop* loop)
        {
            return null;
        }

        public unsafe LinkedGeoCoord* addLinkedCoord(LinkedGeoLoop* loop, GeoCoord* vertex)
        {
            return null;
        }

        public unsafe int countLinkedPolygons(LinkedGeoPolygon* polygon)
        {
            return 0;
        }

        public unsafe int countLinkedLoops(LinkedGeoPolygon* polygon)
        {
            return 0;
        }

        public unsafe int countLinkedCoords(LinkedGeoLoop* loop)
        {
            return 0;
        }

        public unsafe void destroyLinkedGeoLoop(LinkedGeoLoop* loop)
        {
        }

// The following functions are created via macro in polygonAlgos.h,
// so their signatures are documented here:

/**
 * Create a bounding box from a LinkedGeoLoop
 * @param geofence Input Geofence
 * @param bbox     Output bbox
 */
//        void bboxFromLinkedGeoLoop(const LinkedGeoLoop* loop, BBox* bbox);

/**
 * Take a given LinkedGeoLoop data structure and check if it
 * contains a given geo coordinate.
 * @param loop          The linked loop
 * @param bbox          The bbox for the loop
 * @param coord         The coordinate to check
 * @return              Whether the point is contained
 */
//        bool pointInsideLinkedGeoLoop(const LinkedGeoLoop* loop, const BBox* bbox,
//        const GeoCoord* coord);

/**
 * Whether the winding order of a given LinkedGeoLoop is clockwise
 * @param loop  The loop to check
 * @return      Whether the loop is clockwise
 */
 //       bool isClockwiseLinkedGeoLoop(const LinkedGeoLoop* loop);


    }

    public class H3Polygon
    {
        public const int loopIndex = -1;

        public unsafe void bboxesFromGeoPolygon(GeoPolygon* polygon, BBox* bboxes)
        {
        }

        public unsafe bool pointInsidePolygon(GeoPolygon* geoPolygon, BBox* bboxes,
            GeoCoord* coord)
        {
            return false;
        }

//	        // Macros for use with polygonAlgos.h
//	/** Macro: Init iteration vars for Geofence */
//	#define INIT_ITERATION_GEOFENCE int loopIndex = -1
//	
//	/** Macro: Increment Geofence loop iteration, or break if done. */
//	#define ITERATE_GEOFENCE(geofence, vertexA, vertexB) \
//	        if (++loopIndex >= geofence->numVerts) break;    \
//	        vertexA = geofence->verts[loopIndex];            \
//	        vertexB = geofence->verts[(loopIndex + 1) % geofence->numVerts]
//	
//	// The following functions are created via macro in polygonAlgos.h,
//	// so their signatures are documented here:
//	
//	/**
//	 * Create a bounding box from a Geofence
//	 * @param geofence Input Geofence
//	 * @param bbox     Output bbox
//	 */
//	        void bboxFromGeofence(const Geofence* loop, BBox* bbox);
//	
//	/**
//	 * Take a given Geofence data structure and check if it
//	 * contains a given geo coordinate.
//	 * @param loop          The geofence
//	 * @param bbox          The bbox for the loop
//	 * @param coord         The coordinate to check
//	 * @return              Whether the point is contained
//	 */
//	        bool pointInsideGeofence(const Geofence* loop, const BBox* bbox,
//	        const GeoCoord* coord);
//	
//	/**
//	 * Whether the winding order of a given Geofence is clockwise
//	 * @param loop  The loop to check
//	 * @return      Whether the loop is clockwise
//	 */
//	        bool isClockwiseGeofence(const Geofence* geofence);
    }

    public class H3Algos
    {
        // neighbor along the ijk coordinate system of the current face, rotated
        public unsafe ulong h3NeighborRotations(ulong origin, Direction dir, int* rotations)
        {
            return 0;
        }

// k-ring implementation
        public unsafe void _kRingInternal(ulong origin, int k, ulong* out_ring, int* distances,
            int maxIdx, int curK)
        {
        }

// Create a vertex graph from a set of hexagons
        public unsafe void h3SetToVertexGraph( ulong* h3Set,  int numHexes, VertexGraph* out_graph) {
        }

// Create a LinkedGeoPolygon from a vertex graph
        public unsafe void _vertexGraphToLinkedGeo(VertexGraph* graph, LinkedGeoPolygon* out_polygon)
        {
        }
    }

    public struct BaseCellData
    {
        public FaceIJK homeFijk;
        public int isPentagon;
        public int[] cwOffsetPent;// [2]
    }

    public class H3baseCells
    {
        public const int INVALID_BASE_CELL = 127;
        public const int MAX_FACE_COORD = 2;
        public static int[,] baseCellNeighbors = new int[H3Constants.NUM_BASE_CELLS, 7];
        public static int[,] baseCellNeighbor60CCWRots = new int[H3Constants.NUM_BASE_CELLS, 7];
        public static BaseCellData[] baseCellData = new BaseCellData[H3Constants.NUM_BASE_CELLS];


        public int _isBaseCellPentagon(int baseCell)
        {
            return 0;}

        public unsafe int _faceIjkToBaseCell(FaceIJK* h)
        {
            return 0;
        }

        public unsafe int _faceIjkToBaseCellCCWrot60( FaceIJK* h)
        {
            return 0;
        }

        public unsafe void _baseCellToFaceIjk(int baseCell, FaceIJK* h)
        {
        }

        public bool _baseCellIsCwOffset(int baseCell, int testFace)
        {
            return false;
        }

        public int _getBaseCellNeighbor(int baseCell, Direction dir)
        {
            return 0;
        }

        public Direction _getBaseCellDirection(int originBaseCell, int destinationBaseCell)
        {
            return new Direction();
        }

    }

    public class h3Index
    {
        public static int H3_NUM_BITS = 64;
        public static int H3_MAX_OFFSET = 63;
        public static int H3_MODE_OFFSET = 59;
        public static int H3_BC_OFFSET = 45;
        public static int H3_RES_OFFSET = 52;
        public static int H3_RESERVED_OFFSET = 56;
        public static int H3_PER_DIGIT_OFFSET = 3;
        public static ulong H3_MODE_MASK = (ulong)15 << H3_MODE_OFFSET;
        public static ulong H3_MODE_MASK_NEGATIVE = ~H3_MODE_MASK;
        public static ulong H3_BC_MASK = (ulong) 127 << H3_BC_OFFSET;
        public static ulong H3_BC_MASK_NEGATIVE = ~H3_BC_MASK;
        public static ulong H3_RES_MASK = (ulong) 15 << H3_RES_OFFSET;
        public static ulong H3_RES_MASK_NEGATIVE = ~H3_RES_MASK;
        public static ulong H3_RESERVED_MASK = (ulong) 7 << H3_RESERVED_OFFSET;
        public static ulong H3_RESERVED_MASK_NEGATIVE = ~H3_RESERVED_MASK;
        public static ulong H3_DIGIT_MASK = 7;
        public static ulong H3_DIGIT_MASK_NEGATIVE = ~H3_DIGIT_MASK;
        public static ulong H3_INIT = 35184372088831;
        public static ulong H3_INVALID_INDEX = 0;

        // Should set value of h3
        public static void H3_SET_MODE(ref ulong h3, ulong v)
        {
            h3 = h3 & H3_MODE_MASK_NEGATIVE | (v << H3_MODE_OFFSET);
        }

        public static int H3_GET_BASE_CELL(ulong h3)
        {
            return (int)((h3 & H3_BC_MASK) >> H3_BC_OFFSET);
        }

        public static void H3_SET_BASE_CELL(ref ulong h3, ulong bc)
        {
            h3 = (h3 & H3_BC_MASK_NEGATIVE) | (bc << H3_BC_OFFSET);
        }

        public static int H3_GET_RESOLUTION(ulong h3)
        {
            return (int) ((h3 & H3_RES_MASK) >> H3_RES_OFFSET);
        }

        public static void H3_SET_RESOLUTION(ref ulong h3, ulong res)
        {
            h3 = (h3 & H3_RES_MASK_NEGATIVE) | (res << H3_RES_OFFSET);
        }

        public static Direction H3_GET_INDEX_DIGIT(ulong h3, int res)
        {
            return (Direction) ((h3 >> ((H3Constants.MAX_H3_RES - res) * H3_PER_DIGIT_OFFSET)) & H3_DIGIT_MASK);
        }

        public static void H3_SET_RESERVED_BITS(ref ulong h3, ulong v)
        {
            h3 = (h3 & H3_RESERVED_MASK_NEGATIVE) | (v << H3_RESERVED_OFFSET);
        }

        public static int H3_GET_RESERVED_BITS(ulong h3)
        {
            return (int) ((h3 & H3_RESERVED_MASK) >> H3_RESERVED_OFFSET);
        }

        public static void H3_SET_INDEX_DIGIT(ref ulong h3, int res, ulong digit)
        {
            h3 = (h3 & ~(H3_DIGIT_MASK << ((H3Constants.MAX_H3_RES - res) * H3_PER_DIGIT_OFFSET)))
                 | (digit << ((H3Constants.MAX_H3_RES - res) * H3_PER_DIGIT_OFFSET));
        }

        public unsafe void setH3Index(ulong* h, int res, int baseCell, Direction initDigit)
        {
        }

        public int isResClassIII(int res)
        {
            return 0;
        }

// Internal functions

        public unsafe ulong _faceIjkToH3(FaceIJK* fijk, int res)
        {
            return 0;
        }

        public Direction _h3LeadingNonZeroDigit(ulong h)
        {
            return Direction.INVALID_DIGIT;
        }

        public ulong _h3RotatePent60ccw(ulong h)
        {
            return 0;
        }

        public ulong _h3Rotate60ccw(ulong h)
        {
            return 0;
        }

        public ulong _h3Rotate60cw(ulong h)
        {
            return 0;
        }

        public unsafe int h3ToIjk(ulong origin, ulong h3, CoordIJK* out_coord)
        {
            return 0;
        }

    }

    public class H3MathExtensions
    {
        public int _ipow(int nbase, int exp)
        {
            return 0;
        }
    }
}


/*
polygonAlgos.h *** Fucking special case regarding C macros
stackAlloc.h
vec3d.h
 */
