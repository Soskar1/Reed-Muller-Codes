namespace CodingTheory.Math;

public static class MatrixExtensions
{
    public static MatrixMod2 ToMod2(this Matrix matrix) => new MatrixMod2(matrix.Values);
}
