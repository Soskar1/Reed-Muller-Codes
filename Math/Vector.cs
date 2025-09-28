using System.Collections;
using System.Text;

namespace CodingTheory.Math;

public class Vector : IEnumerable<int>
{
    private int[] m_values;
    public int Length => m_values.Length;

    public Vector(int[] values)
    {
        if (values.Length == 0)
            throw new ArgumentException("Vector must have at least one element.");

        m_values = values;
    }

    public Vector(int size)
    {
        if (size <= 0)
            throw new ArgumentException("Vector size must be positive.");

        m_values = new int[size];
    }

    public Matrix ToColumnMatrix()
    {
        int[,] matrixValues = new int[Length, 1];

        for (int i = 0; i < Length; ++i)
            matrixValues[i, 0] = m_values[i];
        
        return new Matrix(matrixValues);
    }

    public Matrix ToRowMatrix()
    {
        int[,] matrixValues = new int[1, Length];

        for (int j = 0; j < Length; ++j)
            matrixValues[0, j] = m_values[j];

        return new Matrix(matrixValues);
    }

    public IEnumerator<int> GetEnumerator()
    {
        foreach (var value in m_values)
            yield return value;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int this[int index]
    {
        get => m_values[index];
        set => m_values[index] = value;
    }

    public static Vector operator +(Vector v1, Vector v2)
    {
        if (v1.Length != v2.Length)
            throw new ArgumentException("Vectors must be of the same length to add.");

        int[] resultValues = new int[v1.Length];

        for (int i = 0; i < v1.Length; ++i)
            resultValues[i] = v1[i] + v2[i];

        return new Vector(resultValues);
    }

    public static Vector operator *(Vector v, int scalar)
    {
        int[] resultValues = new int[v.Length];

        for (int i = 0; i < v.Length; ++i)
            resultValues[i] = v[i] * scalar;

        return new Vector(resultValues);
    }

    public static Vector operator *(int scalar, Vector v) => v * scalar;

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        foreach (int value in this)
            sb.Append(value);

        return sb.ToString();
    }

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

    public static Vector ByteToVector(byte value)
    {
        int[] bits = new int[8];
        for (int i = 0; i < 8; i++)
            bits[i] = (value >> (7 - i)) & 1;

        return new Vector(bits);
    }

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
