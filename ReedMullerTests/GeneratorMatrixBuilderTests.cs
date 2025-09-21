using CodingTheory.Math;

namespace CodingTheory.ReedMuller.Tests;

public class GeneratorMatrixBuilderTests
{
    [Test]
    public void Build_M1_ReturnsCorrectMatrix()
    {
        var expected = new Matrix(new int[,]
        {
            { 1, 1 },
            { 0, 1 }
        });
        var result = GeneratorMatrixBuilder.Build(1);
        CompareMatrices(expected, result);
    }

    [Test]
    public void Build_M2_ReturnsCorrectMatrix()
    {
        var expected = new Matrix(new int[,]
        {
            { 1, 1, 1, 1 },
            { 0, 1, 0, 1 },
            { 0, 0, 1, 1 }
        });

        var result = GeneratorMatrixBuilder.Build(2);
        CompareMatrices(expected, result);
    }

    [Test]
    public void Build_M3_ReturnsCorrectMatrix()
    {
        var expected = new Matrix(new int[,]
        {
            { 1, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 1, 0, 1, 0, 1, 0, 1 },
            { 0, 0, 1, 1, 0, 0, 1, 1 },
            { 0, 0, 0, 0, 1, 1, 1, 1 }
        });
        var result = GeneratorMatrixBuilder.Build(3);
        CompareMatrices(expected, result);
    }

    private void CompareMatrices(Matrix expected, Matrix actual)
    {
        Assert.That(actual.Rows, Is.EqualTo(expected.Rows));
        Assert.That(actual.Columns, Is.EqualTo(expected.Columns));
        for (int i = 0; i < expected.Rows; i++)
            for (int j = 0; j < expected.Columns; j++)
                Assert.That(actual[i, j], Is.EqualTo(expected[i, j]));
    }
}