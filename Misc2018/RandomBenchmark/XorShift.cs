using System;
using System.Security.Cryptography;
using System.Threading;

namespace RandomBenchmark
{
    public class XorShift
    {
        private uint _x;
        private uint _y;
        private uint _z;
        private uint _w;

        public XorShift()
            : this(123456789, 362436069, 521288629, 88675123)
        { }

        public XorShift(int x, int y, int z, int w)
        {
            unchecked
            {
                _x = (uint)x;
                _y = (uint)y;
                _z = (uint)z;
                _w = (uint)w;
            }
        }

        public int Next()
        {
            var t = _x ^ (_x << 11);

            _x = _y;
            _y = _z;
            _z = _w;

            unchecked
            {
                return (int)(_w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8)));
            }
        }
    }
}