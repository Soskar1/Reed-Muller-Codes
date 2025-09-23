namespace CodingTheory.Math;

public static class MatrixExtensions
{
    public static MatrixMod2 ToMod2(this Matrix matrix) => new MatrixMod2(matrix.Values);

    public static Matrix KroneckerProduct(this Matrix m1, Matrix m2) => Matrix.KroneckerProduct(m1, m2);
}
