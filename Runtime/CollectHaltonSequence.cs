using UnityEngine;

namespace LazyCoder.Collect
{
    public static class CollectHaltonSequence
    {
        public static Vector3 CurrentPosition = Vector3.zero;

        private static long _base2 = 0;
        private static long _base3 = 0;
        private static long _base5 = 0;

        public static long Increment(bool useX = true, bool useY = true, bool useZ = true)
        {
            float fOneOver3 = 1.0f / 3.0f;
            float fOneOver5 = 1.0f / 5.0f;

            long oldBase2 = _base2;
            _base2++;
            long diff = _base2 ^ oldBase2;

            float s = 0.5f;

            if (useX)
            {
                do
                {
                    if ((oldBase2 & 1) == 1)
                        CurrentPosition.x -= s;
                    else
                        CurrentPosition.x += s;

                    s *= 0.5f;

                    diff = diff >> 1;
                    oldBase2 = oldBase2 >> 1;
                }
                while (diff > 0);
            }

            long bitmask = 0x3;
            long bitadd = 0x1;
            s = fOneOver3;

            _base3++;

            if (useY)
            {
                while (true)
                {
                    if ((_base3 & bitmask) == bitmask)
                    {
                        _base3 += bitadd;
                        CurrentPosition.y -= 2 * s;

                        bitmask = bitmask << 2;
                        bitadd = bitadd << 2;

                        s *= fOneOver3;
                    }
                    else
                    {
                        CurrentPosition.y += s;
                        break;
                    }
                };
            }

            bitmask = 0x7;
            bitadd = 0x3;
            long dmax = 0x5;

            s = fOneOver5;

            _base5++;

            if (useZ)
            {
                while (true)
                {
                    if ((_base5 & bitmask) == dmax)
                    {
                        _base5 += bitadd;
                        CurrentPosition.z -= 4 * s;

                        bitmask = bitmask << 3;
                        dmax = dmax << 3;
                        bitadd = bitadd << 3;

                        s *= fOneOver5;
                    }
                    else
                    {
                        CurrentPosition.z += s;
                        break;
                    }
                };
            }

            return _base2;
        }

        public static void Reset()
        {
            CurrentPosition = Vector3.zero;

            _base2 = 0;
            _base3 = 0;
            _base5 = 0;
        }
    }
}