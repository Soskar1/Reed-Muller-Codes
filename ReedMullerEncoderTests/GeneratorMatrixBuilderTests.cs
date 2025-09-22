using CodingTheory.Math;

namespace CodingTheory.ReedMuller.Tests;

public class GeneratorMatrixBuilderTests
{
    [Test]
    public void Build_M1_ReturnsCorrectMatrix()
    {
        MatrixMod2 expected = new MatrixMod2(new int[,]
        {
            { 1, 1 },
            { 0, 1 }
        });
        MatrixMod2 result = GeneratorMatrixBuilder.Build(1);
        CompareMatrices(expected, result);
    }

    [Test]
    public void Build_M2_ReturnsCorrectMatrix()
    {
        MatrixMod2 expected = new MatrixMod2(new int[,]
        {
            { 1, 1, 1, 1 },
            { 0, 1, 0, 1 },
            { 0, 0, 1, 1 }
        });

        MatrixMod2 result = GeneratorMatrixBuilder.Build(2);
        CompareMatrices(expected, result);
    }

    [Test]
    public void Build_M3_ReturnsCorrectMatrix()
    {
        MatrixMod2 expected = new MatrixMod2(new int[,]
        {
            { 1, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 1, 0, 1, 0, 1, 0, 1 },
            { 0, 0, 1, 1, 0, 0, 1, 1 },
            { 0, 0, 0, 0, 1, 1, 1, 1 }
        });
        MatrixMod2 result = GeneratorMatrixBuilder.Build(3);
        CompareMatrices(expected, result);
    }

    [Test]
    public void Build_InvalidArgument_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => GeneratorMatrixBuilder.Build(0));
        Assert.Throws<ArgumentException>(() => GeneratorMatrixBuilder.Build(-1));
    }

    private void CompareMatrices(MatrixMod2 expected, MatrixMod2 actual)
    {
        Assert.That(actual.Rows, Is.EqualTo(expected.Rows));
        Assert.That(actual.Columns, Is.EqualTo(expected.Columns));
        for (int i = 0; i < expected.Rows; i++)
            for (int j = 0; j < expected.Columns; j++)
                Assert.That(actual[i, j], Is.EqualTo(expected[i, j]));
    }
}