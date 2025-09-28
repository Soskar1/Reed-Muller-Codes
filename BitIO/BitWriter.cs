namespace CodingTheory.BitIO;

public class BitWriter
{
    private readonly List<byte> m_buffer = new List<byte>();
    private int m_currentByte;
    private int m_bitPosition; // 0..7

    public void WriteBit(int bit)
    {
        if (bit != 0 && bit != 1)
            throw new ArgumentOutOfRangeException(nameof(bit), "Bit must be 0 or 1.");

        // Place bit at current position (MSB first)
        m_currentByte = (m_currentByte << 1) | bit;
        m_bitPosition++;

        if (m_bitPosition == 8)
        {
            m_buffer.Add((byte)m_currentByte);
            m_currentByte = 0;
            m_bitPosition = 0;
        }
    }

    public void WriteBits(IEnumerable<int> bits)
    {
        foreach (var bit in bits)
            WriteBit(bit);
    }

    public byte[] ToArray(int paddingZeros = 0)
    {
        // Flush last partial byte if needed (pad with zeros on the right)
        if (m_bitPosition > 0 && m_bitPosition - paddingZeros > 0)
        {
            int shift = 8 - m_bitPosition;
            m_currentByte <<= shift;
            m_buffer.Add((byte)m_currentByte);
        }
        return m_buffer.ToArray();
    }
}
