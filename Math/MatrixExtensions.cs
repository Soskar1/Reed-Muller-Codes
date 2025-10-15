namespace CodingTheory.Math;

public static class MatrixExtensions
{
    /// <summary>
    /// Converts the current <see cref="Matrix"/> to a <see cref="MatrixMod2"/> representation.
    /// </summary>
    /// <param name="matrix">The source <see cref="Matrix"/> to convert. Cannot be <see langword="null"/>.</param>
    /// <returns>A new <see cref="MatrixMod2"/> instance containing the mod-2 representation of the source matrix.</returns>
    public static MatrixMod2 ToMod2(this Matrix matrix) => new MatrixMod2(matrix.Values);

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
    public static Matrix KroneckerProduct(this Matrix m1, Matrix m2) => Matrix.KroneckerProduct(m1, m2);
}
