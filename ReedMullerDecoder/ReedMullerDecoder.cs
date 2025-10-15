using CodingTheory.Math;
using CodingTheory.BitIO;

namespace CodingTheory.ReedMuller;

public class ReedMullerDecoder
{
    private int m_m;
    private Matrix[] m_kroneckerMatrices;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReedMullerDecoder"/> class
    /// without specifying the code order.
    /// </summary>
    /// <remarks>
    /// Use this constructor if the decoder will later be configured automatically
    /// from the metadata of an encoded message (see <see cref="Decode(Vector[])"/>).
    /// </remarks>
    public ReedMullerDecoder() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReedMullerDecoder"/> class
    /// for the specified order parameter <paramref name="m"/>.
    /// </summary>
    /// <param name="m">
    /// The order parameter of the Reed–Muller code <c>RM(1, m)</c>.
    /// Determines the number of variables and code length.
    /// </param>
    /// <remarks>
    /// This constructor precomputes the Kronecker product matrices required
    /// for fast Hadamard-based decoding.
    /// </remarks>
    public ReedMullerDecoder(int m)
    {
        m_m = m;
        m_kroneckerMatrices = GenerateKroneckerMatrices();
    }

    /// <summary>
    /// Generates the sequence of Kronecker matrices used in decoding.
    /// </summary>
    /// <returns>
    /// An array of <see cref="Matrix"/> objects, each representing a partial
    /// Kronecker product expansion of the Hadamard matrix.
    /// </returns>
    /// <remarks>
    /// The method constructs matrices of the form:
    /// <code>
    /// I_{2^(m−i)} × H × I_{2^(i−1)}
    /// </code>
    /// where <c>H</c> is the 2×2 Hadamard matrix:
    /// <code>
    /// H = [ 1  1 ]
    ///     [ 1 -1 ]
    /// </code>
    /// These matrices are used sequentially to perform the decoding transform.
    /// </remarks>
    private Matrix[] GenerateKroneckerMatrices()
    {
        Matrix[] kroneckerMatrices = new Matrix[m_m];

        Matrix H = new Matrix(new int[,]
        {
            { 1, 1 },
            { 1, -1 }
        });

        for (int i = 1; i <= m_m; ++i)
        {
            Matrix firstIdentity = Matrix.IdentityMatrix((int)MathF.Pow(2, m_m - i));
            Matrix secondIdentity = Matrix.IdentityMatrix((int)MathF.Pow(2, i - 1));
            kroneckerMatrices[i - 1] = firstIdentity.KroneckerProduct(H).KroneckerProduct(secondIdentity);
        }

        return kroneckerMatrices;
    }

    /// <summary>
    /// Decodes a single Reed–Muller encoded codeword vector.
    /// </summary>
    /// <param name="encodedMessage">The encoded vector representing a single codeword.</param>
    /// <returns>
    /// A <see cref="Vector"/> containing the decoded message bits.
    /// </returns>
    public Vector Decode(Vector encodedMessage)
    {
        // 1st step: replace all 0 by -1
        for (int i = 0; i < encodedMessage.Length; ++i)
            if (encodedMessage[i] == 0)
                encodedMessage[i] = -1;

        // 2nd step: apply Hadamard transform using Kronecker matrices
        for (int i = 0; i < m_m; ++i)
            encodedMessage = encodedMessage * m_kroneckerMatrices[i];

        // 3rd step: find index of largest absolute value
        int largestIndex = -1;
        int maxValue = Int32.MinValue;
        for (int i = 0; i < encodedMessage.Length; ++i)
        {
            if (MathF.Abs(encodedMessage[i]) > maxValue)
            {
                maxValue = (int)MathF.Abs(encodedMessage[i]);
                largestIndex = i;
            }
        }

        // 4th step: construct decoded message vector
        Vector decodedMessage = new Vector(m_m + 1);
        if (encodedMessage[largestIndex] > 0)
            decodedMessage[0] = 1;

        // Convert index to binary and reverse it
        char[] indexBinary = Convert.ToString(largestIndex, 2).PadLeft(m_m, '0').ToCharArray();
        Array.Reverse(indexBinary);
        string mirroredBinaryRepresentation = new string(indexBinary);

        // Fill in the rest of the decoded message
        for (int i = 1; i < decodedMessage.Length; ++i)
            decodedMessage[i] = (int)Char.GetNumericValue(mirroredBinaryRepresentation[i - 1]);

        return decodedMessage;
    }

    /// <summary>
    /// Decodes a sequence of Reed–Muller encoded vectors into a byte array.
    /// </summary>
    /// <param name="encodedMessage">
    /// An array of <see cref="Vector"/> objects representing the full encoded message,
    /// including metadata vectors and codeword blocks.
    /// </param>
    /// <returns>
    /// A byte array representing the reconstructed original message.
    /// </returns>
    /// <remarks>
    /// The method expects the input array in the following structure:
    /// <list type="number">
    ///   <item><description>Index 0: Encoded message length (<c>m</c>).</description></item>
    ///   <item><description>Index 1: Number of zero bits appended for padding.</description></item>
    ///   <item><description>Indices 2...n: Encoded message codewords.</description></item>
    /// </list>
    /// <para>
    /// The decoder reconstructs <c>m</c> from the metadata, regenerates the Kronecker matrices,
    /// decodes each codeword via <see cref="Decode(Vector)"/>, and writes the resulting bits to a <see cref="BitWriter"/>.
    /// </para>
    /// </remarks>
    public byte[] Decode(Vector[] encodedMessage)
    {
        BitWriter writer = new BitWriter();

        m_m = (byte)encodedMessage[0];
        byte paddingZeros = (byte)encodedMessage[1];

        m_kroneckerMatrices = GenerateKroneckerMatrices();

        foreach (Vector msg in encodedMessage.Skip(2))
        {
            Vector decodedMessage = Decode(msg);
            writer.WriteBits(decodedMessage);
        }

        return writer.ToArray(paddingZeros);
    }
}
