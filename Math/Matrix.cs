using System.Collections;
using System.Text;

namespace CodingTheory.Math;

public class Matrix : IEnumerable<int>, IEquatable<Matrix>
{
    private int[,] m_values;
    public int Rows { get; init; }
    public int Columns { get; init; }
    public int[,] Values => m_values;

    /// <summary>
    /// Matrix constructor using initial values
    /// </summary>
    /// <param name="values">Initial matrix values</param>
    public Matrix(int[,] values)
    {
        if (values.GetLength(0) == 0 || values.GetLength(1) == 0)
            throw new ArgumentException("Matrix must have at least one row and one column.");
        
        m_values = values;
        Rows = values.GetLength(0);
        Columns = values.GetLength(1);
    }

    /// <summary>
    /// Zero matrix constructor with given dimensions
    /// </summary>
    /// <param name="rows">Row count</param>
    /// <param name="columns">Column count</param>
    public Matrix(int rows, int columns)
    {
        if (rows <= 0 || columns <= 0)
            throw new ArgumentException("Matrix must have at least one row and one column.");
        
        m_values = new int[rows, columns];
        Rows = rows;
        Columns = columns;
    }

    /// <summary>
    /// Indexing operator
    /// </summary>
    /// <param name="row">Row index (from 0 to "row count - 1")</param>
    /// <param name="col">Column index (from 0 to "column count - 1")</param>
    /// <returns>Matrix value</returns>
    public int this[int row, int col]
    {
        get => m_values[row, col];
        set => m_values[row, col] = value;
    }

    /// <summary>
    /// Get row vector
    /// </summary>
    /// <param name="rowIndex">Row index</param>
    /// <returns>Row vector</returns>
    public Vector GetRow(int rowIndex)
    {
        if (rowIndex < 0 || rowIndex >= Rows)
            throw new ArgumentOutOfRangeException(nameof(rowIndex), "Row index is out of range.");
        
        int[] rowValues = new int[Columns];
        for (int j = 0; j < Columns; ++j)
            rowValues[j] = m_values[rowIndex, j];
        
        return new Vector(rowValues);
    }

    /// <summary>
    /// Get column vector
    /// </summary>
    /// <param name="columnIndex">Column index</param>
    /// <returns>Column vector</returns>
    public Vector GetColumn(int columnIndex)
    {
        if (columnIndex < 0 || columnIndex >= Columns)
            throw new ArgumentOutOfRangeException(nameof(columnIndex), "Column index is out of range.");
        
        int[] columnValues = new int[Rows];
        for (int i = 0; i < Rows; ++i)
            columnValues[i] = m_values[i, columnIndex];
        
        return new Vector(columnValues);
    }

    /// <summary>
    /// Row enumeration
    /// </summary>
    /// <returns>Returns each row starting from 0 to "row count - 1"</returns>
    public IEnumerable<Vector> GetRows()
    {
        for (int i = 0; i < Rows; ++i)
            yield return GetRow(i);
    }

    /// <summary>
    /// Matrix value enumerations from first row, first column and ends at last row, last column.
    /// </summary>
    /// <returns>Returns all values in the first row, then moves to the second row, etc.</returns>
    public IEnumerator<int> GetEnumerator()
    {
        for (int i = 0; i < Rows; ++i)
            foreach (int value in GetRow(i))
                yield return value;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Matrix and vector multiplication
    /// </summary>
    /// <param name="m">Matrix (z x y)</param>
    /// <param name="v">Vector with y elements</param>
    /// <returns>Vector</returns>
    /// <exception cref="ArgumentException"></exception>
    public static Vector operator *(Matrix m, Vector v)
    {
        if (m.Columns != v.Length)
            throw new ArgumentException("Matrix columns must match vector length for multiplication.");

        // Convert vector to a column matrix (y x 1)
        Matrix vMatrix = v.ToColumnMatrix();

        // (z x y) * (y x 1) = (z x 1). Getting column vector (see Matrix.GetColumn)
        return (m * vMatrix).GetColumn(0);
    }

    /// <summary>
    /// Vector and matrix multiplication
    /// </summary>
    /// <param name="v">Vector with y elements</param>
    /// <param name="m">Matrix (y x z)</param>
    /// <returns>Vector</returns>
    /// <exception cref="ArgumentException"></exception>
    public static Vector operator *(Vector v, Matrix m)
    {
        if (v.Length != m.Rows)
            throw new ArgumentException("Vector length must match matrix rows for multiplication.");

        // Convert vector to a row matrix (1 x y)
        Matrix vMatrix = v.ToRowMatrix();

        // (1 x y) * (y x z) = (1 x z). Getting row vector (see Matrix.GetRow)
        return (vMatrix * m).GetRow(0);
    }

    /// <summary>
    /// Matrix multiplication
    /// </summary>
    /// <param name="m1">Matrix (a x b)</param>
    /// <param name="m2">Matric (b x c)</param>
    /// <returns>Matrix (a x c)</returns>
    /// <exception cref="ArgumentException"></exception>
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

    /// <summary>
    /// Matrix comparison by value
    /// </summary>
    /// <param name="obj">Any object. Can be anything, but it need to be a Matrix to pass through all comparison logic</param>
    /// <returns>Returns False if obj is not a Matrix. Returns False if obj is a Matrix but if the provided matrix is not the same.
    /// Return True if the provided matrix is the same</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not Matrix other || other.Rows != Rows || other.Columns != Columns)
            return false;

        return Equals(other);
    }

    /// <summary>
    /// Matrix comparison by value
    /// </summary>
    /// <param name="other">Any matrix</param>
    /// <returns>Returns False if the provided matrix is not the same.
    /// Returns True if the provided matrix is the same</returns>
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

    /// <summary>
    /// Matrix equality operator implementation
    /// </summary>
    /// <param name="m1">Any matrix</param>
    /// <param name="m2">Any matrix</param>
    /// <returns>Returns False if two matrices are not the same by value comparison.
    /// Returns True if two matrices are the same by value comparison</returns>
    public static bool operator ==(Matrix m1, Matrix m2) => Equals(m1, m2);

    /// <summary>
    /// Matrix inequality operator implementation
    /// </summary>
    /// <param name="m1">Any matrix</param>
    /// <param name="m2">Any matrix</param>
    /// <returns>Returns False if two matrices are the same by value comparison.
    /// Returns True if two matrices are not hte same by value comparison</returns>
    public static bool operator !=(Matrix m1, Matrix m2) => !Equals(m1, m2);

    /// <summary>
    /// Kronecker product using two matrices
    /// </summary>
    /// <param name="m1">Any matrix</param>
    /// <param name="m2">Any matrix</param>
    /// <returns>Matrix</returns>
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

    /// <summary>
    /// Identity matrix creation using size parameter
    /// </summary>
    /// <param name="size">Size of the identity matrix</param>
    /// <returns>Identity matrix (size x size)</returns>
    public static Matrix IdentityMatrix(int size)
    {
        Matrix identityMatrix = new Matrix(size, size);

        int currentIndex = 0;
        
        // Inserting 1 into matrix diagonal
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
