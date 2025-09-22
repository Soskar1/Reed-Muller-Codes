using CodingTheory.Math;

namespace CodingTheory.ReedMuller;

public class ReedMullerEncoder
{
    private readonly MatrixMod2 m_generatorMatrix;

    // (1, m) Reed-Muller code
    public ReedMullerEncoder(int m)
    {
        if (m < 2)
            throw new ArgumentException("Parameter m must be at least 2.", nameof(m));

        m_generatorMatrix = GeneratorMatrixBuilder.Build(m);
    }

    public Vector Encode(Vector message) => message * m_generatorMatrix;
}
