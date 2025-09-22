namespace CodingTheory.Math.Tests;

internal class MatrixMod2UnitTests
{
    [Test]
    public void MatrixMod2Initialization_ValidParameters_MatrixCreated()
    {
        MatrixMod2 matrix = new MatrixMod2(new int[,] { { 1, 2 }, { 3, 4 } });
        Assert.That(matrix[0, 0], Is.EqualTo(1));
        Assert.That(matrix[0, 1], Is.EqualTo(0));
        Assert.That(matrix[1, 0], Is.EqualTo(1));
        Assert.That(matrix[1, 1], Is.EqualTo(0));
    }

    [Test]
    public void MatrixMod2Initialization_InvalidParameters_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => new MatrixMod2(new int[,] { }));
    }

    [Test]
    public void MatrixMod2Indexing_ValidIndices_ReturnsCorrectValue()
    {
        MatrixMod2 matrix = new MatrixMod2(new int[,] { { 1, 0 }, { 0, 1 } });
        Assert.That(matrix[0, 0], Is.EqualTo(1));
        Assert.That(matrix[0, 1], Is.EqualTo(0));
        Assert.That(matrix[1, 0], Is.EqualTo(0));
        Assert.That(matrix[1, 1], Is.EqualTo(1));
    }

    [Test]
    public void MatrixMod2Setter_ValidIndices_SetsValueCorrectly()
    {
        MatrixMod2 matrix = new MatrixMod2(2, 2);
        matrix[0, 0] = 3; // 3 % 2 = 1
        matrix[0, 1] = 4; // 4 % 2 = 0
        matrix[1, 0] = 5; // 5 % 2 = 1
        matrix[1, 1] = 6; // 6 % 2 = 0

        Assert.That(matrix[0, 0], Is.EqualTo(1));
        Assert.That(matrix[0, 1], Is.EqualTo(0));
        Assert.That(matrix[1, 0], Is.EqualTo(1));
        Assert.That(matrix[1, 1], Is.EqualTo(0));
    }

    [Test]
    public void MatrixMod2Multiplication_ValidMatrices_ReturnsCorrectResult()
    {
        MatrixMod2 matrixA = new MatrixMod2(new int[,] {
            { 1, 0 },
            { 1, 1 } });

        MatrixMod2 matrixB = new MatrixMod2(new int[,] {
            { 1, 1 },
            { 0, 1 } });
        
        MatrixMod2 result = matrixA * matrixB;
        
        Assert.That(result.Rows, Is.EqualTo(2));
        Assert.That(result.Columns, Is.EqualTo(2));
        Assert.That(result[0, 0], Is.EqualTo(1));
        Assert.That(result[0, 1], Is.EqualTo(1));
        Assert.That(result[1, 0], Is.EqualTo(1));
        Assert.That(result[1, 1], Is.EqualTo(0));

        result = matrixB * matrixA;

        Assert.That(result.Rows, Is.EqualTo(2));
        Assert.That(result.Columns, Is.EqualTo(2));
        Assert.That(result[0, 0], Is.EqualTo(0));
        Assert.That(result[0, 1], Is.EqualTo(1));
        Assert.That(result[1, 0], Is.EqualTo(1));
        Assert.That(result[1, 1], Is.EqualTo(1));
    }

    [Test]
    public void MatrixMod2Multiplication_InvalidMatrices_ThrowsException()
    {
        MatrixMod2 matrixA = new MatrixMod2(new int[,] {
            { 1, 0, 1 },
            { 1, 1, 0 } });
        MatrixMod2 matrixB = new MatrixMod2(new int[,] {
            { 1, 1 },
            { 0, 1 } });

        Assert.Throws<ArgumentException>(() => { MatrixMod2 result = matrixA * matrixB; });
    }

    [Test]
    public void MatrixMod2Multiplication_MultiplyMatrixByVector_ReturnsCorrectResult()
    {
        MatrixMod2 matrix = new MatrixMod2(new int[,] {
            { 1, 0 },
            { 1, 1 } });
        
        Vector vector = new Vector(new int[] { 1, 1 });
        Vector result = matrix * vector;
        
        Assert.That(result.Length, Is.EqualTo(2));
        Assert.That(result[0], Is.EqualTo(1));
        Assert.That(result[1], Is.EqualTo(0));

        result = vector * matrix;
        Assert.That(result.Length, Is.EqualTo(2));
        Assert.That(result[0], Is.EqualTo(0));
        Assert.That(result[1], Is.EqualTo(1));
    }

    [Test]
    public void MatrixMod2Multiplication_MultiplyMatrixByIncompatibleVector_ThrowsException()
    {
        MatrixMod2 matrix = new MatrixMod2(new int[,] {
            { 1, 0 },
            { 1, 1 } });
        
        Vector vector = new Vector(new int[] { 1, 1, 0 });
        Assert.Throws<ArgumentException>(() => { Vector result = matrix * vector; });
        Assert.Throws<ArgumentException>(() => { Vector result = vector * matrix; });
    }
}
