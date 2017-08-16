using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BitMasks
{
    public struct BitMask : IEquatable<BitMask>
    {
        [Flags]
        private enum BitEnum
        {
            None = 0,
            B1 = 1 << 0,
            B2 = 1 << 1,
            B3 = 1 << 2,
            B4 = 1 << 3,
            B5 = 1 << 4,
            B6 = 1 << 5,
            B7 = 1 << 6,
            B8 = 1 << 7,
            B9 = 1 << 8,
            B10 = 1 << 9,
            B11 = 1 << 10,
            B12 = 1 << 11,
            B13 = 1 << 12,
            B14 = 1 << 13,
            B15 = 1 << 14,
            B16 = 1 << 15,
            B17 = 1 << 16,
            B18 = 1 << 17,
            B19 = 1 << 18,
            B20 = 1 << 19,
            B21 = 1 << 20,
            B22 = 1 << 21,
            B23 = 1 << 22,
            B24 = 1 << 23,
            B25 = 1 << 24,
            B26 = 1 << 25,
            B27 = 1 << 26,
            B28 = 1 << 27,
            B29 = 1 << 28,
            B30 = 1 << 29,
            B31 = 1 << 30,
            B32 = 1 << 31,
        }

        public static readonly BitMask None = new BitMask(bits: new int[0]);

        private const int MaskEnums = 1;
        private const int BitsPerEnum = 8 * sizeof(BitEnum);

        private BitEnum[] _enums;

        public BitMask(int[] bits)
        {
            _enums = new BitEnum[MaskEnums];

            for (int i = 0; i < bits.Length; ++i)
            {
                ref var bit = ref bits[i];

                int byteIndex = bit / BitsPerEnum;
                int bitIndex = bit % BitsPerEnum;
                var mask = (BitEnum) (1 << bitIndex);

                _enums[byteIndex] |= mask;
            }
        }

        private BitMask(BitEnum[] enums)
        {
            _enums = new BitEnum[MaskEnums];

            for (int i = 0; i < MaskEnums; ++i)
            {
                _enums[i] = enums[i];
            }
        }

        public bool Equals(BitMask other)
        {
            for (int i = 0; i < MaskEnums; ++i)
            {
                if (_enums[i] != other._enums[i])
                    return false;
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
            var newBytes = new BitEnum[MaskEnums];

            for (int i = 0; i < MaskEnums; ++i)
            {
                newBytes[i] = mask1._enums[i] & mask2._enums[i];
            }

            return new BitMask(enums: newBytes);
        }

        public static BitMask operator |(BitMask mask1, BitMask mask2)
        {
            var newBytes = new BitEnum[MaskEnums];

            for (int i = 0; i < MaskEnums; ++i)
            {
                newBytes[i] = mask1._enums[i] | mask2._enums[i];
            }

            return new BitMask(enums: newBytes);
        }

        public static BitMask operator ~(BitMask mask)
        {
            var newBytes = new BitEnum[MaskEnums];

            for (int i = 0; i < MaskEnums; ++i)
            {
                newBytes[i] = ~mask._enums[i];
            }

            return new BitMask(enums: newBytes);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has(BitMask mask)
        {
            for (int i = 0; i < MaskEnums; ++i)
            {
                ref var maskBytes = ref mask._enums[i];

                if ((_enums[i] & maskBytes) != maskBytes)
                    return false;
            }

            return true;
        }
    }
}
