using CodingTheory.BitIO;
using CodingTheory.Math;

namespace CodingTheory.ReedMuller;

public class ReedMullerEncoder
{
    private readonly MatrixMod2 m_generatorMatrix;
    public byte RequiredMessageLength { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReedMullerEncoder"/> class
    /// for the first-order Reed–Muller code <c>RM(1, m)</c>.
    /// </summary>
    /// <param name="m">
    /// The order parameter of the code. Must be at least 2.
    /// Determines the number of variables and thus the size of the generator matrix.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="m"/> is less than 2.
    /// </exception>
    /// <remarks>
    /// This constructor builds the generator matrix <c>G(1, m)</c> using the
    /// <see cref="GeneratorMatrixBuilder"/> and sets the expected input vector length
    /// for encoding.
    /// </remarks>
    public ReedMullerEncoder(byte m)
    {
        if (m < 2)
            throw new ArgumentException("Parameter m must be at least 2.", nameof(m));

        RequiredMessageLength = (byte)(m + 1);
        m_generatorMatrix = GeneratorMatrixBuilder.Build(m);
    }

    /// <summary>
    /// Encodes a byte array into a sequence of Reed–Muller codewords.
    /// </summary>
    /// <param name="input">
    /// The input byte array representing the raw message bits to encode.
    /// </param>
    /// <returns>
    /// A list of <see cref="Vector"/> instances representing encoded codewords.
    /// The first two entries contain metadata:
    /// <list type="number">
    ///   <item>
    ///     <description>The required message length (as an 8-bit vector).</description>
    ///   </item>
    ///   <item>
    ///     <description>The number of zero bits appended to pad the final block (as an 8-bit vector).</description>
    ///   </item>
    /// </list>
    /// The remaining entries are the actual encoded codewords.
    /// </returns>
    /// <remarks>
    /// The method reads the input bits sequentially using <see cref="BitReader"/>.
    /// If the remaining bits at the end of the input are fewer than <see cref="RequiredMessageLength"/>,
    /// the encoder pads the block with zeros and records the number of added bits.
    /// <para>
    /// The resulting list structure is as follows:
    /// <code>
    /// [0]: Encoded RequiredMessageLength (8 bits)
    /// [1]: Encoded number of appended zeros (8 bits)
    /// [2..n]: Encoded message vectors
    /// </code>
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="input"/> is <see langword="null"/>.
    /// </exception>
    public List<Vector> Encode(byte[] input)
    {
        List<Vector> encodedMessage = new List<Vector>();
        BitReader bitReader = new BitReader(input);
        byte appendedZeroCount = 0;

        // First vector: store required message length
        Vector encodedM = Vector.ByteToVector((byte)(RequiredMessageLength - 1));
        encodedMessage.Add(encodedM);

        // Second vector: placeholder for appended zero count
        encodedMessage.Add(new Vector(8));

        // Process all bits in chunks of RequiredMessageLength
        while (!bitReader.EndOfBuffer)
        {
            List<int> bits = bitReader.ReadBits(RequiredMessageLength);

            // Pad with zeros if necessary
            while (bits.Count < RequiredMessageLength)
            {
                bits.Add(0);
                ++appendedZeroCount;
            }

            Vector v = new Vector(bits.ToArray());
            Vector encodedV = Encode(v);
            encodedMessage.Add(encodedV);
        }

        // Replace placeholder with the encoded appended zero count
        Vector encodedAppendedZeroCount = Vector.ByteToVector(appendedZeroCount);
        encodedMessage[1] = encodedAppendedZeroCount;

        return encodedMessage;
    }

    /// <summary>
    /// Encodes a single message vector using the Reed–Muller generator matrix.
    /// </summary>
    /// <param name="message">The message vector to encode. Must have length equal to <see cref="RequiredMessageLength"/>.</param>
    /// <returns>
    /// A <see cref="Vector"/> representing the encoded codeword.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the message vector has an invalid length.
    /// </exception>
    /// <remarks>
    /// Encoding is performed using binary matrix multiplication:
    /// <code>
    /// codeword = message × G(1, m)
    /// </code>
    /// All arithmetic is performed modulo 2.
    /// </remarks>
    public Vector Encode(Vector message)
    {
        if (message.Length != RequiredMessageLength)
            throw new ArgumentException("Invalid vector length");
        
        return message * m_generatorMatrix;
    }
}
