using System;
using System.Configuration;

namespace PolynomialLib
{
    /// <summary>
    /// Class that provides functionality to work with polynomials.
    /// </summary>
    public sealed class Polynomial : ICloneable, IEquatable<Polynomial>
    {
        #region Constants
        private const int SummingSign = 1;

        private const int SubtractionSign = -1;

        private const double EpsilonByDefault = 10e-10;
        #endregion

        #region ICloneable methods
        object ICloneable.Clone() => Clone();

        public Polynomial Clone() => new Polynomial(_coefficients);
        #endregion

        #region IEquatable
        public bool Equals(Polynomial other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (this is null || other is null)
            {
                return false;
            }

            if (Power != other.Power)
            {
                return false;
            }

            for (int i = 0; i < other.Length; i++)
            {
                if (!IsEqualDoubles(other[i], _coefficients[i]))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region Public API
        #region Constructor and properties
        private double[] _coefficients;

        private static readonly double ComparisonEpsilon;

        public int Length
        {
            get
            {
                return _coefficients.Length;
            }
        }

        public int Power
        {
            get
            {
                return Length - 1;
            }
        }

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

                _coefficients = new double[coefficients.Length];
                coefficients.CopyTo(_coefficients, 0);
            }

        static Polynomial()
        {
            try
            {
                ComparisonEpsilon =  double.Parse(ConfigurationSettings.AppSettings["EpsilonValue"]);
            }
            catch (ArgumentNullException e)
            {
                ComparisonEpsilon = EpsilonByDefault;
            }
        }

        /// <summary>
        /// Indexer of Polynomial class.
        /// </summary>
        /// <param name="indexer">Current index.</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws when <paramref name="indexer"/> is out of range.</exception>
        /// <returns>Current Polynomial value based on <paramref name="indexer"/> value.</returns>
        public double this[int indexer]
        {
            get
            {
                if (indexer < 0 || indexer > Length)
                {
                    throw new ArgumentOutOfRangeException($"{nameof(indexer)} is out of range!");
                }

                return _coefficients[indexer];
            }

            private set { }
        }
        #endregion

        #region Overloaded static operators
        public static Polynomial operator +(Polynomial lhs, Polynomial rhs) => Add(lhs, rhs);
        public static Polynomial operator +(Polynomial lhs, double rhs) => Add(lhs, rhs);
        public static Polynomial operator +(double lhs, Polynomial rhs) => Add(lhs, rhs);
        public static Polynomial operator -(Polynomial lhs, Polynomial rhs) => Subract(lhs, rhs);
        public static Polynomial operator -(Polynomial lhs, double rhs) => Subract(lhs, rhs);
        public static Polynomial operator -(double lhs, Polynomial rhs) => Subract(lhs, rhs);
        public static Polynomial operator *(Polynomial lhs, Polynomial rhs) => Multiply(lhs, rhs);
        public static Polynomial operator *(Polynomial lhs, double rhs) => Multiply(lhs, rhs);
        public static Polynomial operator *(double lhs, Polynomial rhs) => Multiply(lhs, rhs);
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
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (this is null || obj is null)
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            Polynomial poly = obj as Polynomial;

            if (Power != poly.Power)
            {
                return false;
            }

            for (int i = 0; i < poly.Length; i++)
            {
                if (IsEqualDoubles(poly[i], _coefficients[i]))
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
                        result += _coefficients[i] + "x^" + i + " + ";
                    }
                    else
                    {
                        result += _coefficients[i] + "x" + " + ";
                    }
                }
                else
                {
                    result += _coefficients[i];
                }
            }

            return result;
        }
        #endregion

        #region Public methods
        public static Polynomial Add(Polynomial lhs, Polynomial rhs)
        {
            CheckInput(lhs, rhs);

            return Sum(lhs, rhs);
        }

        public static Polynomial Add(Polynomial lhs, double rhs)
        {
            CheckInput(lhs);

            return Sum(lhs, rhs);
        }

        public static Polynomial Add(double lhs, Polynomial rhs)
        {
            CheckInput(rhs);

            return Sum(lhs, rhs);
        }

        public static Polynomial Subract(Polynomial lhs, Polynomial rhs)
        {
            CheckInput(lhs, rhs);

            return Subtr(lhs, rhs);
        }

        public static Polynomial Subract(Polynomial lhs, double rhs)
        {
            CheckInput(lhs);

            return Subtr(lhs, rhs);
        }

        public static Polynomial Subract(double lhs, Polynomial rhs)
        {
            CheckInput(rhs);

            return Subtr(lhs, rhs);
        }

        public static Polynomial Multiply(Polynomial lhs, Polynomial rhs)
        {
            CheckInput(lhs, rhs);

            return Mult(lhs, rhs);
        }

        public static Polynomial Multiply(Polynomial lhs, double rhs)
        {
            CheckInput(lhs);

            return Mult(lhs, rhs);
        }

        public static Polynomial Multiply(double lhs, Polynomial rhs)
        {
            CheckInput(rhs);

            return Mult(lhs, rhs);
        }

        public double[] ToArray()
        {
            double[] result = new double[Length];
            Array.Copy(_coefficients, result, Length);

            return result;
        }
        #endregion
        #endregion

        #region Private API
        private bool IsEqualDoubles(double a, double b) => Math.Abs(a - b) < ComparisonEpsilon;

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

        private static void CheckInput(Polynomial poly1, Polynomial poly2)
        {
            CheckInput(poly1);
            CheckInput(poly2);
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

        private static double[] GetSummingResult(double[] result, double lhs, double[] rhs, int sign)
        {
            rhs.CopyTo(result, 0);
            result[0] += lhs * sign;
            return result;
        }

        private static double[] GetSummingResult(double[] result, double[] lhs, double rhs, int sign)
        {
            lhs.CopyTo(result, 0);
            result[0] += rhs * sign;
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

        private static double[] GetMultuplyResult(double[] result, double lhs, double[] rhs)
        {
            for(int i = 0; i < rhs.Length; i++)
            {
                result[i] += rhs[i] * lhs;
            }

            return result;
        }

        private static double[] GetMultuplyResult(double[] result, double[] lhs, double rhs)
        {
            for (int i = 0; i < lhs.Length; i++)
            {
                result[i] += lhs[i] * rhs;
            }

            return result;
        }

        private static Polynomial Sum(Polynomial lhs, Polynomial rhs)
        {
            double[] result = MakePolyArr(lhs._coefficients, rhs._coefficients);

            return new Polynomial(GetSummingResult(result, lhs._coefficients, rhs._coefficients, SummingSign));
        }

        private static Polynomial Sum(Polynomial lhs, double rhs)
        {
            CheckInput(lhs);
            double[] result = new double[lhs.Length];

            return new Polynomial(GetSummingResult(result, lhs._coefficients, rhs, SummingSign));
        }

        private static Polynomial Sum(double lhs, Polynomial rhs)
        {
            CheckInput(rhs);
            double[] result = new double[rhs.Length];

            return new Polynomial(GetSummingResult(result, lhs, rhs._coefficients, SummingSign));
        }

        private static Polynomial Subtr(Polynomial lhs, Polynomial rhs)
        {
            double[] result = MakePolyArr(lhs._coefficients, rhs._coefficients);

            return new Polynomial(GetSummingResult(result, lhs._coefficients, rhs._coefficients, SubtractionSign));
        }

        private static Polynomial Subtr(Polynomial lhs, double rhs)
        {
            double[] result = new double[lhs.Length];

            return new Polynomial(GetSummingResult(result, lhs._coefficients, rhs, SubtractionSign));
        }

        private static Polynomial Subtr(double lhs, Polynomial rhs)
        {
            double[] result = new double[rhs.Length];

            return new Polynomial(GetSummingResult(result, lhs, rhs._coefficients, SubtractionSign));
        }

        private static Polynomial Mult(Polynomial lhs, Polynomial rhs)
        {
            double[] result = new double[lhs.Length + rhs.Length - 1];

            return new Polynomial(GetMultuplyResult(result, lhs._coefficients, rhs._coefficients));
        }

        private static Polynomial Mult(double lhs, Polynomial rhs)
        {
            double[] result = new double[rhs.Length];

            return new Polynomial(GetMultuplyResult(result, lhs, rhs._coefficients));
        }

        private static Polynomial Mult(Polynomial lhs, double rhs)
        {
            double[] result = new double[lhs.Length];

            return new Polynomial(GetMultuplyResult(result, lhs._coefficients, rhs));
        }
        #endregion
    }
}