/*
 * Copyright 2018, Richard Vasquez
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *         http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * Original version written in C, Copyright 2016-2017 Uber Technologies, Inc.
 * C version licensed under the Apache License, Version 2.0 (the "License");
 * C Source code available at: https://github.com/uber/h3
 *
 */

using H3Net.Code;
using NUnit.Framework;

namespace h3tests
{
    [TestFixture]
    public class TestBBox
    {
        public void assertBBox(Geofence Geofence, BBox expected, GeoCoord inside, GeoCoord outside)
        {
            BBox result = new BBox();

            Polygon.bboxFromGeofence(ref Geofence, ref result);
            Assert.True(BBox.bboxEquals(result, expected));
            Assert.True(BBox.bboxContains(result, inside));
            Assert.False(BBox.bboxContains(result, outside));
        }

        [Test]
        public void posLatPosLon()
        {
            GeoCoord[] testVerts =
            {
                new GeoCoord(0.8, 0.3),
                new GeoCoord(0.7, 0.6),
                new GeoCoord(1.1, 0.7),
                new GeoCoord(1.0, 0.2)
            };

            Geofence Geofence = new Geofence {numVerts = 4, verts = testVerts};
            BBox expected = new BBox {north = 1.1, south = 0.7, east = 0.7, west = 0.2};
            GeoCoord inside = new GeoCoord(0.9, 0.4);
            GeoCoord outside = new GeoCoord(0.0, 0.0);
            assertBBox(Geofence, expected, inside, outside);
        }

        [Test]
        public void negLatPosLon()
        {
            GeoCoord[] testVerts =
            {
                new GeoCoord(-0.3, 0.6),
                new GeoCoord(-0.4, 0.9),
                new GeoCoord(-0.2, 0.8),
                new GeoCoord(-0.1, 0.6)
            };
            Geofence Geofence = new Geofence {numVerts = 4, verts = testVerts};
            BBox expected = new BBox {north = -0.1, south = -0.4, east = 0.9, west = 0.6};
            GeoCoord inside = new GeoCoord(-0.3, 0.8);
            GeoCoord outside = new GeoCoord(0.0, 0.0);
            assertBBox(Geofence, expected, inside, outside);
        }

        [Test]
        public void posLatNegLon()
        {
            GeoCoord[] testVerts =
                {new GeoCoord(0.7, -1.4), new GeoCoord(0.8, -0.9), new GeoCoord(1.0, -0.8), new GeoCoord(1.1, -1.3)};
            Geofence Geofence = new Geofence {numVerts = 4, verts = testVerts};
            BBox expected = new BBox {north = 1.1, south = 0.7, east = -0.8, west = -1.4};
            GeoCoord inside = new GeoCoord(0.9, -1.0);
            GeoCoord outside = new GeoCoord(0.0, 0.0);
            assertBBox(Geofence, expected, inside, outside);
        }

        [Test]
        public void negLatNegLon()
        {
            GeoCoord[] testVerts =
            {
                new GeoCoord(-0.4, -1.4),
                new GeoCoord(-0.3, -1.1),
                new GeoCoord(-0.1, -1.2),
                new GeoCoord(-0.2, -1.4)
            };
            Geofence Geofence = new Geofence {numVerts = 4, verts = testVerts};
            BBox expected = new BBox {north = -0.1, south = -0.4, east = -1.1, west = -1.4};
            GeoCoord inside = new GeoCoord(-0.3, -1.2);
            GeoCoord outside = new GeoCoord(0.0, 0.0);
            assertBBox(Geofence, expected, inside, outside);
        }

        [Test]
        public void aroundZeroZero()
        {
            GeoCoord[] testVerts =
            {
                new GeoCoord(0.4, -0.4),
                new GeoCoord(0.4, 0.4),
                new GeoCoord(-0.4, 0.4),
                new GeoCoord(-0.4, -0.4)
            };
            Geofence Geofence = new Geofence {numVerts = 4, verts = testVerts};
            BBox expected = new BBox {north = 0.4, south = -0.4, east = 0.4, west = -0.4};
            GeoCoord inside = new GeoCoord(-0.1, -0.1);
            GeoCoord outside = new GeoCoord(1.0, -1.0);
            assertBBox(Geofence, expected, inside, outside);
        }

        [Test]
        public void transmeridian()
        {
            GeoCoord[] testVerts =
            {
                new GeoCoord(0.4, Constants.M_PI - 0.1),
                new GeoCoord(0.4, -Constants.M_PI + 0.1),
                new GeoCoord(-0.4, -Constants.M_PI + 0.1),
                new GeoCoord(-0.4, Constants.M_PI - 0.1)
            };
            Geofence Geofence = new Geofence {numVerts = 4, verts = testVerts};
            BBox expected = new BBox
                            {
                                north = 0.4, south = -0.4, east = -Constants.M_PI + 0.1,
                                west = Constants.M_PI - 0.1
                            };
            GeoCoord insideOnMeridian = new GeoCoord(-0.1, Constants.M_PI);
            GeoCoord outside = new GeoCoord(1.0, Constants.M_PI - 0.5);
            assertBBox(Geofence, expected, insideOnMeridian, outside);

            GeoCoord westInside = new GeoCoord(0.1, Constants.M_PI - 0.05);
            Assert.True(BBox.bboxContains(expected, westInside));
            GeoCoord eastInside = new GeoCoord(0.1, -Constants.M_PI + 0.05);
            Assert.True(BBox.bboxContains(expected, eastInside));

            GeoCoord westOutside = new GeoCoord(0.1, Constants.M_PI - 0.5);
            Assert.False(BBox.bboxContains(expected, westOutside));
            GeoCoord eastOutside = new GeoCoord(0.1, -Constants.M_PI + 0.5);
            Assert.False(BBox.bboxContains(expected, eastOutside));
        }

        [Test]
        public void edgeOnNorthPole()
        {
            GeoCoord[] testVerts =
            {
                new GeoCoord(Constants.M_PI_2 - 0.1, 0.1),
                new GeoCoord(Constants.M_PI_2 - 0.1, 0.8),
                new GeoCoord(Constants.M_PI_2, 0.8),
                new GeoCoord(Constants.M_PI_2, 0.1)
            };
            Geofence Geofence = new Geofence {numVerts = 4, verts = testVerts};
            BBox expected = new BBox {north = Constants.M_PI_2, south = Constants.M_PI_2 - 0.1, east = 0.8, west = 0.1};
            GeoCoord inside = new GeoCoord(Constants.M_PI_2 - 0.01, 0.4);
            GeoCoord outside = new GeoCoord(Constants.M_PI_2, 0.9);
            assertBBox(Geofence, expected, inside, outside);
        }

        [Test]
        public void edgeOnSouthPole()
        {
            GeoCoord[] testVerts =
            {
                new GeoCoord(-Constants.M_PI_2 + 0.1, 0.1),
                new GeoCoord(-Constants.M_PI_2 + 0.1, 0.8),
                new GeoCoord(-Constants.M_PI_2, 0.8),
                new GeoCoord(-Constants.M_PI_2, 0.1)
            };
            Geofence Geofence = new Geofence {numVerts = 4, verts = testVerts};
            BBox expected = new BBox
                            {
                                north = -Constants.M_PI_2 + 0.1, south = -Constants.M_PI_2, east = 0.8,
                                west = 0.1
                            };
            GeoCoord inside = new GeoCoord(-Constants.M_PI_2 + 0.01, 0.4);
            GeoCoord outside = new GeoCoord(-Constants.M_PI_2, 0.9);
            assertBBox(Geofence, expected, inside, outside);
        }

        [Test]
        public void containsEdges()
        {
            BBox bbox = new BBox {north = 0.1, south = -0.1, east = 0.2, west = -0.2};
            GeoCoord[] points =
            {
                new GeoCoord(0.1, 0.2), new GeoCoord(0.1, 0.0),
                new GeoCoord(0.1, -0.2), new GeoCoord(0.0, 0.2),
                new GeoCoord(-0.1, 0.2), new GeoCoord(-0.1, 0.0),
                new GeoCoord(-0.1, -0.2), new GeoCoord(0.0, -0.2),
            };
            int numPoints = 8;

            for (int i = 0; i < numPoints; i++)
            {
                Assert.True(BBox.bboxContains(bbox, points[i]), $"Failed on point {i}");
            }
        }

        [Test]
        public void containsEdgesTransmeridian()
        {
            BBox bbox = new BBox {north = 0.1, south = -0.1, east = -Constants.M_PI + 0.2, west = Constants.M_PI - 0.2};
            GeoCoord[] points =
            {
                new GeoCoord(0.1, -Constants.M_PI + 0.2),
                new GeoCoord(0.1, Constants.M_PI),
                new GeoCoord(0.1, Constants.M_PI - 0.2),
                new GeoCoord(0.0, -Constants.M_PI + 0.2),
                new GeoCoord(-0.1, -Constants.M_PI + 0.2),
                new GeoCoord(-0.1, Constants.M_PI),
                new GeoCoord(-0.1, Constants.M_PI - 0.2),
                new GeoCoord(0.0, Constants.M_PI - 0.2),
            };
            int numPoints = 8;

            for (int i = 0; i < numPoints; i++)
            {
                Assert.True(BBox.bboxContains(bbox, points[i]), $"Failed on point {i}");
            }
        }

        [Test]
        public void bboxCenterBasicQuandrants()
        {
            GeoCoord center = new GeoCoord();

            BBox bbox1 = new BBox {north = 1.0, south = 0.8, east = 1.0, west = 0.8};
            GeoCoord expected1 = new GeoCoord(0.9, 0.9);
            BBox.bboxCenter(bbox1, ref center);
            Assert.True(GeoCoord.geoAlmostEqual(center, expected1));

            BBox bbox2 = new BBox {north = -0.8, south = -1.0, east = 1.0, west = 0.8};
            GeoCoord expected2 = new GeoCoord(-0.9, 0.9);
            BBox.bboxCenter(bbox2, ref center);
            Assert.True(GeoCoord.geoAlmostEqual(center, expected2));

            BBox bbox3 = new BBox {north = 1.0, south = 0.8, east = -0.8, west = -1.0};
            GeoCoord expected3 = new GeoCoord(0.9, -0.9);
            BBox.bboxCenter(bbox3, ref center);
            Assert.True(GeoCoord.geoAlmostEqual(center, expected3));

            BBox bbox4 = new BBox {north = -0.8, south = -1.0, east = -0.8, west = -1.0};
            GeoCoord expected4 = new GeoCoord(-0.9, -0.9);
            BBox.bboxCenter(bbox4, ref center);
            Assert.True(GeoCoord.geoAlmostEqual(center, expected4));

            BBox bbox5 = new BBox {north = 0.8, south = -0.8, east = 1.0, west = -1.0};
            GeoCoord expected5 = new GeoCoord(0.0, 0.0);
            BBox.bboxCenter(bbox5, ref center);
            Assert.True(GeoCoord.geoAlmostEqual(center, expected5));
        }

        [Test]
        public void bboxCenterTransmeridian()
        {
            GeoCoord center = new GeoCoord();

            BBox bbox1 = new BBox {north = 1.0, south = 0.8, east = -Constants.M_PI + 0.3, west = Constants.M_PI - 0.1};
            GeoCoord expected1 = new GeoCoord(0.9, -Constants.M_PI + 0.1);
            BBox.bboxCenter(bbox1, ref center);

            Assert.True(GeoCoord.geoAlmostEqual(center, expected1));

            BBox bbox2 = new BBox {north = 1.0, south = 0.8, east = -Constants.M_PI + 0.1, west = Constants.M_PI - 0.3};
            GeoCoord expected2 = new GeoCoord(0.9, Constants.M_PI - 0.1);
            BBox.bboxCenter(bbox2, ref center);
            Assert.True(GeoCoord.geoAlmostEqual(center, expected2));

            BBox bbox3 = new BBox {north = 1.0, south = 0.8, east = -Constants.M_PI + 0.1, west = Constants.M_PI - 0.1};
            GeoCoord expected3 = new GeoCoord(0.9, Constants.M_PI);
            BBox. bboxCenter(bbox3, ref center);
            Assert.True(GeoCoord.geoAlmostEqual(center, expected3));
        }

        [Test]
        public void bboxIsTransmeridian()
        {
            BBox bboxNormal = new BBox {north = 1.0, south = 0.8, east = 1.0, west = 0.8};
            Assert.False(BBox.bboxIsTransmeridian(bboxNormal));

            BBox bboxTransmeridian = new BBox
                                     {
                                         north = 1.0, south = 0.8, east = -Constants.M_PI + 0.3,
                                         west = Constants.M_PI - 0.1
                                     };
            Assert.True(BBox.bboxIsTransmeridian(bboxTransmeridian));
        }
    }
}
