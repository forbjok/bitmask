using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BitMasks
{
    using DataType = System.Int64;

    public unsafe struct BitMask : IEquatable<BitMask>
    {
        public static readonly BitMask None = new BitMask(bits: new int[0]);

        private const int MaskDataSize = 1;
        private const int BitsPerData = 8 * sizeof(DataType);
        private const int MaxBitIndex = (MaskDataSize * BitsPerData) - 1;

        private fixed DataType _data[MaskDataSize];

        public BitMask(int[] bits)
        {
            fixed (DataType* data = _data)
            {
                for (int i = 0; i < bits.Length; ++i)
                {
                    ref var bit = ref bits[i];

                    if (bit < 0 || bit > MaxBitIndex)
                        throw new Exception($"Attempted to set bit #{bit}, but the maximum is {MaxBitIndex}");

                    var dataIndex = bit / BitsPerData;
                    var bitIndex = bit % BitsPerData;

                    var mask = (DataType) 1 << bitIndex;

                    data[dataIndex] |= mask;
                }
            }
        }

        public bool Equals(BitMask other)
        {
            fixed (DataType* data = _data)
            {
                for (int i = 0; i < MaskDataSize; ++i)
                {
                    if (data[i] != other._data[i])
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
            var newBitMask = new BitMask();

            DataType* newData = newBitMask._data;
            DataType* mask1Data = mask1._data;
            DataType* mask2Data = mask2._data;

            for (int i = 0; i < MaskDataSize; ++i)
            {
                *newData = *mask1Data & *mask2Data;

                ++newData;
                ++mask1Data;
                ++mask2Data;
            }

            return newBitMask;
        }

        public static BitMask operator |(BitMask mask1, BitMask mask2)
        {
            var newBitMask = new BitMask();

            DataType* newData = newBitMask._data;
            DataType* mask1Data = mask1._data;
            DataType* mask2Data = mask2._data;

            for (int i = 0; i < MaskDataSize; ++i)
            {
                *newData = *mask1Data | *mask2Data;

                ++newData;
                ++mask1Data;
                ++mask2Data;
            }

            return newBitMask;
        }

        public static BitMask operator ~(BitMask mask)
        {
            var newBitMask = new BitMask();

            DataType* newData = newBitMask._data;
            DataType* maskData = mask._data;

            for (int i = 0; i < MaskDataSize; ++i)
            {
                *newData = ~*maskData;

                ++newData;
                ++maskData;
            }

            return newBitMask;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has(BitMask mask)
        {
            fixed (DataType* data = _data)
            {
                DataType* myData = data;
                DataType* maskData = mask._data;

                for (int i = 0; i < MaskDataSize; ++i)
                {
                    if ((*myData & *maskData) != *maskData)
                        return false;

                    ++myData;
                    ++maskData;
                }
            }

            return true;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            fixed (DataType* data = _data)
            {
                for (int i = 0; i < MaskDataSize; ++i)
                {
                    var binaryString = Convert.ToString(data[i], 2);

                    builder.Append(binaryString.PadLeft(BitsPerData * MaskDataSize, '0'));
                }
            }

            return builder.ToString();
        }
    }
}
