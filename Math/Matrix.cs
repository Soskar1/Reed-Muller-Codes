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
    /// Initializes a new instance of the <see cref="Matrix"/> class with the specified two-dimensional array of values.
    /// </summary>
    /// <param name="values">A two-dimensional array representing the elements of the matrix. The array must have at least one row and one
    /// column.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="values"/> has zero rows or zero columns.</exception>
    public Matrix(int[,] values)
    {
        if (values.GetLength(0) == 0 || values.GetLength(1) == 0)
            throw new ArgumentException("Matrix must have at least one row and one column.");
        
        m_values = values;
        Rows = values.GetLength(0);
        Columns = values.GetLength(1);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Matrix"/> class with the specified number of rows and columns.
    /// </summary>
    /// <param name="rows">The number of rows in the matrix. Must be greater than 0.</param>
    /// <param name="columns">The number of columns in the matrix. Must be greater than 0.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="rows"/> or <paramref name="columns"/> is less than or equal to 0.</exception>
    public Matrix(int rows, int columns)
    {
        if (rows <= 0 || columns <= 0)
            throw new ArgumentException("Matrix must have at least one row and one column.");
        
        m_values = new int[rows, columns];
        Rows = rows;
        Columns = columns;
    }

    /// <summary>
    /// Gets or sets the value at the specified row and column in the matrix.
    /// </summary>
    /// <param name="row">The zero-based index of the row.</param>
    /// <param name="col">The zero-based index of the column.</param>
    /// <returns></returns>
    public int this[int row, int col]
    {
        get => m_values[row, col];
        set => m_values[row, col] = value;
    }

    /// <summary>
    /// Retrieves a row from the matrix as a <see cref="Vector"/>.
    /// </summary>
    /// <param name="rowIndex">The zero-based index of the row to retrieve. Must be within the range [0, <see cref="Rows"/> - 1].</param>
    /// <returns>A <see cref="Vector"/> representing the values in the specified row of the matrix.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="rowIndex"/> is less than 0 or greater than or equal to <see cref="Rows"/>.</exception>
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
    /// Retrieves a column from the matrix as a <see cref="Vector"/>.
    /// </summary>
    /// <param name="columnIndex">The zero-based index of the column to retrieve. Must be within the range [0, <see cref="Columns"/> - 1].</param>
    /// <returns>A <see cref="Vector"/> representing the values in the specified column.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="columnIndex"/> is less than 0 or greater than or equal to <see cref="Columns"/>.</exception>
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
    /// Retrieves all rows of the matrix as a sequence of vectors.
    /// </summary>
    /// <remarks>Each row is returned as a <see cref="Vector"/> object. The rows are yielded in order,
    /// starting from the first row. This method uses deferred execution, meaning rows are generated on demand as the
    /// sequence is enumerated.</remarks>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Vector"/> objects, where each element represents a row of the
    /// matrix.</returns>
    public IEnumerable<Vector> GetRows()
    {
        for (int i = 0; i < Rows; ++i)
            yield return GetRow(i);
    }

    /// <summary>
    /// Returns an enumerator that iterates through all the integer values in the collection.
    /// </summary>
    /// <remarks>The enumerator traverses the collection row by row, yielding each integer value in
    /// sequence.</remarks>
    /// <returns>An <see cref="IEnumerator{T}"/> that can be used to iterate through the integers in the collection.</returns>
    public IEnumerator<int> GetEnumerator()
    {
        for (int i = 0; i < Rows; ++i)
            foreach (int value in GetRow(i))
                yield return value;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Multiplies a matrix by a vector, producing a new vector as the result.
    /// </summary>
    /// <remarks>This operator performs standard matrix-vector multiplication. The input vector is treated as
    /// a column vector  during the operation. The resulting vector has a length equal to the number of rows in the
    /// matrix.</remarks>
    /// <param name="m">The matrix to multiply. The number of columns in the matrix must match the length of the vector.</param>
    /// <param name="v">The vector to multiply. The length of the vector must match the number of columns in the matrix.</param>
    /// <returns>A new <see cref="Vector"/> representing the result of the matrix-vector multiplication.</returns>
    /// <exception cref="ArgumentException">Thrown if the number of columns in <paramref name="m"/> does not match the length of <paramref name="v"/>.</exception>
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
    /// Multiplies a vector by a matrix, returning the resulting vector.
    /// </summary>
    /// <remarks>This operator performs a vector-matrix multiplication. The vector is treated as a row vector
    /// during the operation. The resulting vector corresponds to the first row of the resulting matrix after the
    /// multiplication.</remarks>
    /// <param name="v">The vector to be multiplied. The length of the vector must match the number of rows in the matrix.</param>
    /// <param name="m">The matrix to multiply the vector by. The matrix must have a number of rows equal to the length of the vector.</param>
    /// <returns>A new <see cref="Vector"/> representing the result of the multiplication.</returns>
    /// <exception cref="ArgumentException">Thrown if the length of <paramref name="v"/> does not match the number of rows in <paramref name="m"/>.</exception>
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
    /// Multiplies two matrices and returns the resulting matrix.
    /// </summary>
    /// <param name="m1">The first matrix to multiply. The number of columns in this matrix must match the number of rows in <paramref
    /// name="m2"/>.</param>
    /// <param name="m2">The second matrix to multiply. The number of rows in this matrix must match the number of columns in <paramref
    /// name="m1"/>.</param>
    /// <returns>A new <see cref="Matrix"/> representing the product of <paramref name="m1"/> and <paramref name="m2"/>.</returns>
    /// <exception cref="ArgumentException">Thrown if the number of columns in <paramref name="m1"/> does not match the number of rows in <paramref
    /// name="m2"/>.</exception>
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
    /// Determines whether the specified object is equal to the current <see cref="Matrix"/> instance.
    /// </summary>
    /// <remarks>This method performs a comparison based on the dimensions and values of the matrices.  If the
    /// specified object is not a <see cref="Matrix"/> or has different dimensions, the method returns <see
    /// langword="false"/>.</remarks>
    /// <param name="obj">The object to compare with the current <see cref="Matrix"/> instance.</param>
    /// <returns><see langword="true"/> if the specified object is a <see cref="Matrix"/> with the same dimensions and values as
    /// the current instance; otherwise, <see langword="false"/>.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not Matrix other || other.Rows != Rows || other.Columns != Columns)
            return false;

        return Equals(other);
    }

    /// <summary>
    /// Determines whether the current <see cref="Matrix"/> is equal to another <see cref="Matrix"/>.
    /// </summary>
    /// <remarks>Two matrices are considered equal if they have the same dimensions and all corresponding
    /// elements are equal.</remarks>
    /// <param name="other">The <see cref="Matrix"/> to compare with the current instance. Can be <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the specified <see cref="Matrix"/> is equal to the current instance; otherwise, <see
    /// langword="false"/>.</returns>
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

    /// <summary>
    /// Computes the Kronecker product of two matrices.
    /// </summary>
    /// <remarks>The Kronecker product is a block matrix where each element of the first matrix is multiplied
    /// by the entire second matrix. For example, if <paramref name="m1"/> is an <c>m x n</c> matrix and <paramref
    /// name="m2"/> is a <c>p x q</c> matrix,  the resulting matrix will have dimensions <c>(m * p) x (n *
    /// q)</c>.</remarks>
    /// <param name="m1">The first matrix. Must not be <c>null</c>.</param>
    /// <param name="m2">The second matrix. Must not be <c>null</c>.</param>
    /// <returns>A new matrix representing the Kronecker product of <paramref name="m1"/> and <paramref name="m2"/>.  The
    /// resulting matrix will have dimensions equal to the product of the row and column counts of the input matrices.</returns>
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
    /// Creates and returns an identity matrix of the specified size.
    /// </summary>
    /// <param name="size">The number of rows and columns in the square matrix. Must be a positive integer.</param>
    /// <returns>A square matrix of the specified size with ones on the main diagonal and zeros elsewhere.</returns>
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
