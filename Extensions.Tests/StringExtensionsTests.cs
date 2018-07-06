using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Extensions.Tests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [TestCase("1111111111111111111111111111111", 2, ExpectedResult = int.MaxValue)]
        [TestCase("0110111101100001100001010111111", 2, ExpectedResult = 934331071)]
        [TestCase("01101111011001100001010111111", 2, ExpectedResult = 233620159)]
        [TestCase("11101101111011001100001010", 2, ExpectedResult = 62370570)]
        [TestCase("1AeF101", 16, ExpectedResult = 28242177)]
        [TestCase("1ACB67", 16, ExpectedResult = 1756007)]
        [TestCase("764241", 8, ExpectedResult = 256161)]
        [TestCase("10", 5, ExpectedResult = 5)]
        public int ConvertToInt_IsCorrect(string number, int scale)
            => number.ConvertToInt(scale);

        [TestCase("10000000000000000000000000000000", 2)]
        [TestCase("111111111111111111111111111111111", 2)]
        public void ConvertToInt_OverflowValue(string number, int scale)
            => Assert.Throws<OverflowException>(() => number.ConvertToInt(scale));

        [TestCase("764241", 18)]
        [TestCase("11110001010", -1)]
        [TestCase("764241", 0)]
        public void ConvertToInt_BigScale(string number, int scale)
            => Assert.Throws<ArgumentException>(() => number.ConvertToInt(scale));

        [TestCase("1AeF101", 2)]
        [TestCase("SA123", 2)]
        public void ConvertToInt_IncorrectScale(string number, int scale)
            => Assert.Throws<ArgumentException>(() => number.ConvertToInt(scale));
    }
}
