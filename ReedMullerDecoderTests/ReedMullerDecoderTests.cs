using CodingTheory.Math;

namespace CodingTheory.ReedMuller.Tests;

internal class ReedMullerDecoderTests
{
    [TestCaseSource(typeof(TestCases), nameof(TestCases.M3EncodedMessaged))]
    public void Decode_M3Message_DecodesCorrectly(Vector encodedMessage, Vector expected)
    {
        ReedMullerDecoder decoder = new ReedMullerDecoder(3);
        Vector result = decoder.Decode(encodedMessage);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Decode_M3VectorArray_DecodesCorrectly()
    {
        Vector[] encodedMessage = new Vector[]
            {
            new Vector([0, 0, 0, 0, 0, 0, 1, 1]), // m = 3
            new Vector([0, 0, 0, 0, 0, 0, 0, 0]), // PaddingZeros = 0
            new Vector([0, 1, 0, 1, 1, 0, 1, 0]), // T - first 4 bits
            new Vector([0, 1, 0, 1, 0, 1, 0, 1]), // T - last 4 bits
            new Vector([0, 1, 1, 0, 0, 1, 1, 0]), // e - first 4 bits
            new Vector([0, 1, 0, 1, 1, 0, 1, 0]), // e - last 4 bits
            new Vector([0, 1, 1, 0, 1, 0, 0, 1]), // s - first 4 bits
            new Vector([0, 0, 1, 1, 1, 1, 0, 0]), // s - last 4 bits
            new Vector([0, 1, 1, 0, 1, 0, 0, 1]), // t - first 4 bits
            new Vector([0, 1, 0, 1, 0, 1, 0, 1])  // t - last 4 bits
            };

        ReedMullerDecoder decoder = new ReedMullerDecoder();
        byte[] bytes = decoder.Decode(encodedMessage);
        string text = System.Text.Encoding.UTF8.GetString(bytes);

        Assert.That(text, Is.EqualTo("Test"));
    }

    [Test]
    public void Decode_M4VectorArray_DecodesCorrecly()
    {
        Vector[] encodedMessage = new Vector[]
            {
            new Vector([0, 0, 0, 0, 0, 1, 0, 0]), // m = 4
            new Vector([0, 0, 0, 0, 0, 0, 1, 1]), // PaddingZeros = 3
            new Vector([0, 1, 0, 1, 1, 0, 1, 0, 0, 1, 0, 1, 1, 0, 1, 0]),
            new Vector([1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0]),
            new Vector([1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0]),
            new Vector([1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0]),
            new Vector([0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0]),
            new Vector([1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 1, 0, 0, 1, 1, 0]),
            new Vector([0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]) // Has 3 padding zeros
            };

        ReedMullerDecoder decoder = new ReedMullerDecoder();
        byte[] bytes = decoder.Decode(encodedMessage);
        string text = System.Text.Encoding.UTF8.GetString(bytes);

        Assert.That(text, Is.EqualTo("Test"));
    }
}