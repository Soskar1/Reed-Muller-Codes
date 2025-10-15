namespace CodingTheory.Math;

public sealed class MatrixMod2 : Matrix
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MatrixMod2"/> class with the specified values, ensuring all
    /// elements are reduced modulo 2.
    /// </summary>
    /// <remarks>The input array is modified in place, with each element replaced by its value modulo 2. 
    /// Ensure that the input array is not shared with other parts of the application if this behavior is not
    /// desired.</remarks>
    /// <param name="values">A two-dimensional array representing the initial values of the matrix.  Each element will be reduced modulo 2.</param>
    public MatrixMod2(int[,] values) : base(values)
    {
        for (int i = 0; i < Rows; ++i)
            for (int j = 0; j < Columns; ++j)
                values[i, j] %= 2;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MatrixMod2"/> class with the specified number of rows and columns.
    /// </summary>
    /// <remarks>The matrix operates in modulo 2 arithmetic, where all entries are either 0 or 1.</remarks>
    /// <param name="rows">The number of rows in the matrix. Must be a non-negative integer.</param>
    /// <param name="columns">The number of columns in the matrix. Must be a non-negative integer.</param>
    public MatrixMod2(int rows, int columns) : base(rows, columns) { }

    /// <summary>
    /// Gets or sets the element at the specified row and column, reducing any assigned value modulo 2.
    /// </summary>
    /// <param name="row">The zero-based row index of the element to access.</param>
    /// <param name="col">The zero-based column index of the element to access.</param>
    /// <returns>The value of the specified matrix element (always 0 or 1).</returns>
    public new int this[int row, int col]
    {
        get => base[row, col];
        set => base[row, col] = value % 2;
    }

    /// <summary>
    /// Multiplies two matrices using modulo 2 arithmetic.
    /// </summary>
    /// <param name="a">The left-hand operand of the multiplication.</param>
    /// <param name="b">The right-hand operand of the multiplication.</param>
    /// <returns>
    /// A new <see cref="MatrixMod2"/> instance representing the product <c>a × b</c>,
    /// computed modulo 2.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the number of columns in <paramref name="a"/> does not match the
    /// number of rows in <paramref name="b"/>.
    /// </exception>
    public static MatrixMod2 operator *(MatrixMod2 a, MatrixMod2 b)
    {
        if (a.Columns != b.Rows)
            throw new ArgumentException("Incompatible matrix dimensions for multiplication.");
        
        MatrixMod2 result = new MatrixMod2(a.Rows, b.Columns);
        for (int i = 0; i < a.Rows; ++i)
        {
            for (int j = 0; j < b.Columns; ++j)
            {
                int sum = 0;
                
                for (int k = 0; k < a.Columns; ++k)
                    sum += a[i, k] * b[k, j];
                
                result[i, j] = sum % 2;
            }
        }

        return result;
    }

    /// <summary>
    /// Multiplies a matrix by a vector using modulo 2 arithmetic.
    /// </summary>
    /// <param name="m">The matrix operand.</param>
    /// <param name="v">The vector operand.</param>
    /// <returns>
    /// A new <see cref="Vector"/> representing the product <c>m × v</c>,
    /// with all computations performed modulo 2.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the number of columns in <paramref name="m"/> does not match
    /// the length of <paramref name="v"/>.
    /// </exception>
    public static Vector operator *(MatrixMod2 m, Vector v)
    {
        if (m.Columns != v.Length)
            throw new ArgumentException("Matrix columns must match vector length for multiplication.");
        
        MatrixMod2 vMatrix = v.ToColumnMatrix().ToMod2();

        return (m * vMatrix).GetColumn(0);
    }

    /// <summary>
    /// Multiplies a vector by a matrix using modulo 2 arithmetic.
    /// </summary>
    /// <param name="v">The vector operand.</param>
    /// <param name="m">The matrix operand.</param>
    /// <returns>
    /// A new <see cref="Vector"/> representing the product <c>v × m</c>,
    /// with all computations performed modulo 2.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the length of <paramref name="v"/> does not match
    /// the number of rows in <paramref name="m"/>.
    /// </exception>
    public static Vector operator *(Vector v, MatrixMod2 m)
    {
        if (v.Length != m.Rows)
            throw new ArgumentException("Vector length must match matrix rows for multiplication.");
        
        MatrixMod2 vMatrix = v.ToRowMatrix().ToMod2();
        return (vMatrix * m).GetRow(0);
    }
}
