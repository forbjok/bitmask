using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitMasks;

namespace Benchmark
{
    public class BenchmarkImplementation
    {
        public enum ComponentType : long
        {
            None = 0,
            Component1 = 1 << 0,
            Component2 = 1 << 1,
            Component3 = 1 << 2,
            Component4 = 1 << 3,
            Component5 = 1 << 4,
            Component6 = 1 << 5,
            Component7 = 1 << 6,
            Component8 = 1 << 7,
            Component9 = 1 << 8,
            Component10 = 1 << 9,
            Component11 = 1 << 10,
            Component12 = 1 << 11,
            Component13 = 1 << 12,
            Component14 = 1 << 13,
            Component15 = 1 << 14,
            Component16 = 1 << 15,
            Component17 = 1 << 16,
            Component18 = 1 << 17,
            Component19 = 1 << 18,
            Component20 = 1 << 19,
            Component21 = 1 << 20,
            Component22 = 1 << 21,
            Component23 = 1 << 22,
            Component24 = 1 << 23,
            Component25 = 1 << 24,
            Component26 = 1 << 25,
            Component27 = 1 << 26,
            Component28 = 1 << 27,
            Component29 = 1 << 28,
            Component30 = 1 << 29,
            Component31 = 1 << 30,
            Component32 = 1 << 31,
            Component33 = 1 << 32,
            Component34 = 1 << 33,
            Component35 = 1 << 34,
            Component36 = 1 << 35,
            Component37 = 1 << 36,
            Component38 = 1 << 37,
            Component39 = 1 << 38,
            Component40 = 1 << 39,
            Component41 = 1 << 40,
            Component42 = 1 << 41,
            Component43 = 1 << 42,
            Component44 = 1 << 43,
            Component45 = 1 << 44,
            Component46 = 1 << 45,
            Component47 = 1 << 46,
            Component48 = 1 << 47,
            Component49 = 1 << 48,
            Component50 = 1 << 49,
            Component51 = 1 << 50,
            Component52 = 1 << 51,
            Component53 = 1 << 52,
            Component54 = 1 << 53,
            Component55 = 1 << 54,
            Component56 = 1 << 55,
            Component57 = 1 << 56,
            Component58 = 1 << 57,
            Component59 = 1 << 58,
            Component60 = 1 << 59,
            Component61 = 1 << 60,
            Component62 = 1 << 61,
            Component63 = 1 << 62,
            Component64 = 1 << 63,
        }

        private const int NumberOfOperations = 100000;
        private const int NumberOfBits = 8;

        private static readonly int[] WantedBits = new[] {58};

        private ComponentType[] _enumComponentMasks;
        private ComponentType _wantedEnumMask;

        private int[] _results;

        private BitMask[] _componentBitMasks;
        private BitMask _wantedBitMask;

        private long[] _intComponentMasks;
        private long _wantedIntMask;


        public BenchmarkImplementation()
        {
            _results = new int[NumberOfOperations];

            /* Set up EnumFlags benchmark */
            _enumComponentMasks = new ComponentType[NumberOfOperations];
            _componentBitMasks = new BitMask[NumberOfOperations];

            _wantedEnumMask = ComponentType.None;
            _wantedBitMask = new BitMask(bits: WantedBits);

            _intComponentMasks = new long[NumberOfOperations];
            _wantedIntMask = 0;

            var random = new Random();

            for (int i = 0; i < WantedBits.Length; ++i)
            {
                var bit = WantedBits[i];

                _wantedEnumMask |= (ComponentType) (1 << bit);
                _wantedIntMask |= (1 << bit);
            }

            for (int i = 0; i < NumberOfOperations; ++i)
            {
                ComponentType enumComponentMask = ComponentType.None;
                BitMask componentBitMask = BitMask.None;
                int intComponentMask = 0;

                for (int b = 0; b < NumberOfBits; ++b)
                {
                    var bit = random.Next(0, 31);

                    enumComponentMask |= (ComponentType) (1 << bit);
                    componentBitMask |= new BitMask(bits: new[] {bit});
                    intComponentMask |= (1 << bit);
                }

                _enumComponentMasks[i] = enumComponentMask;
                _componentBitMasks[i] = componentBitMask;
                _intComponentMasks[i] = intComponentMask;
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
    }
}
