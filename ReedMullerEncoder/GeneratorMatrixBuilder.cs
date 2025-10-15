using CodingTheory.Math;

namespace CodingTheory.ReedMuller;

public static class GeneratorMatrixBuilder
{
    /// <summary>
    /// Builds the generator matrix <c>G(1, m)</c> for the first-order Reed–Muller code <c>RM(1, m)</c>.
    /// </summary>
    /// <param name="m">
    /// The number of variables (or the order parameter) for the Reed–Muller code.
    /// Must be a positive integer.
    /// </param>
    /// <returns>
    /// A <see cref="MatrixMod2"/> instance representing the generator matrix <c>G(1, m)</c>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="m"/> is less than or equal to zero.
    /// </exception>
    /// <remarks>
    /// This method implements the recursive relationship:
    /// <list type="bullet">
    ///   <item>
    ///     <description>For <c>m = 1</c>:
    ///     <code>
    ///     G(1, 1) =
    ///     [1 1]
    ///     [0 1]
    ///     </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>For <c>m > 1</c>:
    ///     <code>
    ///     G(1, m) =
    ///     [ G(1, m − 1)  G(1, m − 1) ]
    ///     [      0          11...1   ]
    ///     </code>
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    public static MatrixMod2 Build(int m)
    {
        if (m <= 0)
            throw new ArgumentException("Parameter m must be a positive integer.", nameof(m));

        if (m == 1)
        {
            return new MatrixMod2(new int[,]
            {
                { 1, 1 },
                { 0, 1 }
            });
        }

        MatrixMod2 smallerMatrix = Build(m - 1);
        int rows = smallerMatrix.Rows + 1;
        int cols = smallerMatrix.Columns * 2;

        MatrixMod2 result = new MatrixMod2(rows, cols);

        for (int i = 0; i < smallerMatrix.Rows; ++i)
        {
            for (int j = 0; j < smallerMatrix.Columns; ++j)
            {
                result[i, j] = smallerMatrix[i, j];
                result[i, j + smallerMatrix.Columns] = smallerMatrix[i, j];
            }
        }

        for (int j = 0; j < smallerMatrix.Columns; ++j)
            result[smallerMatrix.Rows, j] = 0;

        for (int j = smallerMatrix.Columns; j < cols; ++j)
            result[smallerMatrix.Rows, j] = 1;

        return result;
    }
}