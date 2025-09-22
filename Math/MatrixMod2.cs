namespace CodingTheory.Math;

public sealed class MatrixMod2 : Matrix
{
    public MatrixMod2(int[,] values) : base(values)
    {
        for (int i = 0; i < Rows; ++i)
            for (int j = 0; j < Columns; ++j)
                values[i, j] %= 2;
    }

    public MatrixMod2(int rows, int columns) : base(rows, columns) { }

    public new int this[int row, int col]
    {
        get => base[row, col];
        set => base[row, col] = value % 2;
    }

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

    public static Vector operator *(MatrixMod2 m, Vector v)
    {
        if (m.Columns != v.Length)
            throw new ArgumentException("Matrix columns must match vector length for multiplication.");
        
        MatrixMod2 vMatrix = v.ToColumnMatrix().ToMod2();

        return (m * vMatrix).GetColumn(0);
    }

    public static Vector operator *(Vector v, MatrixMod2 m)
    {
        if (v.Length != m.Rows)
            throw new ArgumentException("Vector length must match matrix rows for multiplication.");
        
        MatrixMod2 vMatrix = v.ToRowMatrix().ToMod2();
        return (vMatrix * m).GetRow(0);
    }
}
