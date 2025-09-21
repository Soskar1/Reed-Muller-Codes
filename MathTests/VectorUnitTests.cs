namespace ReedMullerCodes.Math.Tests;

public class Tests
{
    [Test]
    public void VectorInitialization_ValidParameters_VectorCreated()
    {
        Vector vector = new Vector(new int[] { 1, 1, 1 });
        Assert.Pass();
    }

    [Test]
    public void VectorInitialization_InvalidParameters_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => new Vector(new int[] { }));
    }

    [Test]
    public void VectorIndexing_ValidIndex_ReturnsCorrectValue()
    {
        Vector vector = new Vector(new int[] { 1, 2, 3 });
        Assert.That(vector[0], Is.EqualTo(1));
        Assert.That(vector[1], Is.EqualTo(2));
        Assert.That(vector[2], Is.EqualTo(3));
    }

    [Test]
    public void VectorIndexing_SetValue_UpdatesValue()
    {
        Vector vector = new Vector(new int[] { 1, 2, 3 });
        vector[1] = 5;

        Assert.That(vector[0], Is.EqualTo(1));
        Assert.That(vector[1], Is.EqualTo(5));
        Assert.That(vector[2], Is.EqualTo(3));
    }

    [Test]
    public void VectorLength_ReturnsCorrectLength()
    {
        Vector vector = new Vector(new int[] { 1, 2, 3, 4 });
        Assert.That(vector.Length, Is.EqualTo(4));
    }

    [Test]
    public void VectorAddition_AddTwoVectors_ReturnsCorrectResult()
    {
        Vector vector1 = new Vector(new int[] { 1, 2, 3 });
        Vector vector2 = new Vector(new int[] { 4, 5, 6 });
        Vector result = vector1 + vector2;

        Assert.That(result[0], Is.EqualTo(5));
        Assert.That(result[1], Is.EqualTo(7));
        Assert.That(result[2], Is.EqualTo(9));
    }

    [Test]
    public void VectorAddition_DifferentLengthVectors_ThrowsException()
    {
        Vector vector1 = new Vector(new int[] { 1, 2, 3 });
        Vector vector2 = new Vector(new int[] { 4, 5 });
        Assert.Throws<ArgumentException>(() => { var result = vector1 + vector2; });
    }

    [Test]
    public void VectorMultiplication_MultiplyVectorByScalar_ReturnsCorrectResult()
    {
        Vector vector = new Vector(new int[] { 1, 2, 3 });
        Vector result = vector * 3;

        Assert.That(result[0], Is.EqualTo(3));
        Assert.That(result[1], Is.EqualTo(6));
        Assert.That(result[2], Is.EqualTo(9));

        result = 3 * vector;
        Assert.That(result[0], Is.EqualTo(3));
        Assert.That(result[1], Is.EqualTo(6));
        Assert.That(result[2], Is.EqualTo(9));
    }
}