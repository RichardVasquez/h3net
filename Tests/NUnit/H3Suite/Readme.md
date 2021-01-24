# NUnit Tests

The base test suites converted to NUnit functionality.

At this writing, there are 222 tests, and 10 of them fail.  My hunch, based on mapping
many points on the globe and mappping out hexagons and pentagons on h3explorer lead me
to considering that somewhere, in _some_ cases, shapes are slightly smaller than they're
supposed to be, which is causing discrepancies that lead to the test failures.

The _logic_ seems to work, i.e., Hex A has children A0-A6, and Pentagon P has children
P0-P5, but the actual geographic locations of the vertices are sometimes "off".

Failing Tests:

  1.  TestH3Api
      * h3ToGeoBoundary_classIIIEdgeVertex
      * H3ToGeoBoundary_coslonConstrain
  2.  TestH3CellAreaExhaustive
      * cell_area_earth
      * cell_area_positive
  3.  TestH3LineExhaustive
      * H3Line_kRing
  4.  TestH3SetToLinkedGeo
      * Hole
      * NegativeHashedCoordinates
      * NestedDonut
      * NestedDonutTransmeridian
  5.  TestH3UniEdgeExhaustive
      * h3UniEdgeBoundary
  6.  TestPolyfillReported
      * h3_136
