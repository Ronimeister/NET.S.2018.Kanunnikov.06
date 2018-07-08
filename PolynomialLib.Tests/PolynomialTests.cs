using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace PolynomialLib.Tests
{
    [TestFixture]
    public class PolynomialTests
    {
        [TestCase]
        public void Polynomial_Summ_IsCorrect()
        {
            Polynomial lhs = new Polynomial(1, 2, 3);
            Polynomial rhs = new Polynomial(2, 3, 4);
            Polynomial expected = new Polynomial(3, 5, 7);

            Polynomial result = lhs + rhs;
            Assert.AreEqual(expected, result);
        }

        [TestCase]
        public void Polynomial_Subtr_IsCorrect()
        {
            Polynomial lhs = new Polynomial(1, 3, 7);
            Polynomial rhs = new Polynomial(2, 2, 4);
            Polynomial expected = new Polynomial(-1, 1, 3);

            Polynomial result = lhs - rhs;
            Assert.AreEqual(expected, result);
        }

        [TestCase]
        public void Polynomial_Mult_IsCorrect()
        {
            Polynomial lhs = new Polynomial(1, 2, 3);
            Polynomial rhs = new Polynomial(2, 3, 4);
            Polynomial expected = new Polynomial(2, 7, 16, 17, 12);

            Polynomial result = lhs * rhs;
            Assert.AreEqual(expected, result);
        }

        [TestCase]
        public void Polynomial_ToString_IsCorrect()
        {
            Polynomial test = new Polynomial(1, 2, 3);
            string expected = "3x^2 + 2x + 1";

            string result = test.ToString();
            Assert.AreEqual(expected, result);
        }
    }
}
