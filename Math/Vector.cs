using System.Collections;
using System.Text;

namespace CodingTheory.Math;

public class Vector : IEnumerable<int>
{
    private int[] m_values;
    public int Length => m_values.Length;

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector"/> class using the specified values.
    /// </summary>
    /// <param name="values">An array of integers representing the vector elements.</param>
    /// <exception cref="ArgumentException">Thrown when the array is empty.</exception>
    public Vector(int[] values)
    {
        if (values.Length == 0)
            throw new ArgumentException("Vector must have at least one element.");

        m_values = values;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector"/> class with the specified size.
    /// </summary>
    /// <param name="size">The number of elements in the vector. Must be positive.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="size"/> is less than or equal to zero.</exception>
    public Vector(int size)
    {
        if (size <= 0)
            throw new ArgumentException("Vector size must be positive.");

        m_values = new int[size];
    }

    /// <summary>
    /// Converts the vector into a column matrix.
    /// </summary>
    /// <returns>
    /// A <see cref="Matrix"/> instance with the same elements as this vector,
    /// arranged as a single column.
    /// </returns>
    public Matrix ToColumnMatrix()
    {
        int[,] matrixValues = new int[Length, 1];

        for (int i = 0; i < Length; ++i)
            matrixValues[i, 0] = m_values[i];
        
        return new Matrix(matrixValues);
    }

    /// <summary>
    /// Converts the vector into a row matrix.
    /// </summary>
    /// <returns>
    /// A <see cref="Matrix"/> instance with the same elements as this vector,
    /// arranged as a single row.
    /// </returns>
    public Matrix ToRowMatrix()
    {
        int[,] matrixValues = new int[1, Length];

        for (int j = 0; j < Length; ++j)
            matrixValues[0, j] = m_values[j];

        return new Matrix(matrixValues);
    }

    /// <summary>
    /// Returns an enumerator that iterates through the vector elements.
    /// </summary>
    /// <returns>An enumerator for the vector’s elements.</returns>
    public IEnumerator<int> GetEnumerator()
    {
        foreach (var value in m_values)
            yield return value;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Gets or sets the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get or set.</param>
    /// <returns>The value of the element at the specified index.</returns>
    public int this[int index]
    {
        get => m_values[index];
        set => m_values[index] = value;
    }

    /// <summary>
    /// Adds two vectors element-wise.
    /// </summary>
    /// <param name="v1">The first vector operand.</param>
    /// <param name="v2">The second vector operand.</param>
    /// <returns>
    /// A new <see cref="Vector"/> whose elements are the sum of the corresponding
    /// elements in <paramref name="v1"/> and <paramref name="v2"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the vectors have different lengths.
    /// </exception>
    public static Vector operator +(Vector v1, Vector v2)
    {
        if (v1.Length != v2.Length)
            throw new ArgumentException("Vectors must be of the same length to add.");

        int[] resultValues = new int[v1.Length];

        for (int i = 0; i < v1.Length; ++i)
            resultValues[i] = v1[i] + v2[i];

        return new Vector(resultValues);
    }

    /// <summary>
    /// Multiplies a vector by a scalar value.
    /// </summary>
    /// <param name="v">The vector to be scaled.</param>
    /// <param name="scalar">The scalar multiplier.</param>
    /// <returns>
    /// A new <see cref="Vector"/> where each element is the product of
    /// the corresponding element in <paramref name="v"/> and <paramref name="scalar"/>.
    /// </returns>
    public static Vector operator *(Vector v, int scalar)
    {
        int[] resultValues = new int[v.Length];

        for (int i = 0; i < v.Length; ++i)
            resultValues[i] = v[i] * scalar;

        return new Vector(resultValues);
    }

    /// <summary>
    /// Multiplies a scalar by a vector (commutative with <see cref="operator *(Vector, int)"/>).
    /// </summary>
    /// <param name="scalar">The scalar multiplier.</param>
    /// <param name="v">The vector to be scaled.</param>
    /// <returns>
    /// A new <see cref="Vector"/> where each element is the product of
    /// the corresponding element in <paramref name="v"/> and <paramref name="scalar"/>.
    /// </returns>
    public static Vector operator *(int scalar, Vector v) => v * scalar;

    /// <summary>
    /// Returns a string representation of the vector by concatenating its elements.
    /// </summary>
    /// <returns>A string consisting of the vector elements in sequence.</returns>
    /// <remarks>
    /// No delimiters are used between elements. For example, a vector with elements
    /// <c>[1, 0, 1]</c> will produce the string <c>"101"</c>.
    /// </remarks>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        foreach (int value in this)
            sb.Append(value);

        return sb.ToString();
    }

    /// <summary>
    /// Attempts to parse a string representation of a numeric vector into a <see cref="Vector"/> instance.
    /// </summary>
    /// <param name="value">A string containing digits representing vector elements.</param>
    /// <param name="v">When this method returns, contains the resulting <see cref="Vector"/>, if parsing succeeded.</param>
    /// <returns>
    /// <see langword="true"/> if parsing succeeded; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// Each character in the input string must be a digit (0–9). 
    /// For example, passing <c>"1011"</c> produces a vector <c>[1, 0, 1, 1]</c>.
    /// </remarks>
    public static bool TryParse(string value, out Vector v)
    {
        v = new Vector(value.Length);

        for (int i = 0; i < value.Length; ++i)
        {
            char num = value[i];
            if (!Char.IsDigit(num))
                return false;

            int intValue = (int)Char.GetNumericValue(num);
            v[i] = intValue;
        }

        return true;
    }

    /// <summary>
    /// Converts an 8-bit unsigned integer into a vector of its binary representation.
    /// </summary>
    /// <param name="value">The byte value to convert.</param>
    /// <returns>
    /// A <see cref="Vector"/> of length 8 representing the binary digits of <paramref name="value"/>,
    /// with the most significant bit (MSB) first.
    /// </returns>
    public static Vector ByteToVector(byte value)
    {
        int[] bits = new int[8];
        for (int i = 0; i < 8; i++)
            bits[i] = (value >> (7 - i)) & 1;

        return new Vector(bits);
    }

    /// <summary>
    /// Converts a <see cref="Vector"/> into its corresponding byte value.
    /// </summary>
    /// <param name="v">The vector to convert.</param>
    /// <returns>A byte representing the binary value encoded by the vector.</returns>
    /// <exception cref="InvalidCastException">
    /// Thrown if the vector contains more than 8 elements.
    /// </exception>
    /// <remarks>
    /// The first element in the vector is treated as the most significant bit (MSB).
    /// </remarks>
    public static explicit operator byte(Vector v)
    {
        if (v.Length > 8)
            throw new InvalidCastException();

        byte value = 0;
        for (int i = 0; i < v.Length; ++i)
            value = (byte)((value << 1) | v[i]);

        return value;
    }
}
