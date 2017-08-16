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
            var bitMask = new BitMask(new[] {1, 2, 3, 4, 10, 16, 17, 20, 30, 31});
            var bitMask2 = new BitMask(new[] {1, 2, 3, 4, 10, 16, 17, 20, 30, 31});

            Assert.True(bitMask == bitMask2);
        }

        [Fact]
        public void CanEqualsFalse()
        {
            var bitMask = new BitMask(new[] {1, 2, 3, 4, 10, 16, 17, 20, 30, 31});
            var bitMask2 = new BitMask(new[] {1, 2, 4, 21, 29, 30});

            Assert.False(bitMask == bitMask2);
        }

        [Fact]
        public void CanHasTrue()
        {
            var bitMask = new BitMask(new[] {1, 2, 3, 4, 10, 16, 17, 20, 30, 31});
            var bitMask2 = new BitMask(new[] {2, 4, 16, 17, 30});

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
            var bitMask = new BitMask(new[] {1, 2, 3, 4, 10, 16, 17, 20, 30});
            var bitMask2 = new BitMask(new[] {1, 2, 3, 4, 10, 17, 20, 30, 31});
            var expectedResult = new BitMask(new[] {1, 2, 3, 4, 10, 17, 20, 30});

            Assert.Equal(expectedResult, bitMask & bitMask2);
        }

        [Fact]
        public void CanOr()
        {
            var bitMask = new BitMask(new[] {1, 2, 3, 4, 10, 16, 17, 20, 30});
            var bitMask2 = new BitMask(new[] {1, 2, 3, 4, 10, 17, 20, 30, 31});
            var expectedResult = new BitMask(new[] {1, 2, 3, 4, 10, 16, 17, 20, 30, 31});

            Assert.Equal(expectedResult, bitMask | bitMask2);
        }

        [Fact]
        public void CanNot()
        {
            var bitMask = new BitMask(new[] {1, 2, 3, 4, 10, 16, 17, 20, 30, 31});
            var expectedResult = new BitMask(new[] {0, 5, 6, 7, 8, 9, 11, 12, 13, 14, 15, 18, 19, 21, 22, 23, 24, 25, 26, 27, 28, 29});

            Assert.Equal(expectedResult, ~bitMask);
        }
    }
}
