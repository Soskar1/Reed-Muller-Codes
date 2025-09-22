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
        Assert.That(result, Is.EqualTo(expected));
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
        Assert.That(result, Is.EqualTo(expected));
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
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Build_InvalidArgument_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => GeneratorMatrixBuilder.Build(0));
        Assert.Throws<ArgumentException>(() => GeneratorMatrixBuilder.Build(-1));
    }
}