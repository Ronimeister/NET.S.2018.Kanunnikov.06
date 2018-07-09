using System;
using NUnit.Framework;

namespace PolynomialLib.Tests
{
    [TestFixture]
    public class PolynomialTests
    {
        #region Overloaded operations
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
        public void Polynomial_SummPolyAndDouble_IsCorrect()
        {
            Polynomial lhs = new Polynomial(1, 2, 3);
            double rhs = 2;
            Polynomial expected = new Polynomial(3, 2, 3);

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
        public void Polynomial_SubtrPolyAndDouble_IsCorrect()
        {
            Polynomial lhs = new Polynomial(1, 2, 3);
            double rhs = 2;
            Polynomial expected = new Polynomial(-1, 2, 3);

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
        public void Polynomial_MultPolyAndDouble_IsCorrect()
        {
            Polynomial lhs = new Polynomial(1, 2, 3);
            double rhs = 2;
            Polynomial expected = new Polynomial(2, 4, 6);

            Polynomial result = lhs * rhs;
            Assert.AreEqual(expected, result);
        }

        [TestCase]
        public void Polynomial_EqualOperator_IsCorrect()
        {
            Polynomial poly1 = new Polynomial(1, 2);
            Polynomial poly2 = poly1;

            bool expected = true;
            bool actual = (poly1 == poly2);
            Assert.AreEqual(expected, actual);
        }
        
        [TestCase]
        public void Polynomial_NotEqualOperator_IsCorrect()
        {
            Polynomial poly1 = new Polynomial(1, 2);
            Polynomial poly2 = poly1;

            bool expected = false;
            bool actual = (poly1 != poly2);
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region System.Object overrided methods
        [TestCase]
        public void Polynomial_ToString_IsCorrect()
        {
            Polynomial test = new Polynomial(1, 2, 3);
            string expected = "3x^2 + 2x + 1";

            string result = test.ToString();
            Assert.AreEqual(expected, result);
        }

        [TestCase]
        public void Polynomial_ToString_IsNotCorrect()
        {
            Polynomial test = new Polynomial(1, 2, 3);
            string expected = "9x^2 + 5x";

            string result = test.ToString();
            Assert.AreNotEqual(expected, result);
        }

        [TestCase]
        public void Polynomial_ToString_NullReferenceException()
        {
            Polynomial test = null;
            Assert.Throws<NullReferenceException>(() => test.ToString());
        }

        [TestCase]
        public void Polynomial_Equals_IsEqual()
        {
            Polynomial lhs = new Polynomial(1, 2, 3);
            bool expected = true;

            bool result = lhs.Equals(lhs);
            Assert.AreEqual(expected, result);
        }
        
        [TestCase]
        public void Polynomial_EqualsObject_IsCorrect()
        {
            Polynomial poly1 = new Polynomial(1, 2);
            object poly2 = new Polynomial(1, 2);

            bool expected = true;
            bool actual = poly1.Equals(poly2);
            Assert.AreEqual(expected, actual);
        }

        [TestCase]
        public void Polynomial_Equals_IsNotEqual()
        {
            Polynomial lhs = new Polynomial(1, 2, 3);
            Polynomial rhs = new Polynomial(2, 3, 4);
            bool expected = false;

            bool result = lhs.Equals(rhs);
            Assert.AreEqual(expected, result);
        }
        #endregion

        #region Constructor tests
        [TestCase]
        public void PolynomialConstructor_NullArg_ArgumentNullException()
            => Assert.Throws<ArgumentNullException>(() => new Polynomial(null));

        [TestCase]
        public void PolynomialConstructor_EmptyArg_ArgumentException()
            => Assert.Throws<ArgumentException>(() => new Polynomial(new double[] { }));

        [TestCase]
        public void PolynomialIndexer_IsCorrect()
        {
            Polynomial poly = new Polynomial(new double[] { 1, 2, 3 });
            double expected = 2;

            double actual = poly[1];
            Assert.AreEqual(expected, actual);
        }

        [TestCase]
        public void PolynomialIndexer_ArgumentOutOfRangeException()
        {
            Polynomial poly = new Polynomial(new double[] { 1, 2, 3 });
            double temp = 0;

            Assert.Throws<ArgumentOutOfRangeException>(() => temp = poly[10]);
        }
        #endregion
    }
}