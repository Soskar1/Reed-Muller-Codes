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
}