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
 * Original implementation, Copyright 2016-2017 Uber Technologies, Inc.,
 * available at: https://github.com/uber/h3
 *
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using h3net.API;
using h3net.Code;

namespace h3net.Types {
    [DebuggerDisplay("{Value} - {ToString()}")]
    public struct H3Index
    {
        public ulong Value { get; }
        public int Mode => H3IndexCode.H3_GET_MODE(Value);
        public bool IsValid => H3IndexCode.IsValid(Value);
        public int BaseCell => H3IndexCode.h3GetBaseCell(Value);
        public int Resolution => H3IndexCode.h3GetResolution(Value);
        public int ReservedBits => H3IndexCode.H3_GET_RESERVED_BITS(Value);
        public bool IsPentagon => H3IndexCode.NetH3IsPentagon(Value);

    #region Constructors
        
        public H3Index(ulong val)
        {
            Value = val;
        }

        public H3Index(string s)
        {
            var h3 = H3IndexCode.StringToH3(s);
            Value = h3.Value;
        }
    #endregion

    #region Comparers/Convertors
        public bool Equals(H3Index other)
        {
            return Value == other.Value;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj.GetType() == GetType() && Equals((H3Index) obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(H3Index h1, int i2)
        {
            return h1.Value == (ulong)i2;
        }

        public static bool operator !=(H3Index h1, int i2)
        {
            return !(h1==i2);
        }

        public static bool operator ==(int i1, H3Index h2)
        {
            return h2.Value == (ulong)i1;
        }

        public static bool operator !=(int i1, H3Index h2)
        {
            return !(i1==h2);
        }

        public static bool operator ==(H3Index h1, H3Index h2)
        {
            return h1.Value == h2.Value;
        }

        public static bool operator !=(H3Index h1, H3Index h2)
        {
            return !(h1 == h2);
        }

        public static bool operator ==(H3Index h1, ulong u2)
        {
            return h1.Value == u2;
        }

        public static bool operator !=(H3Index  u1, ulong u2)
        {
            return !(u1 == u2);
        }

        public static bool operator ==(ulong u1, H3Index h2)
        {
            return h2.Value == u1;
        }

        public static bool operator !=(ulong  u1, H3Index h2)
        {
            return !(u1 == h2);
        }

        public static implicit operator H3Index(ulong u)
        {
            H3Index h3 = new H3Index(u);
            return h3;
        }

        public static implicit operator H3Index(ushort u)
        {
            ulong u2 = u;
            H3Index h3 = new H3Index(u2);
            return h3;
        }

        public static implicit operator H3Index(uint u)
        {
            ulong u2 = u;
            H3Index h3 = new H3Index(u2);
            return h3;
        }

        public static implicit operator H3Index(int i)
        {
            if (i < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            ulong u2 = (ulong) i;
            H3Index h3 = new H3Index(u2);
            return h3;
        }

        public static implicit operator ulong(H3Index h3)
        {
            return h3.Value;
        }

    #endregion

        public override string ToString()
        {
            return Value.ToString("X").ToLower();
        }

        //----------------------------------------------------

        public H3Index SetIndexDigit(int res, ulong digit)
        {
            var h3 = H3IndexCode.H3_SET_INDEX_DIGIT(Value, res, digit);
            return new H3Index(h3);
        }

        public H3Index SetReservedBits(ulong v)
        {
            var h3 = H3IndexCode.H3_SET_RESERVED_BITS(Value, v);
            return new H3Index(h3);
        }

        public List<H3Index> GetChildren(int resolution)
        {
            return H3IndexCode.h3ToChildren(Value, resolution);
        }
        
    }
}