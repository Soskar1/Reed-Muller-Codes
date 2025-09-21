using CodingTheory.Math;

namespace CodingTheory.ReedMuller;

public static class GeneratorMatrixBuilder
{
    public static Matrix Build(int m)
    {
        if (m <= 0)
            throw new ArgumentException("Parameter m must be a positive integer.", nameof(m));

        if (m == 1)
        {
            return new Matrix(new int[,]
            {
                { 1, 1 },
                { 0, 1 }
            });
        }

        Matrix smallerMatrix = Build(m - 1);
        int rows = smallerMatrix.Rows + 1;
        int cols = smallerMatrix.Columns * 2;

        Matrix result = new Matrix(rows, cols);

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