using System.Collections;
using System.Text;

namespace CodingTheory.Math;

public class Matrix : IEnumerable<int>, IEquatable<Matrix>
{
    private int[,] m_values;
    public int Rows { get; init; }
    public int Columns { get; init; }
    public int[,] Values => m_values;

    public Matrix(int[,] values)
    {
        if (values.GetLength(0) == 0 || values.GetLength(1) == 0)
            throw new ArgumentException("Matrix must have at least one row and one column.");
        
        m_values = values;
        Rows = values.GetLength(0);
        Columns = values.GetLength(1);
    }

    public Matrix(int rows, int columns)
    {
        if (rows <= 0 || columns <= 0)
            throw new ArgumentException("Matrix must have at least one row and one column.");
        
        m_values = new int[rows, columns];
        Rows = rows;
        Columns = columns;
    }

    public int this[int row, int col]
    {
        get => m_values[row, col];
        set => m_values[row, col] = value;
    }

    public Vector GetRow(int rowIndex)
    {
        if (rowIndex < 0 || rowIndex >= Rows)
            throw new ArgumentOutOfRangeException(nameof(rowIndex), "Row index is out of range.");
        
        int[] rowValues = new int[Columns];
        for (int j = 0; j < Columns; ++j)
            rowValues[j] = m_values[rowIndex, j];
        
        return new Vector(rowValues);
    }

    public Vector GetColumn(int columnIndex)
    {
        if (columnIndex < 0 || columnIndex >= Columns)
            throw new ArgumentOutOfRangeException(nameof(columnIndex), "Column index is out of range.");
        
        int[] columnValues = new int[Rows];
        for (int i = 0; i < Rows; ++i)
            columnValues[i] = m_values[i, columnIndex];
        
        return new Vector(columnValues);
    }

    public IEnumerable<Vector> GetRows()
    {
        for (int i = 0; i < Rows; ++i)
            yield return GetRow(i);
    }

    public IEnumerator<int> GetEnumerator()
    {
        for (int i = 0; i < Rows; ++i)
            foreach (int value in GetRow(i))
                yield return value;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static Vector operator *(Matrix m, Vector v)
    {
        if (m.Columns != v.Length)
            throw new ArgumentException("Matrix columns must match vector length for multiplication.");

        Matrix vMatrix = v.ToColumnMatrix();
        return (m * vMatrix).GetColumn(0);
    }

    public static Vector operator *(Vector v, Matrix m)
    {
        if (v.Length != m.Rows)
            throw new ArgumentException("Vector length must match matrix rows for multiplication.");

        Matrix vMatrix = v.ToRowMatrix();
        return (vMatrix * m).GetRow(0);
    }

    public static Matrix operator *(Matrix m1, Matrix m2)
    {
        if (m1.Columns != m2.Rows)
            throw new ArgumentException("First matrix columns must match second matrix rows for multiplication.");
        
        int[,] resultValues = new int[m1.Rows, m2.Columns];
        for (int i = 0; i < m1.Rows; ++i)
        {
            for (int j = 0; j < m2.Columns; ++j)
            {
                int sum = 0;
                for (int k = 0; k < m1.Columns; ++k)
                    sum += m1[i, k] * m2[k, j];
                
                resultValues[i, j] = sum;
            }
        }

        return new Matrix(resultValues);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Matrix other || other.Rows != Rows || other.Columns != Columns)
            return false;

        return Equals(other);
    }

    public bool Equals(Matrix? other)
    {
        if (other is null)
            return false;

        if (Object.ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        if (other.Rows != Rows || other.Columns != Columns)
            return false;

        for (int i = 0; i < Rows; ++i)
            for (int j = 0; j < Columns; ++j)
                if (m_values[i, j] != other[i, j])
                    return false;

        return true;
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        foreach (int value in this)
            hash.Add(value);

        return hash.ToHashCode();
    }

    public static bool operator ==(Matrix m1, Matrix m2) => Equals(m1, m2);
    public static bool operator !=(Matrix m1, Matrix m2) => !Equals(m1, m2);

    public static Matrix KroneckerProduct(Matrix m1, Matrix m2)
    {
        Matrix kronecker = new Matrix(m1.Rows * m2.Rows, m1.Columns * m2.Columns);

        for (int r1 = 0; r1 < m1.Rows; ++r1)
            for (int c1 = 0; c1 < m1.Columns; ++c1)
                for (int r2 = 0; r2 < m2.Rows; ++r2)
                    for (int c2 = 0; c2 < m2.Columns; ++c2)
                        kronecker[r1 * m2.Rows + r2, c1 * m2.Columns + c2] = m1[r1, c1] * m2[r2, c2];

        return kronecker;
    }

    public static Matrix IdentityMatrix(int size)
    {
        Matrix identityMatrix = new Matrix(size, size);

        int currentIndex = 0;
        while (currentIndex < size)
        {
            identityMatrix[currentIndex, currentIndex] = 1;
            ++currentIndex;
        }

        return identityMatrix;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        foreach (Vector row in GetRows())
        {
            foreach (int value in row)
                sb.Append($"{value} ");
            sb.Append('\n');
        }

        return sb.ToString();
    }
}
