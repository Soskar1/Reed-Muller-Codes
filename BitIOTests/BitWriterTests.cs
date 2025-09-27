namespace CodingTheory.BitIO.Tests;

public class BitWriterTests
{
    [Test]
    public void BitWriterTest()
    {
        BitWriter writer = new BitWriter();

        writer.WriteBits(new[] { 1, 0, 1, 1, 0, 0, 1 });
        byte[] buffer = writer.ToArray();

        BitReader reader = new BitReader(buffer);
        List<int> bits = reader.ReadBits(8);

        Assert.That(string.Join("", bits), Is.EqualTo("10110010"));
    }
}
