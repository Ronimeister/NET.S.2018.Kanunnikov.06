﻿using System;
using System.Configuration;
using System.Text;

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

        /// <summary>
        /// Provides functionality to clone <see cref="Polynomial"/> object.
        /// </summary>
        /// <returns>Cloned <see cref="Polynomial"/> object.</returns>
        public Polynomial Clone() => new Polynomial(_coefficients);
        #endregion

        #region IEquatable
        /// <summary>
        /// Realization of IQuetable.Equals(). This method checked two <see cref="Polynomial"/> objects for equality.
        /// </summary>
        /// <param name="other">The <see cref="Polynomial"/> object need to be checked.</param>
        /// <returns>Bool value.</returns>
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

        /// <summary>
        /// Public property that return the length of the Polynomial.
        /// </summary>
        public int Length
        {
            get
            {
                return _coefficients.Length;
            }
        }

        /// <summary>
        /// Public property that return the max power of the Polynomial.
        /// </summary>
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

        /// <summary>
        /// Static constructor for Polynomial class that provides functionality to change <see cref="ComparisonEpsilon"/> value.
        /// </summary>
        static Polynomial()
        {
            try
            {
                ComparisonEpsilon =  double.Parse(ConfigurationSettings.AppSettings["EpsilonValue"]);
            }
            catch (Exception e)
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

            private set
            {
                if (indexer < 0 || indexer > Length)
                {
                    throw new ArgumentOutOfRangeException($"{nameof(indexer)} is out of range!");
                }

                _coefficients[indexer] = value; 
            }
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

            return Equals((Polynomial)obj);
        }

        /// <summary>
        /// Overrided version of System.Object.GetHashCode method.
        /// </summary>
        /// <returns>Int value representing hash code.</returns>
        public override int GetHashCode()
        {
            int result = 0;
            foreach(double item in _coefficients)
            {
                result += ShiftAndWrap(item.GetHashCode(), 2);
            }
            return result;
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

            StringBuilder result = new StringBuilder();

            for(int i = Length - 1; i >= 0; i--)
            {
                if (_coefficients[i] != 0)
                {
                    if (i == 0)
                    {
                        result.Append(_coefficients[i]);
                    }
                    else
                    {
                        if (i != 1)
                        {
                            result.Append(_coefficients[i] + "x^" + i + " + ");
                        }
                        else
                        {
                            if (_coefficients[i - 1] != 0 && i - 1 != -1)
                            {
                                result.Append(_coefficients[i] + "x" + " + ");
                            }
                            else
                            {
                                result.Append(_coefficients[i] + "x");
                            }
                        } 
                    }
                }
            }

            return result.ToString();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// This method summing two <see cref="Polynomial"/> objects.
        /// </summary>
        /// <param name="lhs">First object need to be summed.</param>
        /// <param name="rhs">Second object need to be summed.</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="lhs"/> or <paramref name="rhs"/> is equal to null.</exception>
        /// <exception cref="ArgumentException">Throws when <paramref name="lhs"/> or <paramref name="rhs"/> Length property is equal to 0.</exception>
        /// <returns>New <see cref="Polynomial"/> object.</returns>
        public static Polynomial Add(Polynomial lhs, Polynomial rhs)
        {
            CheckInput(lhs, rhs);

            return Sum(lhs, rhs);
        }

        /// <summary>
        /// This method summing <see cref="Polynomial"/> object and <see cref="double"/> value.
        /// </summary>
        /// <param name="lhs"><see cref="Polynomial"/> object need to be summed.</param>
        /// <param name="rhs"><see cref="double"/> value need to be summed.</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="lhs"/> is equal to null.</exception>
        /// <exception cref="ArgumentException">Throws when <paramref name="lhs"/> Length property is equal to 0.</exception> 
        /// <returns>New <see cref="Polynomial"/> object.</returns>
        public static Polynomial Add(Polynomial lhs, double rhs)
        {
            CheckInput(lhs);

            return Sum(lhs, rhs);
        }

        /// <summary>
        /// This method summing <see cref="double"/> value and <see cref="Polynomial"/> object.
        /// </summary>
        /// <param name="lhs"><see cref="double"/> value need to be summed.</param>
        /// <param name="rhs"><see cref="Polynomial"/> object need to be summed.</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="rhs"/> is equal to null.</exception>
        /// <exception cref="ArgumentException">Throws when <paramref name="rhs"/> Length property is equal to 0.</exception> 
        /// <returns>New <see cref="Polynomial"/> object.</returns>
        public static Polynomial Add(double lhs, Polynomial rhs)
        {
            CheckInput(rhs);

            return Sum(lhs, rhs);
        }

        /// <summary>
        /// This method subtracting two <see cref="Polynomial"/> objects.
        /// </summary>
        /// <param name="lhs">First object need to be subract.</param>
        /// <param name="rhs">Second object need to be subract.</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="lhs"/> or <paramref name="rhs"/> is equal to null.</exception>
        /// <exception cref="ArgumentException">Throws when <paramref name="lhs"/> or <paramref name="rhs"/> Length property is equal to 0.</exception>
        /// <returns>New <see cref="Polynomial"/> object.</returns>
        public static Polynomial Subract(Polynomial lhs, Polynomial rhs)
        {
            CheckInput(lhs, rhs);

            return Subtr(lhs, rhs);
        }

        /// <summary>
        /// This method subtracting <see cref="Polynomial"/> object and <see cref="double"/> value.
        /// </summary>
        /// <param name="lhs"><see cref="Polynomial"/> object need to be subract.</param>
        /// <param name="rhs"><see cref="double"/> value need to be subract.</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="lhs"/> is equal to null.</exception>
        /// <exception cref="ArgumentException">Throws when <paramref name="lhs"/> Length property is equal to 0.</exception> 
        /// <returns>New <see cref="Polynomial"/> object.</returns>
        public static Polynomial Subract(Polynomial lhs, double rhs)
        {
            CheckInput(lhs);

            return Subtr(lhs, rhs);
        }

        /// <summary>
        /// This method subtracting <see cref="double"/> value and <see cref="Polynomial"/> object.
        /// </summary>
        /// <param name="lhs"><see cref="double"/> value need to be subract.</param>
        /// <param name="rhs"><see cref="Polynomial"/> object need to be subract.</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="rhs"/> is equal to null.</exception>
        /// <exception cref="ArgumentException">Throws when <paramref name="rhs"/> Length property is equal to 0.</exception> 
        /// <returns>New <see cref="Polynomial"/> object.</returns>
        public static Polynomial Subract(double lhs, Polynomial rhs)
        {
            CheckInput(rhs);

            return Subtr(lhs, rhs);
        }

        /// <summary>
        /// This method multiplying two <see cref="Polynomial"/> objects.
        /// </summary>
        /// <param name="lhs">First object need to be multiplied.</param>
        /// <param name="rhs">Second object need to be multiplied.</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="lhs"/> or <paramref name="rhs"/> is equal to null.</exception>
        /// <exception cref="ArgumentException">Throws when <paramref name="lhs"/> or <paramref name="rhs"/> Length property is equal to 0.</exception>
        /// <returns>New <see cref="Polynomial"/> object.</returns>
        public static Polynomial Multiply(Polynomial lhs, Polynomial rhs)
        {
            CheckInput(lhs, rhs);

            return Mult(lhs, rhs);
        }

        /// <summary>
        /// This method multiplying <see cref="Polynomial"/> object and <see cref="double"/> value.
        /// </summary>
        /// <param name="lhs"><see cref="Polynomial"/> object need to be multiplied.</param>
        /// <param name="rhs"><see cref="double"/> value need to be multiplied.</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="lhs"/> is equal to null.</exception>
        /// <exception cref="ArgumentException">Throws when <paramref name="lhs"/> Length property is equal to 0.</exception> 
        /// <returns>New <see cref="Polynomial"/> object.</returns>
        public static Polynomial Multiply(Polynomial lhs, double rhs)
        {
            CheckInput(lhs);

            return Mult(lhs, rhs);
        }

        /// <summary>
        /// This method multiplying <see cref="double"/> value and <see cref="Polynomial"/> object.
        /// </summary>
        /// <param name="lhs"><see cref="double"/> value need to be multiplied.</param>
        /// <param name="rhs"><see cref="Polynomial"/> object need to be multiplied.</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="rhs"/> is equal to null.</exception>
        /// <exception cref="ArgumentException">Throws when <paramref name="rhs"/> Length property is equal to 0.</exception> 
        /// <returns>New <see cref="Polynomial"/> object.</returns>
        public static Polynomial Multiply(double lhs, Polynomial rhs)
        {
            CheckInput(rhs);

            return Mult(lhs, rhs);
        }

        /// <summary>
        /// This method allows user to get coefficientf of <see cref="Polynomial"/> object as <see cref="double[]"/> array.
        /// </summary>
        /// <returns><see cref="double[]"/> array of coefficients.</returns>
        public double[] ToArray()
        {
            double[] result = new double[Length];
            Array.Copy(_coefficients, result, Length);

            return result;
        }
        #endregion
        #endregion

        #region Private API
        private int ShiftAndWrap(int value, int positions)
        {
            positions = positions & 0x1F;
            
            uint number = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
            uint wrapped = number >> (32 - positions);

            return BitConverter.ToInt32(BitConverter.GetBytes((number << positions) | wrapped), 0);
        }

        private bool IsEqualDoubles(double a, double b) => Math.Abs(a - b) < ComparisonEpsilon;

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
            double[] result = new double[lhs.Length];

            return new Polynomial(GetSummingResult(result, lhs._coefficients, rhs, SummingSign));
        }

        private static Polynomial Sum(double lhs, Polynomial rhs)
        {
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
