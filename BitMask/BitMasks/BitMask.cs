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

        private const int MaskInts = 2;
        private const int BitsPerInt = 8 * sizeof(int);

        private fixed int _ints[MaskInts];

        public BitMask(int[] bits)
        {
            fixed (int* ints = _ints)
            {
                for (int i = 0; i < bits.Length; ++i)
                {
                    ref var bit = ref bits[i];

                    int intIndex = bit / BitsPerInt;
                    int bitIndex = bit % BitsPerInt;
                    int mask = 1 << bitIndex;

                    ints[intIndex] |= mask;
                }
            }
        }

        private static BitMask CreateWithInts(int[] ints)
        {
            var bitMask = new BitMask();

            for (int i = 0; i < MaskInts; ++i)
            {
                bitMask._ints[i] = ints[i];
            }

            return bitMask;
        }

        public bool Equals(BitMask other)
        {
            fixed (int* ints = _ints)
            {
                for (int i = 0; i < MaskInts; ++i)
                {
                    if (ints[i] != other._ints[i])
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
            var newInts = new int[MaskInts];

            for (int i = 0; i < MaskInts; ++i)
            {
                newInts[i] = mask1._ints[i] & mask2._ints[i];
            }

            return CreateWithInts(newInts);
        }

        public static BitMask operator |(BitMask mask1, BitMask mask2)
        {
            var newInts = new int[MaskInts];

            for (int i = 0; i < MaskInts; ++i)
            {
                newInts[i] = mask1._ints[i] | mask2._ints[i];
            }

            return CreateWithInts(newInts);
        }

        public static BitMask operator ~(BitMask mask)
        {
            var newInts = new int[MaskInts];

            for (int i = 0; i < MaskInts; ++i)
            {
                newInts[i] = ~mask._ints[i];
            }

            return CreateWithInts(newInts);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has(BitMask mask)
        {
            fixed (int* ints = _ints)
            {
                int* myInts = ints;
                int* maskInts = mask._ints;

                for (int i = 0; i < MaskInts; ++i)
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
