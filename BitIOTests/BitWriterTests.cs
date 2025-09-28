namespace CodingTheory.BitIO.Tests;

public class BitWriterTests
{
    [Test]
    public void BitWriter_WriteIntoOneByte_ReturnsCorrectByte()
    {
        BitWriter writer = new BitWriter();

        writer.WriteBits(new[] { 1, 0, 1, 1, 0, 0, 1 });
        byte[] buffer = writer.ToArray();

        BitReader reader = new BitReader(buffer);
        List<int> bits = reader.ReadBits(8);

        Assert.That(string.Join("", bits), Is.EqualTo("10110010"));
    }

    [Test]
    public void BitWriter_WriteIntoTwoBytes_ReturnsCorrectBytes()
    {
        BitWriter writer = new BitWriter();

        writer.WriteBits(new[] { 1, 0, 1, 1, 0, 0, 1, 1, 1, 1 });
        byte[] buffer = writer.ToArray();

        Assert.That(buffer.Length, Is.EqualTo(2));
        Assert.That(buffer[0], Is.EqualTo(179));
        Assert.That(buffer[1], Is.EqualTo(192));
    }
}
