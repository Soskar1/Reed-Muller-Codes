using CodingTheory.Math;

namespace CodingTheory.Channel;

public class Channel
{
    private readonly float m_errorProbability;
    private readonly Random m_random;

    public Channel(float errorProbability)
    {
        if (errorProbability < 0.0 || errorProbability > 1.0)
            throw new ArgumentException("Error probability must be between 0 and 1.", nameof(errorProbability));
        
        m_errorProbability = errorProbability;
        m_random = new Random();
    }

    public Vector PassThrough(Vector data)
    {
        int[] transmittedData = new int[data.Length];

        for (int i = 0; i< data.Length; ++i)
        {
            if (m_random.NextDouble() < m_errorProbability)
                transmittedData[i] = data[i] ^ 1;
            else
                transmittedData[i] = data[i];
        }

        return new Vector(transmittedData);
    }
}
