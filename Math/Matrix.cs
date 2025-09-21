using System.Collections;

namespace CodingTheory.Math;

public class Matrix : IEnumerable<int>
{
    private int[,] m_values;
    public int Rows { get; init; }
    public int Columns { get; init; }

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
}
