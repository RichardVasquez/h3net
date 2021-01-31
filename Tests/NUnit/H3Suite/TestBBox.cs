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
                            new GeoCoord(0.8m, 0.3m), new GeoCoord(0.7m, 0.6m),
                            new GeoCoord(1.1m, 0.7m), new GeoCoord(1.0m, 0.2m)
                        };

            var geofence = new GeoFence {Verts = verts, NumVerts = 4};
            var expected = new BBox(1.1m, 0.7m, 0.7m, 0.2m);
            var inside = new GeoCoord(0.9m, 0.4m);
            var outside = new GeoCoord(0.0m, 0.0m);
            RepeatBoxTest(geofence, expected, inside, outside);
        }

        [Test]
        public void NegLatPosLon()
        {
            var verts = new[]
                        {
                            new GeoCoord(-0.3m, 0.6m), new GeoCoord(-0.4m, 0.9m),
                            new GeoCoord(-0.2m, 0.8m), new GeoCoord(-0.1m, 0.6m)
                        };

            var geofence = new GeoFence {NumVerts = 4, Verts = verts};
            var expected = new BBox(-0.1m, -0.4m, 0.9m, 0.6m);
            var inside = new GeoCoord(-0.3m, 0.8m);
            var outside = new GeoCoord(0.0m, 0.0m);
            RepeatBoxTest(geofence, expected, inside, outside);
        }

        [Test]
        public void PosLatNegLan()
        {
            var verts = new[]
                        {
                            new GeoCoord(0.7m, -1.4m), new GeoCoord(0.8m, -0.9m),
                            new GeoCoord(1.0m, -0.8m), new GeoCoord(1.1m, -1.3m)
                        };

            var geofence = new GeoFence {NumVerts = 4, Verts = verts};
            var expected = new BBox(1.1m, 0.7m, -0.8m, -1.4m);
            var inside = new GeoCoord(0.9m, -1.0m);
            var outside = new GeoCoord(0.0m, 0.0m);
            RepeatBoxTest(geofence, expected, inside, outside);
        }

        [Test]
        public void NegLatNegLon()
        {
            var verts = new[]
                        {
                            new GeoCoord(-0.4m, -1.4m), new GeoCoord(-0.3m, -1.1m),
                            new GeoCoord(-0.1m, -1.2m), new GeoCoord(-0.2m, -1.4m)
                        };

            var geofence = new GeoFence {NumVerts = 4, Verts = verts};
            var expected = new BBox(-0.1m, -0.4m, -1.1m, -1.4m);
            var inside = new GeoCoord(-0.3m, -1.2m);
            var outside = new GeoCoord(0.0m, 0.0m);
            RepeatBoxTest(geofence, expected, inside, outside);
        }

        [Test]
        public void AroundZeroZero()
        {
            var verts = new[]
                        {
                            new GeoCoord(0.4m, -0.4m), new GeoCoord(0.4m, 0.4m),
                            new GeoCoord(-0.4m, 0.4m), new GeoCoord(-0.4m, -0.4m)
                        };

            var geofence = new GeoFence {NumVerts = 4, Verts = verts};
            var expected = new BBox(0.4m, -0.4m, 0.4m, -0.4m);
            var inside = new GeoCoord(-0.1m, -0.1m);
            var outside = new GeoCoord(1.0m, -1.0m);
            RepeatBoxTest(geofence, expected, inside, outside);
        }

        [Test]
        public void Transmeridian()
        {
            var verts = new[]
                        {
                            new GeoCoord(0.4m, Constants.H3.M_PI - 0.1m),
                            new GeoCoord(0.4m, -Constants.H3.M_PI + 0.1m),
                            new GeoCoord(-0.4m, -Constants.H3.M_PI + 0.1m),
                            new GeoCoord(-0.4m, Constants.H3.M_PI - 0.1m)
                        };

            var geofence = new GeoFence {NumVerts = 4, Verts = verts};
            var expected = new BBox(0.4m, -0.4m, -Constants.H3.M_PI + 0.1m, Constants.H3.M_PI - 0.1m);
            var insideOnMeridian = new GeoCoord(-0.1m, Constants.H3.M_PI);
            var outside = new GeoCoord(1.0m, Constants.H3.M_PI - 0.5m);
            RepeatBoxTest(geofence, expected, insideOnMeridian, outside);

            var westInside = new GeoCoord(0.1m, Constants.H3.M_PI - 0.05m);
            Assert.IsTrue(expected.Contains(westInside));

            var eastInside = new GeoCoord(0.1m, -Constants.H3.M_PI + 0.05m);
            Assert.IsTrue(expected.Contains(eastInside));

            var westOutside = new GeoCoord(0.1m, Constants.H3.M_PI - 0.5m);
            Assert.IsFalse(expected.Contains(westOutside));

            var eastOutside = new GeoCoord(0.1m, -Constants.H3.M_PI + 0.5m);
            Assert.IsFalse(expected.Contains(eastOutside));
        }

        [Test]
        public void EdgeOnNorthPole()
        {
            var verts = new[]
                        {
                            new GeoCoord(Constants.H3.M_PI_2 - 0.1m, 0.1m),
                            new GeoCoord(Constants.H3.M_PI_2 - 0.1m, 0.8m),
                            new GeoCoord(Constants.H3.M_PI_2, 0.8m),
                            new GeoCoord(Constants.H3.M_PI_2, 0.1m)
                        };
            var geofence = new GeoFence {NumVerts = 4, Verts = verts};
            var expected = new BBox(Constants.H3.M_PI_2, Constants.H3.M_PI_2 - 0.1m, 0.8m, 0.1m);
            var inside = new GeoCoord(Constants.H3.M_PI_2 - 0.01m, 0.4m);
            var outside = new GeoCoord(Constants.H3.M_PI_2, 0.9m);
            RepeatBoxTest(geofence, expected, inside, outside);
        }

        [Test]
        public void EdgeOnSouthPole()
        {
            var verts = new[]
                        {
                            new GeoCoord(-Constants.H3.M_PI_2 + 0.1m, 0.1m),
                            new GeoCoord(-Constants.H3.M_PI_2 + 0.1m, 0.8m),
                            new GeoCoord(-Constants.H3.M_PI_2, 0.8m),
                            new GeoCoord(-Constants.H3.M_PI_2, 0.1m)
                        };

            var geofence = new GeoFence {NumVerts = 4, Verts = verts};
            var expected = new BBox(-Constants.H3.M_PI_2 + 0.1m, -Constants.H3.M_PI_2, 0.8m, 0.1m);
            var inside = new GeoCoord(-Constants.H3.M_PI_2 + 0.01m, 0.4m);
            var outside = new GeoCoord(-Constants.H3.M_PI_2, 0.9m);
            RepeatBoxTest(geofence, expected, inside, outside);
        }
        
        [Test]
        public void ContainsEdges()
        {
            var bbox = new BBox(0.1m, -0.1m, 0.2m, -0.2m);
            var points = new[]
                         {
                             new GeoCoord(0.1m, 0.2m), new GeoCoord(0.1m, 0.0m),
                             new GeoCoord(0.1m, -0.2m), new GeoCoord(0.0m, 0.2m),
                             new GeoCoord(-0.1m, 0.2m), new GeoCoord(-0.1m, 0.0m),
                             new GeoCoord(-0.1m, -0.2m), new GeoCoord(0.0m, -0.2m)
                         };

            foreach (var point in points)
            {
                Assert.IsTrue(bbox.Contains(point));
            }
        }

        [Test]
        public void ContainsEdgesTransmeridian()
        {
            var bbox = new BBox(0.1m, -0.1m, -Constants.H3.M_PI + 0.2m, Constants.H3.M_PI - 0.2m);

            var points = new[]
                         {
                             new GeoCoord(0.1m, -Constants.H3.M_PI + 0.2m),
                             new GeoCoord(0.1m, Constants.H3.M_PI),
                             new GeoCoord(0.1m, Constants.H3.M_PI - 0.2m),
                             new GeoCoord(0.0m, -Constants.H3.M_PI + 0.2m),
                             new GeoCoord(-0.1m, -Constants.H3.M_PI + 0.2m),
                             new GeoCoord(-0.1m, Constants.H3.M_PI),
                             new GeoCoord(-0.1m, Constants.H3.M_PI - 0.2m),
                             new GeoCoord(0.0m, Constants.H3.M_PI - 0.2m)
                         };
            
            foreach (var point in points)
            {
                Assert.IsTrue(bbox.Contains(point));
            }
        }

        [Test]
        public void BboxCenterBasicQuadrants()
        {
            var bbox1 = new BBox(1.0m, 0.8m, 1.0m, 0.8m);
            var expected1 = new GeoCoord(0.9m, 0.9m);
            var center = bbox1.Center();
            Assert.AreEqual(center, expected1);

            var bbox2 = new BBox(-0.8m, -1.0m, 1.0m, 0.8m);
            var expected2 = new GeoCoord(-0.9m, 0.9m);
            center = bbox2.Center();
            Assert.AreEqual(center, expected2);

            var bbox3 = new BBox(1.0m, 0.8m, -0.8m, -1.0m);
            var expected3 = new GeoCoord(0.9m, -0.9m);
            center = bbox3.Center();
            Assert.AreEqual(center, expected3);

            var bbox4 = new BBox(-0.8m, -1.0m, -0.8m, -1.0m);
            var expected4 = new GeoCoord(-0.9m, -0.9m);
            center = bbox4.Center();
            Assert.AreEqual(center, expected4);

            var bbox5 = new BBox(0.8m, -0.8m, 1.0m, -1.0m);
            var expected5 = new GeoCoord(0.0m, 0.0m);
            center = bbox5.Center();
            Assert.AreEqual(center, expected5);
        }

        [Test]
        public void BboxCenterTransmeridian()
        {
            var bbox1 = new BBox(1.0m, 0.8m, -Constants.H3.M_PI + 0.3m, Constants.H3.M_PI - 0.1m);
            var expected1 = new GeoCoord(0.9m, -Constants.H3.M_PI + 0.1m);
            var center = bbox1.Center();
            Assert.AreEqual(center, expected1);

            var bbox2 = new BBox(1.0m, 0.8m, -Constants.H3.M_PI + 0.1m, Constants.H3.M_PI - 0.3m);
            var expected2 = new GeoCoord(0.9m, Constants.H3.M_PI - 0.1m);
            center = bbox2.Center();
            Assert.AreEqual(center, expected2);

            var bbox3 = new BBox(1.0m, 0.8m, -Constants.H3.M_PI + 0.1m, Constants.H3.M_PI - 0.1m);
            var expected3 = new GeoCoord(0.9m, Constants.H3.M_PI);
            center = bbox3.Center();
            Assert.AreEqual(center, expected3);
        }

        [Test]
        public void BboxIsTransmeridian()
        {
            var bboxNormal = new BBox(1.0m, 0.8m, 1.0m, 0.8m);
            Assert.IsFalse(bboxNormal.IsTransmeridian);

            var bboxTransmeridian = new BBox(1.0m, 0.8m, -Constants.H3.M_PI + 0.3m, Constants.H3.M_PI - 0.1m);
            Assert.IsTrue(bboxTransmeridian.IsTransmeridian);
        }

        [Test]
        public void BboxEquals()
        {
            var bbox = new BBox(1.0m, 0.0m, 1.0m, 0.0m);
            var north = bbox.ReplaceNorth(bbox.North + 0.1m);
            var south = bbox.ReplaceSouth(bbox.South + 0.1m);
            var east = bbox.ReplaceEast(bbox.East + 0.1m);
            var west = bbox.ReplaceWest(bbox.West + 0.1m);

            Assert.AreEqual(bbox,bbox);
            Assert.AreNotEqual(bbox,north);
            Assert.AreNotEqual(bbox,south);
            Assert.AreNotEqual(bbox,east);
            Assert.AreNotEqual(bbox,west);
        }
    }
}
