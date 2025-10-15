namespace CodingTheory.BitIO;

public class BitReader
{
    private readonly byte[] _buffer;
    private int _byteIndex;
    private int _bitIndex; // 0 = Most Significant Bit, 7 = Least Significant Bit
    private const int MaxBits = 8;

    public bool EndOfBuffer { get; private set; }

    /// <summary>
    /// BitReader constructor
    /// </summary>
    /// <param name="buffer">Buffer to read</param>
    /// <exception cref="ArgumentNullException"></exception>
    public BitReader(byte[] buffer)
    {
        _buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
        _byteIndex = 0;
        _bitIndex = 0;
        EndOfBuffer = false;
    }

    /// <summary>
    /// Reads a specified number of bits from the internal buffer and returns them as a list of integers.
    /// </summary>
    /// <remarks>The method updates the internal state of the buffer, including the current byte and bit
    /// indices. If the end of the buffer is reached during the read operation, the <see cref="EndOfBuffer"/> property
    /// is set to <see langword="true"/>.</remarks>
    /// <param name="count">The number of bits to read. Must be non-negative.</param>
    /// <returns>A list of integers representing the bits read from the buffer. Each integer is either 0 or 1. If the end of the
    /// buffer is reached before reading the specified number of bits, the list will contain only the bits that were
    /// successfully read.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="count"/> is negative.</exception>
    public List<int> ReadBits(int count)
    {
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative.");

        var result = new List<int>(count);

        for (int i = 0; i < count; i++)
        {
            // Check if we've reached the end of the buffer
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

                if (_byteIndex == _buffer.Length)
                    EndOfBuffer = true;
            }
        }

        return result;
    }
}
