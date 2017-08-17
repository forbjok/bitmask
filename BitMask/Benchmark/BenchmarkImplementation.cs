using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitMasks;

namespace Benchmark
{
    using IntType = UInt64;

    public class BenchmarkImplementation
    {
        private const int BitSize = sizeof(IntType) * 8;

        public enum ComponentType : IntType
        {
            None = 0,
            Component1 = (IntType) 1 << 0,
            Component2 = (IntType) 1 << 1,
            Component3 = (IntType) 1 << 2,
            Component4 = (IntType) 1 << 3,
            Component5 = (IntType) 1 << 4,
            Component6 = (IntType) 1 << 5,
            Component7 = (IntType) 1 << 6,
            Component8 = (IntType) 1 << 7,
            Component9 = (IntType) 1 << 8,
            Component10 = (IntType) 1 << 9,
            Component11 = (IntType) 1 << 10,
            Component12 = (IntType) 1 << 11,
            Component13 = (IntType) 1 << 12,
            Component14 = (IntType) 1 << 13,
            Component15 = (IntType) 1 << 14,
            Component16 = (IntType) 1 << 15,
            Component17 = (IntType) 1 << 16,
            Component18 = (IntType) 1 << 17,
            Component19 = (IntType) 1 << 18,
            Component20 = (IntType) 1 << 19,
            Component21 = (IntType) 1 << 20,
            Component22 = (IntType) 1 << 21,
            Component23 = (IntType) 1 << 22,
            Component24 = (IntType) 1 << 23,
            Component25 = (IntType) 1 << 24,
            Component26 = (IntType) 1 << 25,
            Component27 = (IntType) 1 << 26,
            Component28 = (IntType) 1 << 27,
            Component29 = (IntType) 1 << 28,
            Component30 = (IntType) 1 << 29,
            Component31 = (IntType) 1 << 30,
            Component32 = (IntType) 1 << 31,
            Component33 = (IntType) 1 << 32,
            Component34 = (IntType) 1 << 33,
            Component35 = (IntType) 1 << 34,
            Component36 = (IntType) 1 << 35,
            Component37 = (IntType) 1 << 36,
            Component38 = (IntType) 1 << 37,
            Component39 = (IntType) 1 << 38,
            Component40 = (IntType) 1 << 39,
            Component41 = (IntType) 1 << 40,
            Component42 = (IntType) 1 << 41,
            Component43 = (IntType) 1 << 42,
            Component44 = (IntType) 1 << 43,
            Component45 = (IntType) 1 << 44,
            Component46 = (IntType) 1 << 45,
            Component47 = (IntType) 1 << 46,
            Component48 = (IntType) 1 << 47,
            Component49 = (IntType) 1 << 48,
            Component50 = (IntType) 1 << 49,
            Component51 = (IntType) 1 << 50,
            Component52 = (IntType) 1 << 51,
            Component53 = (IntType) 1 << 52,
            Component54 = (IntType) 1 << 53,
            Component55 = (IntType) 1 << 54,
            Component56 = (IntType) 1 << 55,
            Component57 = (IntType) 1 << 56,
            Component58 = (IntType) 1 << 57,
            Component59 = (IntType) 1 << 58,
            Component60 = (IntType) 1 << 59,
            Component61 = (IntType) 1 << 60,
            Component62 = (IntType) 1 << 61,
            Component63 = (IntType) 1 << 62,
            Component64 = (IntType) 1 << 63,
        }

        private const int NumberOfOperations = 100000;
        private const int NumberOfBits = 8;

        private static readonly int[] WantedBits = {1, 6, 9, 10, 14, 17, 29, 31, 42, 57, 58, 61, 62};

        private ComponentType[] _enumComponentMasks;
        private ComponentType _wantedEnumMask;

        private int[] _results;

        private BitMask[] _componentBitMasks;
        private BitMask _wantedBitMask;

        private IntType[] _intComponentMasks;
        private IntType _wantedIntMask;

        private BitArray[] _bitArrayComponentMasks;
        private BitArray _wantedBitArrayMask;

        public BenchmarkImplementation()
        {
            _results = new int[NumberOfOperations];

            /* Set up EnumFlags benchmark */
            _enumComponentMasks = new ComponentType[NumberOfOperations];
            _componentBitMasks = new BitMask[NumberOfOperations];

            _wantedEnumMask = ComponentType.None;
            _wantedBitMask = new BitMask(bits: WantedBits);

            _intComponentMasks = new IntType[NumberOfOperations];
            _wantedIntMask = 0;

            _bitArrayComponentMasks = new BitArray[NumberOfOperations];
            _wantedBitArrayMask = new BitArray(BitSize);

            var random = new Random();

            for (int i = 0; i < WantedBits.Length; ++i)
            {
                var bit = WantedBits[i];

                _wantedEnumMask |= (ComponentType) ((IntType) 1 << bit);
                _wantedIntMask |= (IntType) 1 << bit;
                _wantedBitArrayMask.Set(bit, true);
            }

            for (int i = 0; i < NumberOfOperations; ++i)
            {
                ComponentType enumComponentMask = ComponentType.None;
                BitMask componentBitMask = BitMask.None;
                IntType intComponentMask = 0;
                var bitArrayComponentMask = new BitArray(BitSize);

                for (int b = 0; b < NumberOfBits; ++b)
                {
                    var bit = random.Next(0, 31);

                    enumComponentMask |= (ComponentType) ((IntType) 1 << bit);
                    componentBitMask |= new BitMask(bits: new[] {bit});
                    intComponentMask |= (IntType) 1 << bit;
                    bitArrayComponentMask.Set(bit, true);
                }

                _enumComponentMasks[i] = enumComponentMask;
                _componentBitMasks[i] = componentBitMask;
                _intComponentMasks[i] = intComponentMask;
                _bitArrayComponentMasks[i] = bitArrayComponentMask;
            }
        }

        public void BenchmarkEnumFlags()
        {
            int resultId = 0;

            for (int i = 0; i < NumberOfOperations; ++i)
            {
                ref var componentMask = ref _enumComponentMasks[i];

                if ((componentMask & _wantedEnumMask) == _wantedEnumMask)
                    continue;

                _results[resultId++] = i;
            }
        }

        public void BenchmarkBitMask()
        {
            int resultId = 0;

            for (int i = 0; i < NumberOfOperations; ++i)
            {
                ref var componentMask = ref _componentBitMasks[i];

                if (componentMask.Has(_wantedBitMask))
                    continue;

                _results[resultId++] = i;
            }
        }

        public void BenchmarkIntFlags()
        {
            int resultId = 0;

            for (int i = 0; i < NumberOfOperations; ++i)
            {
                ref var componentMask = ref _intComponentMasks[i];

                if ((componentMask & _wantedIntMask) == _wantedIntMask)
                    continue;

                _results[resultId++] = i;
            }
        }

        public void BenchmarkBitArray()
        {
            int resultId = 0;

            for (int i = 0; i < NumberOfOperations; ++i)
            {
                ref var componentMask = ref _bitArrayComponentMasks[i];

                /* Because BitArray modifies its internal value in-place when using And(),
                 * trashing the original component mask, we need to make a copy of it
                 * before performing the check. */
                var componentMaskCopy = new BitArray(componentMask);

                if (componentMaskCopy.And(_wantedBitArrayMask) == _wantedBitArrayMask)
                    continue;

                _results[resultId++] = i;
            }
        }

    }
}
