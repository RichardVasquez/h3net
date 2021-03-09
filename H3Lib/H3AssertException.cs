using System;

namespace H3Lib
{
    public class H3AssertException : Exception
    {
        public H3AssertException()
        {
        }

        public H3AssertException(string message) : base(message)
        {
        }

        public H3AssertException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
