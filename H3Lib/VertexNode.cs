using System;
using System.Diagnostics;

namespace H3Lib
{
    /// <summary>
    /// A single node in a vertex graph, part of a linked list
    /// </summary>
    [DebuggerDisplay("From: {From} => To: {To}")]
    public readonly struct VertexNode : IEquatable<VertexNode>
    {
        /// <summary>
        /// Where the edge starts
        /// </summary>
        public readonly GeoCoord From;
        /// <summary>
        /// Where the edge ends
        /// </summary>
        public readonly GeoCoord To;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="toNode"></param>
        /// <param name="fromNode"></param>
        public VertexNode(GeoCoord toNode, GeoCoord fromNode)
        {
            To = toNode;
            From = fromNode;
        }

        /// <summary>
        /// Equality test
        /// </summary>
        public bool Equals(VertexNode other)
        {
            return From.Equals(other.From) && To.Equals(other.To);
        }

        /// <summary>
        /// Equality test against unboxed object
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is VertexNode other && Equals(other);
        }

        /// <summary>
        /// Hashcode for identity
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(From, To);
        }

        /// <summary>
        /// equality operator
        /// </summary>
        public static bool operator ==(VertexNode left, VertexNode right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// inequality operator
        /// </summary>
        public static bool operator !=(VertexNode left, VertexNode right)
        {
            return !left.Equals(right);
        }
    }
}
