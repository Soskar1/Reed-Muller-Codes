using CodingTheory.Math;
using CodingTheory.BitIO;

namespace CodingTheory.ReedMuller;

public class ReedMullerDecoder
{
    private int m_m;
    private Matrix[] m_kroneckerMatrices;

    public ReedMullerDecoder() { }

    public ReedMullerDecoder(int m)
    {
        m_m = m;
        m_kroneckerMatrices = GenerateKroneckerMatrices();
    }

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

    public Vector Decode(Vector encodedMessage)
    {
        for (int i = 0; i < encodedMessage.Length; ++i)
            if (encodedMessage[i] == 0)
                encodedMessage[i] = -1;

        for (int i = 0; i < m_m; ++i)
            encodedMessage = encodedMessage * m_kroneckerMatrices[i];

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

        Vector decodedMessage = new Vector(m_m + 1);
        if (encodedMessage[largestIndex] > 0)
            decodedMessage[0] = 1;

        char[] indexBinary = Convert.ToString(largestIndex, 2).PadLeft(m_m, '0').ToCharArray();
        Array.Reverse(indexBinary);
        string mirroredBinaryRepresentation = new string(indexBinary);

        for (int i = 1; i < decodedMessage.Length; ++i)
            decodedMessage[i] = (int)Char.GetNumericValue(mirroredBinaryRepresentation[i - 1]);

        return decodedMessage;
    }

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
