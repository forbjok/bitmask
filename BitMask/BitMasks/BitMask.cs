using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BitMasks
{
    using DataType = UInt64;

    public struct BitMask : IEquatable<BitMask>
    {
        public static readonly BitMask None = new BitMask(bits: new int[0]);

        private const int MaskDataSize = 2;
        private const int BitsPerData = 8 * sizeof(DataType);
        private const int MaxBitIndex = (MaskDataSize * BitsPerData) - 1;

        public const int BitSize = MaskDataSize * BitsPerData;

        private DataType _field0;
        private DataType _field1;

        public BitMask(int[] bits)
        {
            _field0 = 0;
            _field1 = 0;

            for (int i = 0; i < bits.Length; ++i)
            {
                ref var bit = ref bits[i];

                if (bit < 0 || bit > MaxBitIndex)
                    throw new Exception($"Attempted to set bit #{bit}, but the maximum is {MaxBitIndex}");

                var dataIndex = bit / BitsPerData;
                var bitIndex = bit % BitsPerData;

                var mask = (DataType) 1 << bitIndex;

                switch (dataIndex)
                {
                    case 0:
                        _field0 |= mask;
                        break;
                    case 1:
                        _field1 |= mask;
                        break;
                    default:
                        throw new Exception($"Nonexistent field: {dataIndex}");
                }
            }
        }

        public bool this[int index]
        {
            get
            {
                if (index < 0 || index > MaxBitIndex)
                    throw new Exception($"Invalid bit index: {index}");

                var dataIndex = index / BitsPerData;
                var bitIndex = index % BitsPerData;

                switch (dataIndex)
                {
                    case 0:
                        return (_field0 & ((DataType) 1 << bitIndex)) != 0;
                    case 1:
                        return (_field1 & ((DataType) 1 << bitIndex)) != 0;
                    default:
                        return false;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BitMask other)
        {
            if (_field0 != other._field0)
                return false;

            if (_field1 != other._field1)
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is BitMask)
                return Equals((BitMask) obj);

            return base.Equals(obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(BitMask mask1, BitMask mask2)
        {
            return mask1.Equals(mask2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(BitMask mask1, BitMask mask2)
        {
            return !mask1.Equals(mask2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BitMask operator &(BitMask mask1, BitMask mask2)
        {
            var newBitMask = new BitMask();

            newBitMask._field0 = mask1._field0 & mask2._field0;
            newBitMask._field1 = mask1._field1 & mask2._field1;

            return newBitMask;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BitMask operator |(BitMask mask1, BitMask mask2)
        {
            var newBitMask = new BitMask();

            newBitMask._field0 = mask1._field0 | mask2._field0;
            newBitMask._field1 = mask1._field1 | mask2._field1;

            return newBitMask;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BitMask operator ~(BitMask mask)
        {
            var newBitMask = new BitMask();

            newBitMask._field0 = ~mask._field0;
            newBitMask._field1 = ~mask._field1;

            return newBitMask;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has(BitMask mask)
        {
            if ((_field0 & mask._field0) != mask._field0)
                return false;

            if ((_field1 & mask._field1) != mask._field1)
                return false;

            return true;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            var fields = new DataType[MaskDataSize];

            fields[0] = _field0;
            fields[1] = _field1;

            for (int i = 0; i < MaskDataSize; ++i)
            {
                var binaryString = Convert.ToString((long) fields[i], 2);

                builder.Append(binaryString.PadLeft(BitsPerData, '0'));
            }

            return builder.ToString();
        }
    }
}
