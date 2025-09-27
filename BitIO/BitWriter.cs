namespace CodingTheory.BitIO;

public class BitWriter
{
    private readonly List<byte> _buffer = new List<byte>();
    private int _currentByte;
    private int _bitPosition; // 0..7

    public void WriteBit(int bit)
    {
        if (bit != 0 && bit != 1)
            throw new ArgumentOutOfRangeException(nameof(bit), "Bit must be 0 or 1.");

        // Place bit at current position (MSB first)
        _currentByte = (_currentByte << 1) | bit;
        _bitPosition++;

        if (_bitPosition == 8)
        {
            _buffer.Add((byte)_currentByte);
            _currentByte = 0;
            _bitPosition = 0;
        }
    }

    public void WriteBits(IEnumerable<int> bits)
    {
        foreach (var bit in bits)
            WriteBit(bit);
    }

    public byte[] ToArray()
    {
        // Flush last partial byte if needed (pad with zeros on the right)
        if (_bitPosition > 0)
        {
            int shift = 8 - _bitPosition;
            _currentByte <<= shift;
            _buffer.Add((byte)_currentByte);
        }
        return _buffer.ToArray();
    }
}
