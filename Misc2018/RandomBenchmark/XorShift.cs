using System;
using System.Security.Cryptography;
using System.Threading;

namespace RandomBenchmark
{
    public class XorShift
    {
        private int _x;
        private int _y;
        private int _z;
        private int _w;

        public XorShift()
            : this(123456789, 362436069, 521288629, 88675123)
        { }

        public XorShift(int x, int y, int z, int w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }

        public int Next()
        {
            var t = _x ^ (_x << 11);

            _x = _y;
            _y = _z;
            _z = _w;
            return _w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8));
        }
    }
}