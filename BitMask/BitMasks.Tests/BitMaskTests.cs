using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BitMasks.Tests
{
    public class BitMaskTests
    {
        [Fact]
        public void CanEqualsTrue()
        {
            var bits = new[] {1, 2, 3, 4, 10, 16, 17, 20, 30, 31, 63};

            var bitMask = new BitMask(bits);
            var bitMask2 = new BitMask(bits);

            Assert.True(bitMask == bitMask2);
        }

        [Fact]
        public void CanEqualsFalse()
        {
            var bitMask = new BitMask(new[] {1, 2, 3, 4, 10, 16, 17, 20, 30, 31, 63});
            var bitMask2 = new BitMask(new[] {1, 2, 4, 21, 29, 30});

            Assert.False(bitMask == bitMask2);
        }

        [Fact]
        public void CanHasTrue()
        {
            var bitMask = new BitMask(new[] {1, 2, 3, 4, 10, 16, 17, 20, 30, 31, 63});
            var bitMask2 = new BitMask(new[] {2, 4, 16, 17, 30, 63});

            Assert.True(bitMask.Has(bitMask2));
        }

        [Fact]
        public void CanHasFalse()
        {
            var bitMask = new BitMask(new[] {1, 2, 3, 4, 10, 16, 17, 20, 30, 31});
            var bitMask2 = new BitMask(new[] {1, 2, 3, 4, 9, 10, 16, 17, 20, 30, 31});

            Assert.False(bitMask.Has(bitMask2));
        }

        [Fact]
        public void CanAnd()
        {
            var bitMask = new BitMask(new[] {1, 2, 3, 4, 10, 16, 17, 20, 30, 31, 63});
            var bitMask2 = new BitMask(new[] {1, 2, 3, 4, 10, 17, 20, 30, 31, 63});
            var expectedResult = new BitMask(new[] {1, 2, 3, 4, 10, 17, 20, 30, 31, 63});

            Assert.Equal(expectedResult, bitMask & bitMask2);
        }

        [Fact]
        public void CanOr()
        {
            var bits = new[] {1, 2, 3, 4, 10, 16, 17, 20, 30, 31, 63};
            var orBits = new[] {1, 2, 3, 4, 10, 17, 20, 30, 31};

            var bitMask = new BitMask(bits);
            var bitMask2 = new BitMask(orBits);
            var expectedResult = new BitMask(bits.Union(orBits).ToArray());

            Assert.Equal(expectedResult, bitMask | bitMask2);
        }

        [Fact]
        public void CanNot()
        {
            var bits = new[] {1, 2, 3, 4, 10, 16, 17, 20, 30, 31};

            var bitMask = new BitMask(bits);
            var expectedResult = new BitMask(Enumerable.Range(0, BitMask.BitSize).Except(bits).ToArray());

            Assert.Equal(expectedResult, ~bitMask);
        }

        [Fact]
        public void CanIndex()
        {
            var allBits = Enumerable.Range(0, BitMask.BitSize).ToArray();
            var setBits = new[] {1, 2, 3, 6, 8, 12, 19, 34, 42, 61};

            var bitMask = new BitMask(bits: setBits);

            Assert.True(setBits.Select(b => bitMask[b]).All(v => v == true));
            Assert.True(allBits.Except(setBits).Select(b => bitMask[b]).All(v => v == false));
        }
    }
}
