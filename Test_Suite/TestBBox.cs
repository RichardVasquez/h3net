using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace TestSuite
{
    [TestFixture]
    public class TestBBox
    {
        private static void RepeatBoxTest(GeoFence geofence, BBox expected, GeoCoord inside, GeoCoord outside)
        {
            var result = geofence.ToBBox();

            Assert.AreEqual(result, expected);
            Assert.IsTrue(result.Contains(inside));
            Assert.IsFalse(result.Contains(outside));
        }

        [Test]
        public void PosLatPosLon()
        {
            var verts = new[]
                        {
                            new GeoCoord(0.8, 0.3), new GeoCoord(0.7, 0.6),
                            new GeoCoord(1.1, 0.7), new GeoCoord(1.0, 0.2)
                        };

            var geofence = new GeoFence {Verts = verts, NumVerts = 4};
            var expected = new BBox(1.1, 0.7, 0.7, 0.2);
            var inside = new GeoCoord(0.9, 0.4);
            var outside = new GeoCoord(0.0, 0.0);
            RepeatBoxTest(geofence, expected, inside, outside);
        }

        [Test]
        public void NegLatPosLon()
        {
            var verts = new[]
                        {
                            new GeoCoord(-0.3, 0.6), new GeoCoord(-0.4, 0.9),
                            new GeoCoord(-0.2, 0.8), new GeoCoord(-0.1, 0.6)
                        };

            var geofence = new GeoFence {NumVerts = 4, Verts = verts};
            var expected = new BBox(-0.1, -0.4, 0.9, 0.6);
            var inside = new GeoCoord(-0.3, 0.8);
            var outside = new GeoCoord(0.0, 0.0);
            RepeatBoxTest(geofence, expected, inside, outside);
        }

        [Test]
        public void PosLatNegLan()
        {
            var verts = new[]
                        {
                            new GeoCoord(0.7, -1.4), new GeoCoord(0.8, -0.9),
                            new GeoCoord(1.0, -0.8), new GeoCoord(1.1, -1.3)
                        };

            var geofence = new GeoFence {NumVerts = 4, Verts = verts};
            var expected = new BBox(1.1, 0.7, -0.8, -1.4);
            var inside = new GeoCoord(0.9, -1.0);
            var outside = new GeoCoord(0.0, 0.0);
            RepeatBoxTest(geofence, expected, inside, outside);
        }

        [Test]
        public void NegLatNegLon()
        {
            var verts = new[]
                        {
                            new GeoCoord(-0.4, -1.4), new GeoCoord(-0.3, -1.1),
                            new GeoCoord(-0.1, -1.2), new GeoCoord(-0.2, -1.4)
                        };

            var geofence = new GeoFence {NumVerts = 4, Verts = verts};
            var expected = new BBox(-0.1, -0.4, -1.1, -1.4);
            var inside = new GeoCoord(-0.3, -1.2);
            var outside = new GeoCoord(0.0, 0.0);
            RepeatBoxTest(geofence, expected, inside, outside);
        }

        [Test]
        public void AroundZeroZero()
        {
            var verts = new[]
                        {
                            new GeoCoord(0.4, -0.4), new GeoCoord(0.4, 0.4),
                            new GeoCoord(-0.4, 0.4), new GeoCoord(-0.4, -0.4)
                        };

            var geofence = new GeoFence {NumVerts = 4, Verts = verts};
            var expected = new BBox(0.4, -0.4, 0.4, -0.4);
            var inside = new GeoCoord(-0.1, -0.1);
            var outside = new GeoCoord(1.0, -1.0);
            RepeatBoxTest(geofence, expected, inside, outside);
        }

        [Test]
        public void Transmeridian()
        {
            var verts = new[]
                        {
                            new GeoCoord(0.4, Constants.M_PI - 0.1),
                            new GeoCoord(0.4, -Constants.M_PI + 0.1),
                            new GeoCoord(-0.4, -Constants.M_PI + 0.1),
                            new GeoCoord(-0.4, Constants.M_PI - 0.1)
                        };

            var geofence = new GeoFence {NumVerts = 4, Verts = verts};
            var expected = new BBox(0.4, -0.4, -Constants.M_PI + 0.1, Constants.M_PI - 0.1);
            var insideOnMeridian = new GeoCoord(-0.1, Constants.M_PI);
            var outside = new GeoCoord(1.0, Constants.M_PI - 0.5);
            RepeatBoxTest(geofence, expected, insideOnMeridian, outside);

            var westInside = new GeoCoord(0.1, Constants.M_PI - 0.05);
            Assert.IsTrue(expected.Contains(westInside));

            var eastInside = new GeoCoord(0.1, -Constants.M_PI + 0.05);
            Assert.IsTrue(expected.Contains(eastInside));

            var westOutside = new GeoCoord(0.1, Constants.M_PI - 0.5);
            Assert.IsFalse(expected.Contains(westOutside));

            var eastOutside = new GeoCoord(0.1, -Constants.M_PI + 0.5);
            Assert.IsFalse(expected.Contains(eastOutside));
        }

        [Test]
        public void EdgeOnNorthPole()
        {
            var verts = new[]
                        {
                            new GeoCoord(Constants.M_PI_2 - 0.1, 0.1),
                            new GeoCoord(Constants.M_PI_2 - 0.1, 0.8),
                            new GeoCoord(Constants.M_PI_2, 0.8),
                            new GeoCoord(Constants.M_PI_2, 0.1)
                        };
            var geofence = new GeoFence {NumVerts = 4, Verts = verts};
            var expected = new BBox(Constants.M_PI_2, Constants.M_PI_2 - 0.1, 0.8, 0.1);
            var inside = new GeoCoord(Constants.M_PI_2 - 0.01, 0.4);
            var outside = new GeoCoord(Constants.M_PI_2, 0.9);
            RepeatBoxTest(geofence, expected, inside, outside);
        }

        [Test]
        public void EdgeOnSouthPole()
        {
            var verts = new[]
                        {
                            new GeoCoord(-Constants.M_PI_2 + 0.1, 0.1),
                            new GeoCoord(-Constants.M_PI_2 + 0.1, 0.8),
                            new GeoCoord(-Constants.M_PI_2, 0.8),
                            new GeoCoord(-Constants.M_PI_2, 0.1)
                        };

            var geofence = new GeoFence {NumVerts = 4, Verts = verts};
            var expected = new BBox(-Constants.M_PI_2 + 0.1, -Constants.M_PI_2, 0.8, 0.1);
            var inside = new GeoCoord(-Constants.M_PI_2 + 0.01, 0.4);
            var outside = new GeoCoord(-Constants.M_PI_2, 0.9);
            RepeatBoxTest(geofence, expected, inside, outside);
        }
        
        [Test]
        public void ContainsEdges()
        {
            var bbox = new BBox(0.1, -0.1, 0.2, -0.2);
            var points = new[]
                         {
                             new GeoCoord(0.1, 0.2), new GeoCoord(0.1, 0.0),
                             new GeoCoord(0.1, -0.2), new GeoCoord(0.0, 0.2),
                             new GeoCoord(-0.1, 0.2), new GeoCoord(-0.1, 0.0),
                             new GeoCoord(-0.1, -0.2), new GeoCoord(0.0, -0.2)
                         };

            foreach (var point in points)
            {
                Assert.IsTrue(bbox.Contains(point));
            }
        }

        [Test]
        public void ContainsEdgesTransmeridian()
        {
            var bbox = new BBox(0.1, -0.1, -Constants.M_PI + 0.2, Constants.M_PI - 0.2);

            var points = new[]
                         {
                             new GeoCoord(0.1, -Constants.M_PI + 0.2),
                             new GeoCoord(0.1, Constants.M_PI),
                             new GeoCoord(0.1, Constants.M_PI - 0.2),
                             new GeoCoord(0.0, -Constants.M_PI + 0.2),
                             new GeoCoord(-0.1, -Constants.M_PI + 0.2),
                             new GeoCoord(-0.1, Constants.M_PI),
                             new GeoCoord(-0.1, Constants.M_PI - 0.2),
                             new GeoCoord(0.0, Constants.M_PI - 0.2)
                         };
            
            foreach (var point in points)
            {
                Assert.IsTrue(bbox.Contains(point));
            }
        }

        [Test]
        public void BboxCenterBasicQuadrants()
        {
            var bbox1 = new BBox(1.0, 0.8, 1.0, 0.8);
            var expected1 = new GeoCoord(0.9, 0.9);
            var center = bbox1.Center();
            Assert.AreEqual(center, expected1);

            var bbox2 = new BBox(-0.8, -1.0, 1.0, 0.8);
            var expected2 = new GeoCoord(-0.9, 0.9);
            center = bbox2.Center();
            Assert.AreEqual(center, expected2);

            var bbox3 = new BBox(1.0, 0.8, -0.8, -1.0);
            var expected3 = new GeoCoord(0.9, -0.9);
            center = bbox3.Center();
            Assert.AreEqual(center, expected3);

            var bbox4 = new BBox(-0.8, -1.0, -0.8, -1.0);
            var expected4 = new GeoCoord(-0.9, -0.9);
            center = bbox4.Center();
            Assert.AreEqual(center, expected4);

            var bbox5 = new BBox(0.8, -0.8, 1.0, -1.0);
            var expected5 = new GeoCoord(0.0, 0.0);
            center = bbox5.Center();
            Assert.AreEqual(center, expected5);
        }

        [Test]
        public void BboxCenterTransmeridian()
        {
            var bbox1 = new BBox(1.0, 0.8, -Constants.M_PI + 0.3, Constants.M_PI - 0.1);
            var expected1 = new GeoCoord(0.9, -Constants.M_PI + 0.1);
            var center = bbox1.Center();
            Assert.AreEqual(center, expected1);

            var bbox2 = new BBox(1.0, 0.8, -Constants.M_PI + 0.1, Constants.M_PI - 0.3);
            var expected2 = new GeoCoord(0.9, Constants.M_PI - 0.1);
            center = bbox2.Center();
            Assert.AreEqual(center, expected2);

            var bbox3 = new BBox(1.0, 0.8, -Constants.M_PI + 0.1, Constants.M_PI - 0.1);
            var expected3 = new GeoCoord(0.9, Constants.M_PI);
            center = bbox3.Center();
            Assert.AreEqual(center, expected3);
        }

        [Test]
        public void BboxIsTransmeridian()
        {
            var bboxNormal = new BBox(1.0, 0.8, 1.0, 0.8);
            Assert.IsFalse(bboxNormal.IsTransmeridian);

            var bboxTransmeridian = new BBox(1.0, 0.8, -Constants.M_PI + 0.3, Constants.M_PI - 0.1);
            Assert.IsTrue(bboxTransmeridian.IsTransmeridian);
        }

        [Test]
        public void BboxEquals()
        {
            var bbox = new BBox(1.0, 0.0, 1.0, 0.0);
            var north = bbox.ReplaceNorth(bbox.North + 0.1);
            var south = bbox.ReplaceSouth(bbox.South + 0.1);
            var east = bbox.ReplaceEast(bbox.East + 0.1);
            var west = bbox.ReplaceWest(bbox.West + 0.1);

            Assert.AreEqual(bbox,bbox);
            Assert.AreNotEqual(bbox,north);
            Assert.AreNotEqual(bbox,south);
            Assert.AreNotEqual(bbox,east);
            Assert.AreNotEqual(bbox,west);
        }
    }
}
