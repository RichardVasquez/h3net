using System;

namespace H3Lib
{
    /// <summary>
    /// A single node in a vertex graph, part of a linked list
    /// </summary>
    public readonly struct VertexNode : IEquatable<VertexNode>
    {
        public readonly GeoCoord From;
        public readonly GeoCoord To;

        public VertexNode(GeoCoord toNode, GeoCoord fromNode)
        {
            To = toNode;
            From = fromNode;
        }

        public bool Equals(VertexNode other)
        {
            return From.Equals(other.From) && To.Equals(other.To);
        }

        public override bool Equals(object obj)
        {
            return obj is VertexNode other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(From, To);
        }

        public static bool operator ==(VertexNode left, VertexNode right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(VertexNode left, VertexNode right)
        {
            return !left.Equals(right);
        }
    }
}
