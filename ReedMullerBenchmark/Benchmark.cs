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
        Vector initialVector = ConstructRandomVector(m_encoder.RequiredMessageLength);
        Vector encodedVector = m_encoder.Encode(initialVector);
        float overallEfficiency = 0.0f;
        float averageErrorCount = 0.0f;

        for (int i = 0; i < experimentCount; ++i)
        {
            Vector distortedVector = m_channel.PassThrough(encodedVector);
            Vector decodedVector = m_decoder.Decode(distortedVector);

            int errorCount = CountErrors(initialVector, decodedVector);
            float efficiency = 1.0f - (float)errorCount / initialVector.Length;
            averageErrorCount += errorCount;
            overallEfficiency += efficiency;

            // Console.WriteLine($"[{i}]".PadRight(experimentCount.ToString().Length + 2) + $"{initialVector} -> {decodedVector}, ({errorCount} errors, {efficiency * 100}% efficiency)");
        }

        overallEfficiency = (float)System.Math.Round(overallEfficiency / experimentCount, 3);
        averageErrorCount = (float)System.Math.Round(averageErrorCount / experimentCount, 3);
        ExperimentResult result = new ExperimentResult(overallEfficiency, averageErrorCount);

        return result;
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
