using CodingTheory.Math;

namespace CodingTheory.Channels;

public class Channel
{
    private readonly float m_errorProbability;
    private readonly Random m_random;

    /// <summary>
    /// Channel constructor
    /// </summary>
    /// <param name="errorProbability">Value distortion probability in interval [0, 1]</param>
    /// <exception cref="ArgumentException"></exception>
    public Channel(float errorProbability)
    {
        if (errorProbability < 0.0 || errorProbability > 1.0)
            throw new ArgumentException("Error probability must be between 0 and 1.", nameof(errorProbability));

        m_errorProbability = errorProbability;
        m_random = new Random();
    }

    /// <summary>
    /// Pass through channel 1 vector
    /// </summary>
    /// <param name="data">Vector with mod 2 elements</param>
    /// <returns>Distorted Vector with mod 2 elements</returns>
    public Vector PassThrough(Vector data)
    {
        int[] transmittedData = new int[data.Length];

        // Iterating through all bits in vector
        for (int i = 0; i < data.Length; ++i)
        {
            // Distoring the bit with probability
            if (m_random.NextDouble() < m_errorProbability)
                transmittedData[i] = data[i] ^ 1;
            else
                transmittedData[i] = data[i];
        }

        return new Vector(transmittedData);
    }

    /// <summary>
    /// Byte array distortion
    /// </summary>
    /// <param name="data">Byte array</param>
    /// <returns>Distorted byte array</returns>
    public byte[] PassThrough(byte[] data)
    {
        byte[] distorted = new byte[data.Length];

        // Iterating through all bytes in array
        for (int i = 0; i < data.Length; ++i)
        {
            // Converting byte to vector
            Vector v = Vector.ByteToVector(data[i]);
            
            // Using Channel.PassThrough(Vector) to distort vector
            v = PassThrough(v);
            
            // Converting back to byte
            distorted[i] = (byte)v;
        }

        return distorted;
    }

    public Vector[] PassThrough(Vector[] data)
    {
        Vector[] distorted = new Vector[data.Length];

        for (int i = 0; i < data.Length; ++i)
            distorted[i] = PassThrough(data[i]);

        return distorted;
    }
}
