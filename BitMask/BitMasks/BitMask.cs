using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMasks
{
    public struct BitMask : IEquatable<BitMask>
    {
        public static readonly BitMask None = new BitMask(bits: new int[0]);

        private const int MaskBytes = 4;
        private const int BitsPerByte = 8;

        private readonly byte[] _bytes;

        public BitMask(int[] bits)
        {
            _bytes = new byte[MaskBytes];

            for (int i = 0; i < bits.Length; ++i)
            {
                ref var bit = ref bits[i];

                int byteIndex = bit / BitsPerByte;
                int bitIndex = bit % BitsPerByte;
                byte mask = (byte) (1 << bitIndex);

                ref var targetByte = ref _bytes[byteIndex];

                targetByte |= mask;
            }
        }

        private BitMask(byte[] bytes)
        {
            _bytes = bytes;
        }

        public bool Equals(BitMask other)
        {
            for (int i = 0; i < MaskBytes; ++i)
            {
                if (_bytes[i] != other._bytes[i])
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
            var newBytes = new byte[MaskBytes];

            for (int i = 0; i < MaskBytes; ++i)
            {
                newBytes[i] = (byte) (mask1._bytes[i] & mask2._bytes[i]);
            }

            return new BitMask(bytes: newBytes);
        }

        public static BitMask operator |(BitMask mask1, BitMask mask2)
        {
            var newBytes = new byte[MaskBytes];

            for (int i = 0; i < MaskBytes; ++i)
            {
                newBytes[i] = (byte) (mask1._bytes[i] | mask2._bytes[i]);
            }

            return new BitMask(bytes: newBytes);
        }

        public static BitMask operator ~(BitMask mask)
        {
            var newBytes = new byte[MaskBytes];

            for (int i = 0; i < MaskBytes; ++i)
            {
                newBytes[i] = (byte) ~mask._bytes[i];
            }

            return new BitMask(bytes: newBytes);
        }

        public bool Has(BitMask mask)
        {
            for (int i = 0; i < MaskBytes; ++i)
            {
                if ((_bytes[i] & mask._bytes[i]) != mask._bytes[i])
                    return false;
            }

            return true;
        }
    }
}
