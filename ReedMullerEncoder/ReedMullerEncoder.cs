using CodingTheory.BitIO;
using CodingTheory.Math;

namespace CodingTheory.ReedMuller;

public class ReedMullerEncoder
{
    private readonly MatrixMod2 m_generatorMatrix;
    public byte RequiredMessageLength { get; init; }

    // (1, m) Reed-Muller code
    public ReedMullerEncoder(byte m)
    {
        if (m < 2)
            throw new ArgumentException("Parameter m must be at least 2.", nameof(m));

        RequiredMessageLength = (byte)(m + 1);
        m_generatorMatrix = GeneratorMatrixBuilder.Build(m);
    }

    public List<Vector> Encode(byte[] input)
    {
        List<Vector> encodedMessage = new List<Vector>();
        BitReader bitReader = new BitReader(input);
        byte appendedZeroCount = 0;

        Vector encodedM = Vector.ByteToVector((byte)(RequiredMessageLength - 1));
        encodedMessage.Add(encodedM);
        encodedMessage.Add(new Vector(8)); // Placeholder for the appended zeros

        while (!bitReader.EndOfBuffer)
        {
            List<int> bits = bitReader.ReadBits(RequiredMessageLength);

            while (bits.Count < RequiredMessageLength)
            {
                bits.Add(0);
                ++appendedZeroCount;
            }

            Vector v = new Vector(bits.ToArray());
            Vector encodedV = Encode(v);
            encodedMessage.Add(encodedV);
        }

        Vector encodedAppendedZeroCount = Vector.ByteToVector(appendedZeroCount);
        encodedMessage[1] = encodedAppendedZeroCount;
        return encodedMessage;
    }

    public Vector Encode(Vector message)
    {
        if (message.Length != RequiredMessageLength)
            throw new ArgumentException("Invalid vector length");
        
        return message * m_generatorMatrix;
    }
}
