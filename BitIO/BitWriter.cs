namespace CodingTheory.BitIO;

public class BitWriter
{
    private readonly List<byte> m_buffer = new List<byte>();
    private int m_currentByte;
    private int m_bitPosition; // 0..7

    /// <summary>
    /// Writes a single bit to the internal buffer.
    /// </summary>
    /// <remarks>The bit is written to the current position in the internal byte buffer, with bits being
    /// packed in most significant bit (MSB) order. Once 8 bits have been written, the resulting byte is added to the
    /// buffer, and the internal state is reset to prepare for the next byte.</remarks>
    /// <param name="bit">The bit to write. Must be either <see langword="0"/> or <see langword="1"/>.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="bit"/> is not <see langword="0"/> or <see langword="1"/>.</exception>
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

    /// <summary>
    /// Writes a sequence of bits to the output stream.
    /// </summary>
    /// <param name="bits">An enumerable collection of integers representing the bits to write. Each integer must be either 0 or 1.</param>
    public void WriteBits(IEnumerable<int> bits)
    {
        foreach (var bit in bits)
            WriteBit(bit);
    }

    /// <summary>
    /// Converts the internal buffer to a byte array, optionally padding the last byte with zeros.
    /// </summary>
    /// <remarks>If the buffer ends on a partial byte boundary, the last byte is padded with zeros on the
    /// right to complete the byte. The padding is determined by the <paramref name="paddingZeros"/>
    /// parameter.</remarks>
    /// <param name="paddingZeros">The number of zero bits to pad the last byte with, if the buffer does not end on a full byte boundary.  Must be
    /// between 0 and 7, inclusive. Defaults to 0.</param>
    /// <returns>A byte array containing the data from the internal buffer.</returns>
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
