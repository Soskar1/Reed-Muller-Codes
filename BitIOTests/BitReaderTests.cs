namespace CodingTheory.BitIO.Tests;

public class BitReaderTests
{
    [Test]
    public void BitReader_ReadOneBit_Success()
    {
        byte[] buffer = { 0b10110010 };
        BitReader reader = new BitReader(buffer);

        List<int> bits = reader.ReadBits(1);

        Assert.That(bits.Count, Is.EqualTo(1));
        Assert.That(bits[0], Is.EqualTo(1));
    }

    [Test]
    public void BitReader_ReadTwoBitsAtOneCall_Sucess()
    {
        byte[] buffer = { 0b10110010 };
        BitReader reader = new BitReader(buffer);

        List<int> bits = reader.ReadBits(2);

        Assert.That(bits.Count, Is.EqualTo(2));
        Assert.That(string.Join("", bits), Is.EqualTo("10"));
    }

    [Test]
    public void BitReader_ReadTwoBitsInASeparateCalls_Success()
    {
        byte[] buffer = { 0b10110010 };
        BitReader reader = new BitReader(buffer);

        List<int> bits1 = reader.ReadBits(1);
        List<int> bits2 = reader.ReadBits(1);

        Assert.That(string.Join("", bits1) + string.Join("", bits2), Is.EqualTo("10"));
    }

    [Test]
    public void BitReader_ReadFourBits_Success()
    {
        byte[] buffer = { 0b10110010 };
        BitReader reader = new BitReader(buffer);

        List<int> bits = reader.ReadBits(4);

        Assert.That(string.Join("", bits), Is.EqualTo("1011"));
    }

    [Test]
    public void BitReader_Read10BitsFrom2Bytes_Sucsess()
    {
        byte[] buffer = { 0b10110010, 0b01101100 };
        BitReader reader = new BitReader(buffer);

        List<int> bits = reader.ReadBits(10);

        Assert.That(string.Join("", bits), Is.EqualTo("1011001001"));
    }

    [Test]
    public void BitReader_ReadMoreBitsThanInABuffer_EndOfBufferFlagIsSet()
    {
        byte[] buffer = { 0b10110010 };
        BitReader reader = new BitReader(buffer);

        List<int> bits1 = reader.ReadBits(7);
        List<int> bits2 = reader.ReadBits(2);

        Assert.That(string.Join("", bits1) + string.Join("", bits2), Is.EqualTo("10110010"));
        Assert.That(reader.EndOfBuffer, Is.True);
    }
}