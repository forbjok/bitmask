using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BitMasks
{
    public unsafe struct BitMask : IEquatable<BitMask>
    {
        public static readonly BitMask None = new BitMask(bits: new int[0]);

        private const int MaskLongs = 1;
        private const int BitsPerLong = 8 * sizeof(long);

        private fixed long _longs[MaskLongs];

        public BitMask(int[] bits)
        {
            fixed (long* longs = _longs)
            {
                for (int i = 0; i < bits.Length; ++i)
                {
                    ref var bit = ref bits[i];

                    int intIndex = bit / BitsPerLong;
                    int bitIndex = bit % BitsPerLong;
                    long mask = (long) 1 << bitIndex;

                    longs[intIndex] |= mask;
                }
            }
        }

        private BitMask(long[] data)
        {
            fixed (long* longs = _longs)
            {
                for (int i = 0; i < MaskLongs; ++i)
                {
                    longs[i] = data[i];
                }
            }
        }

        public bool Equals(BitMask other)
        {
            fixed (long* longs = _longs)
            {
                for (int i = 0; i < MaskLongs; ++i)
                {
                    if (longs[i] != other._longs[i])
                        return false;
                }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is BitMask)
                return Equals((BitMask) obj);

            return base.Equals(obj);
        }

        public static bool operator ==(BitMask mask1, BitMask mask2)
        {
            return mask1.Equals(mask2);
        }

        public static bool operator !=(BitMask mask1, BitMask mask2)
        {
            return !mask1.Equals(mask2);
        }

        public static BitMask operator &(BitMask mask1, BitMask mask2)
        {
            var newLongs = new long[MaskLongs];

            for (int i = 0; i < MaskLongs; ++i)
            {
                newLongs[i] = mask1._longs[i] & mask2._longs[i];
            }

            return new BitMask(data: newLongs);
        }

        public static BitMask operator |(BitMask mask1, BitMask mask2)
        {
            var newLongs = new long[MaskLongs];

            for (int i = 0; i < MaskLongs; ++i)
            {
                newLongs[i] = mask1._longs[i] | mask2._longs[i];
            }

            return new BitMask(data: newLongs);
        }

        public static BitMask operator ~(BitMask mask)
        {
            var newLongs = new long[MaskLongs];

            for (int i = 0; i < MaskLongs; ++i)
            {
                newLongs[i] = ~mask._longs[i];
            }

            return new BitMask(data: newLongs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has(BitMask mask)
        {
            fixed (long* longs = _longs)
            {
                long* myInts = longs;
                long* maskInts = mask._longs;

                for (int i = 0; i < MaskLongs; ++i)
                {
                    if ((*myInts & *maskInts) != *maskInts)
                        return false;

                    ++myInts;
                    ++maskInts;
                }
            }

            return true;
        }
    }
}
