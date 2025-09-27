namespace BitIO;

public class BitReader
{
    private readonly byte[] _buffer;
    private int _byteIndex;
    private int _bitIndex; // 0 = MSB, 7 = LSB
    private const int MaxBits = 8;

    public bool EndOfBuffer { get; private set; }

    public BitReader(byte[] buffer)
    {
        _buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
        _byteIndex = 0;
        _bitIndex = 0;
        EndOfBuffer = false;
    }

    public List<int> ReadBits(int count)
    {
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative.");

        var result = new List<int>(count);

        for (int i = 0; i < count; i++)
        {
            if (_byteIndex >= _buffer.Length)
            {
                EndOfBuffer = true;
                return result;
            }

            // Extract current bit
            int bit = (_buffer[_byteIndex] >> (MaxBits - 1 - _bitIndex)) & 1;
            result.Add(bit);

            // Move to next bit
            _bitIndex++;
            if (_bitIndex == MaxBits)
            {
                _bitIndex = 0;
                _byteIndex++;
            }
        }

        return result;
    }

    public bool HasMoreBits => _byteIndex < _buffer.Length;
}
