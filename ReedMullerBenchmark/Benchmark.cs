using CodingTheory.Channels;
using CodingTheory.Math;
using CodingTheory.ReedMuller;

namespace CodingTheory.ReedMullerCodesBenchmark;

public class Benchmark
{
    private readonly ReedMullerEncoder m_encoder;
    private readonly ReedMullerDecoder m_decoder;
    private readonly Channel m_channel;
    private readonly Random m_random;

    public Benchmark(int m, float errorProbability, Random random)
    {
        m_encoder = new ReedMullerEncoder((byte)m);
        m_decoder = new ReedMullerDecoder(m);

        m_random = random;
        m_channel = new Channel(errorProbability, m_random);
    }

    public ExperimentResult Experiment(int experimentCount)
    {
        int messageLength = m_encoder.RequiredMessageLength;
        int totalBits = experimentCount * messageLength;
        int totalErrors = 0;
        int frameErrors = 0;

        for (int i = 0; i < experimentCount; ++i)
        {
            Vector message = ConstructRandomVector(messageLength);
            Vector encoded = m_encoder.Encode(message);
            Vector noisy = m_channel.PassThrough(encoded);
            Vector decoded = m_decoder.Decode(noisy);

            int errors = CountErrors(message, decoded);
            totalErrors += errors;
            if (errors > 0) frameErrors++;
        }

        float bitErrorRate = (float)System.Math.Round((float)totalErrors / totalBits, 3);
        float frameErrorRate = (float)System.Math.Round((float)frameErrors / experimentCount, 3);
        float efficiency = (float)System.Math.Round(1.0f - bitErrorRate, 3);

        return new ExperimentResult(efficiency, bitErrorRate, frameErrorRate);
    }

    private Vector ConstructRandomVector(int length)
    {
        int[] elements = new int[length];
        for (int i = 0; i < length; ++i)
            elements[i] = m_random.Next(2);
        return new Vector(elements);
    }

    private int CountErrors(Vector v1, Vector v2)
    {
        int errorCount = 0;
        for (int i = 0; i < v1.Length; ++i)
        {
            if (v1[i] != v2[i])
                ++errorCount;
        }
        return errorCount;
    }
}
