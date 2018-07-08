using System;

namespace PolynomialLib
{
    /// <summary>
    /// Class that provides functionality to work with polynomials.
    /// </summary>
    public sealed class Polynomial
    {
        #region Constants
        private const int SummingSign = 1;

        private const int SubtractionSign = -1;

        private const double ComparisonEpsilon = 10e-10;
        #endregion

        #region Public API
        #region Constructor and properties
        /// <summary>
        /// Constructor for Polynomial class.
        /// </summary>
        /// <param name="coefficients">Parametrs of the Polynomial.</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="coefficients"/> is equal to null.</exception>
        /// <exception cref="ArgumentException">Throws when <paramref name="coefficients"/> is empty.</exception>
        public Polynomial(params double[] coefficients)
            {
                if (coefficients == null)
                {
                    throw new ArgumentNullException($"{nameof(coefficients)} can't be equal to null.");
                }

                if (coefficients.Length == 0)
                {
                    throw new ArgumentException($"{nameof(coefficients)} can't be empty!");
                }

                Coefficients = new double[coefficients.Length];
                coefficients.CopyTo(Coefficients, 0);
            }

        public int Length
        {
            get
            {
                return Coefficients.Length;
            }
        }

        public double[] Coefficients { get; } = { };

        /// <summary>
        /// Indexer of Polynomial class.
        /// </summary>
        /// <param name="indexer">Current index.</param>
        /// <exception cref="IndexOutOfRangeException">Throws when <paramref name="indexer"/> is out of range.</exception>
        /// <returns>Current Polynomial value based on <paramref name="indexer"/> value.</returns>
        public double this[int indexer]
        {
            get
            {
                if (indexer < 0 || indexer > Coefficients.Length)
                {
                    throw new IndexOutOfRangeException($"{nameof(indexer)} is out of range!");
                }

                return Coefficients[indexer];
            }
        }
        #endregion

        #region Overloaded static operators
        public static Polynomial operator +(Polynomial lhs, Polynomial rhs) => Sum(lhs, rhs);
        public static Polynomial operator +(Polynomial lhs, double[] rhs) => Sum(lhs, rhs);
        public static Polynomial operator +(double[] lhs, Polynomial rhs) => Sum(lhs, rhs);
        public static Polynomial operator -(Polynomial lhs, Polynomial rhs) => Subtr(lhs, rhs);
        public static Polynomial operator -(Polynomial lhs, double[] rhs) => Subtr(lhs, rhs);
        public static Polynomial operator -(double[] lhs, Polynomial rhs) => Subtr(lhs, rhs);
        public static Polynomial operator *(Polynomial lhs, Polynomial rhs) => Mult(lhs, rhs);
        public static Polynomial operator *(Polynomial lhs, double[] rhs) => Mult(lhs, rhs);
        public static Polynomial operator *(double[] lhs, Polynomial rhs) => Mult(lhs, rhs);
        public static bool operator ==(Polynomial lhs, Polynomial rhs) => Equals(lhs, rhs);
        public static bool operator !=(Polynomial lhs, Polynomial rhs) => !Equals(lhs, rhs);
        #endregion

        #region System.Object overrided methods
        /// <summary>
        /// Overrided version of System.Object.Equals method.
        /// </summary>
        /// <param name="obj">Object which should be checked for equality.</param>
        /// <returns>Bool value.</returns>
        public override bool Equals(object obj)
        {
            Polynomial poly = obj as Polynomial;
            CheckInput(poly);

            if (Length != poly.Length)
            {
                return false;
            }

            for (int i = 0; i < poly.Length; i++)
            {
                if (!poly.Coefficients[i].Equals(Coefficients[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Overrided version of System.Object.GetHashCode method.
        /// </summary>
        /// <returns>Int value representing hash code.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Overrided version of System.Object.ToString method.
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws when object is equal to null.</exception>
        /// <returns>String representing polynomial in special format.</returns>
        public override string ToString()
        {
            if (this == null)
            {
                throw new ArgumentNullException($"Object can't be equal to null!");
            }

            string result = string.Empty;
            for(int i = Length - 1; i >= 0; i--)
            {
                if (i != 0)
                {
                    if (i != 1)
                    {
                        result += Coefficients[i] + "x^" + i + " + ";
                    }
                    else
                    {
                        result += Coefficients[i] + "x" + " + ";
                    }
                }
                else
                {
                    result += Coefficients[i];
                }
            }

            return result;
        }
        #endregion
        #endregion

        #region Private API
        private bool IsEqualDouble(double a, double b) => Math.Abs(a - b) < ComparisonEpsilon;

        /// <summary>
        /// Check input Polynomial object for exceptions.
        /// </summary>
        /// <param name="poly">input Polynomial object</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="poly"/> is equal to null.</exception>
        /// <exception cref="ArgumentException">Throws when <paramref name="poly"/> Length property is equal to 0.</exception>
        private static void CheckInput(Polynomial poly)
        {
            if (poly == null)
            {
                throw new ArgumentNullException($"{nameof(poly)} is null!");
            }

            if (poly.Length == 0)
            {
                throw new ArgumentException($"{nameof(poly)} is empty!");
            }
        }

        /// <summary>
        /// Overloaded version of <see cref="CheckInput(Polynomial)"/> method that takes two Polynomial objects.
        /// </summary>
        /// <param name="poly1">input Polynomial object</param>
        /// <param name="poly2">input Polynomial object</param>
        private static void CheckInput(Polynomial poly1, Polynomial poly2)
        {
            CheckInput(poly1);
            CheckInput(poly2);
        }

        /// <summary>
        /// Overloaded version of <see cref="CheckInput(Polynomial)"/> method that takes two double[] arrays.
        /// </summary>
        /// <param name="lhs">input double[] array.</param>
        /// <param name="rhs">input double[] array.</param>
        /// <exception cref="ArgumentException">Throws when <paramref name="lhs"/> or <paramref name="rhs"/> <see cref="Length"/> is equal to 0.</exception>
        private static void CheckInput(double[] lhs, double[] rhs)
        {
            if (lhs.Length == 0 || rhs.Length == 0)
            {
                throw new ArgumentException($"Coefficients array is empty!");
            }
        }

        /// <summary>
        /// Create double[] array based on <paramref name="lhs"/> and <paramref name="rhs"/> based on their <see cref="Length"/> property.
        /// </summary>
        /// <param name="lhs">input double[] array.</param>
        /// <param name="rhs">input double[] array.</param>
        /// <returns>New double[] array.</returns>
        private static double[] MakePolyArr(double[] lhs, double[] rhs)
            => new double[(lhs.Length > rhs.Length) ? lhs.Length : rhs.Length];

        private static double[] GetSummingResult(double[] result, double[] lhs, double[] rhs, int sign)
        {
            lhs.CopyTo(result, 0);

            for (int i = 0; i < rhs.Length; i++)
            {
                result[i] += rhs[i] * sign;
            }

            return result;
        }

        private static double[] GetMultuplyResult(double[] result, double[] lhs, double[] rhs)
        {
            for (int i = 0; i < lhs.Length; ++i)
            {
                for(int j = 0; j < rhs.Length; ++j)
                {
                    result[i + j] += lhs[i] * rhs[j];
                }
            }

            return result;
        }

        private static Polynomial Sum(Polynomial lhs, Polynomial rhs)
        {
            CheckInput(lhs, rhs);
            double[] result = MakePolyArr(lhs.Coefficients, rhs.Coefficients);

            return new Polynomial(GetSummingResult(result, lhs.Coefficients, rhs.Coefficients, SummingSign));
        }

        private static Polynomial Sum(Polynomial lhs, double[] rhs)
        {
            CheckInput(lhs.Coefficients, rhs);
            double[] result = MakePolyArr(lhs.Coefficients, rhs);

            return new Polynomial(GetSummingResult(result, lhs.Coefficients, rhs, SummingSign));
        }

        private static Polynomial Sum(double[] lhs, Polynomial rhs)
        {
            CheckInput(lhs, rhs.Coefficients);
            double[] result = MakePolyArr(lhs, rhs.Coefficients);

            return new Polynomial(GetSummingResult(result, lhs, rhs.Coefficients, SummingSign));
        }

        private static Polynomial Subtr(Polynomial lhs, Polynomial rhs)
        {
            CheckInput(lhs, rhs);
            double[] result = MakePolyArr(lhs.Coefficients, rhs.Coefficients);

            return new Polynomial(GetSummingResult(result, lhs.Coefficients, rhs.Coefficients, SubtractionSign));
        }

        private static Polynomial Subtr(Polynomial lhs, double[] rhs)
        {
            CheckInput(lhs.Coefficients, rhs);
            double[] result = MakePolyArr(lhs.Coefficients, rhs);

            return new Polynomial(GetSummingResult(result, lhs.Coefficients, rhs, SubtractionSign));
        }

        private static Polynomial Subtr(double[] lhs, Polynomial rhs)
        {
            CheckInput(lhs, rhs.Coefficients);
            double[] result = MakePolyArr(lhs, rhs.Coefficients);

            return new Polynomial(GetSummingResult(result, lhs, rhs.Coefficients, SubtractionSign));
        }

        private static Polynomial Mult(Polynomial lhs, Polynomial rhs)
        {
            CheckInput(lhs, rhs);
            double[] result = new double[lhs.Length + rhs.Length - 1];

            return new Polynomial(GetMultuplyResult(result, lhs.Coefficients, rhs.Coefficients));
        }

        private static Polynomial Mult(Polynomial lhs, double[] rhs)
        {
            CheckInput(lhs.Coefficients, rhs);
            double[] result = new double[lhs.Length + rhs.Length - 1];

            return new Polynomial(GetMultuplyResult(result, lhs.Coefficients, rhs));
        }

        private static Polynomial Mult(double[] lhs, Polynomial rhs)
        {
            CheckInput(lhs, rhs.Coefficients);
            double[] result = new double[lhs.Length + rhs.Length - 1];

            return new Polynomial(GetMultuplyResult(result, lhs, rhs.Coefficients));
        }
        #endregion
    }
}