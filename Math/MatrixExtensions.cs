namespace CodingTheory.Math;

public static class MatrixExtensions
{
    /// <summary>
    /// Constructing a Mod2 matrix from a regular matrix
    /// </summary>
    /// <param name="matrix">Any matrix</param>
    /// <returns>Mod2 matrix</returns>
    public static MatrixMod2 ToMod2(this Matrix matrix) => new MatrixMod2(matrix.Values);

    /// <summary>
    /// Computes the Kronecker product of two matrices.
    /// </summary>
    /// <param name="m1">Any matrix</param>
    /// <param name="m2">Any matrix</param>
    /// <returns>Matrix</returns>
    public static Matrix KroneckerProduct(this Matrix m1, Matrix m2) => Matrix.KroneckerProduct(m1, m2);
}
