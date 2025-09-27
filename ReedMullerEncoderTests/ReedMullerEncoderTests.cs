using CodingTheory.Math;
using System.Text;

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

    [Test]
    public void Encode_M3ByteArray_ReturnsEncodedMessageWithHeaders()
    {
        ReedMullerEncoder encoder = new ReedMullerEncoder(3);

        byte[] message = Encoding.UTF8.GetBytes("Test");
        List<Vector> encodedMessage = encoder.Encode(message);

        Assert.That(encodedMessage[0], Is.EqualTo(Vector.ByteToVector(3))); // M=3
        Assert.That(encodedMessage[1], Is.EqualTo(Vector.ByteToVector(0))); // PaddingZeros=0
        Assert.That(encodedMessage[2], Is.EqualTo(new Vector(new int[8] { 0, 1, 0, 1, 1, 0, 1, 0 }))); // T - first 4 bits
        Assert.That(encodedMessage[3], Is.EqualTo(new Vector(new int[8] { 0, 1, 0, 1, 0, 1, 0, 1 }))); // T - last 4 bits
        Assert.That(encodedMessage[4], Is.EqualTo(new Vector(new int[8] { 0, 1, 1, 0, 0, 1, 1, 0 }))); // e - first 4 bits
        Assert.That(encodedMessage[5], Is.EqualTo(new Vector(new int[8] { 0, 1, 0, 1, 1, 0, 1, 0 }))); // e - last 4 bits
        Assert.That(encodedMessage[6], Is.EqualTo(new Vector(new int[8] { 0, 1, 1, 0, 1, 0, 0, 1 }))); // s - first 4 bits
        Assert.That(encodedMessage[7], Is.EqualTo(new Vector(new int[8] { 0, 0, 1, 1, 1, 1, 0, 0 }))); // s - last 4 bits
        Assert.That(encodedMessage[8], Is.EqualTo(new Vector(new int[8] { 0, 1, 1, 0, 1, 0, 0, 1 }))); // t - first 4 bits
        Assert.That(encodedMessage[9], Is.EqualTo(new Vector(new int[8] { 0, 1, 0, 1, 0, 1, 0, 1 }))); // t - last 4 bits
    }

    [Test]
    public void Encode_M4ByteArray_ReturnsEncoddedMessageWithPaddingZeros()
    {
        ReedMullerEncoder encoder = new ReedMullerEncoder(4);

        byte[] message = Encoding.UTF8.GetBytes("Test");

        // Message to encode: 01010 10001 10010 10111 00110 11101 00
        List<Vector> encodedMessage = encoder.Encode(message);

        Assert.That(encodedMessage[0], Is.EqualTo(Vector.ByteToVector(4))); // M=4
        Assert.That(encodedMessage[1], Is.EqualTo(Vector.ByteToVector(3))); // PaddingZeros=3
        Assert.That(encodedMessage[2], Is.EqualTo(new Vector(new int[16] { 0, 1, 0, 1, 1, 0, 1, 0, 0, 1, 0, 1, 1, 0, 1, 0 }))); // 01010
        Assert.That(encodedMessage[3], Is.EqualTo(new Vector(new int[16] { 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 }))); // 10001
        Assert.That(encodedMessage[4], Is.EqualTo(new Vector(new int[16] { 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0 }))); // 10010
        Assert.That(encodedMessage[5], Is.EqualTo(new Vector(new int[16] { 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0 }))); // 10111
        Assert.That(encodedMessage[6], Is.EqualTo(new Vector(new int[16] { 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0 }))); // 00110
        Assert.That(encodedMessage[7], Is.EqualTo(new Vector(new int[16] { 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 1, 0, 0, 1, 1, 0 }))); // 11101
        Assert.That(encodedMessage[8], Is.EqualTo(new Vector(new int[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }))); // 00000 (3 padding zeros)

    }
}
