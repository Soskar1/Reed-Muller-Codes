using CodingTheory.Math;

namespace CodingTheory.ReedMuller.Tests;

internal class ReedMullerEncoderTests
{
    [Test]
    public void Encode_M3Message_ReturnsCorrectCodeword()
    {
        ReedMullerEncoder encoder = new ReedMullerEncoder(3);

        // Message: (λ1, λ2, λ3, μ) = (1, 0, 1, 1) = x1 + x3 + 1
        Vector message = new Vector(new int[] { 1, 0, 1, 1 });
        Vector expectedCodeword = new Vector(new int[] { 1, 1, 0, 0, 0, 0, 1, 1 });

        Vector result = encoder.Encode(message);
        Assert.That(result, Is.EqualTo(expectedCodeword));
    }
}
