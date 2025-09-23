using System.Collections;

namespace CodingTheory.Math.Tests;

internal class TestCases
{
    private static readonly Matrix H = new Matrix(new int[,] {
        { 1, 1 },
        { 1, -1 }});

    private static readonly Matrix I1 = new Matrix(new int[,] { { 1 } });

    private static readonly Matrix I2 = new Matrix(new int[,]{
        { 1, 0 },
        { 0, 1 }});

    private static readonly Matrix I4 = new Matrix(new int[,]
    {
        { 1, 0, 0, 0 },
        { 0, 1, 0, 0 },
        { 0, 0, 1, 0 },
        { 0, 0, 0, 1 }
    });

    private static readonly Matrix A3x2 = new Matrix(new int[,]
    {
        { 1, 2 },
        { 3, 4 },
        { 1, 0 }
    });

    private static readonly Matrix B2x3 = new Matrix(new int[,]
    {
        { 0, 5, 2 },
        { 6, 7, 3 }
    });

    public static IEnumerable KroneckerProductTestCases
    {
        get
        {
            yield return new TestCaseData(H, I2, new Matrix(new int[,]
                {
                    { 1, 0, 1, 0 },
                    { 0, 1, 0, 1 },
                    { 1, 0, -1, 0 },
                    { 0, 1, 0, -1 }
                }))
                .SetName("H x I2");

            yield return new TestCaseData(I2, H, new Matrix(new int[,]
                {
                    { 1, 1, 0, 0 },
                    { 1, -1, 0, 0 },
                    { 0, 0, 1, 1 },
                    { 0, 0, 1, -1 }
                }))
                .SetName("I2 x H");

            yield return new TestCaseData(I4, H, new Matrix(new int[,]
                {
                    { 1, 1, 0, 0, 0, 0, 0, 0 },
                    { 1, -1, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 1, 1, 0, 0, 0, 0 },
                    { 0, 0, 1, -1, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 1, 1, 0, 0 },
                    { 0, 0, 0, 0, 1, -1, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 1, 1 },
                    { 0, 0, 0, 0, 0, 0, 1, -1 },
                }))
                .SetName("I4 x H");

            yield return new TestCaseData(A3x2, B2x3, new Matrix(new int[,]
                {
                    { 0, 5, 2, 0, 10, 4 },
                    { 6, 7, 3, 12, 14, 6 },
                    { 0, 15, 6, 0, 20, 8 },
                    { 18, 21, 9, 24, 28, 12 },
                    { 0, 5, 2, 0, 0, 0 },
                    { 6, 7, 3, 0, 0, 0 },
                }))
                .SetName("A3x2, B2x3");
        }
    }
    
    public static IEnumerable IdentityMatrixGenerationTestCases
    {
        get
        {
            yield return new TestCaseData(1, I1).SetName("IdentityMatrix size 1 generation");
            yield return new TestCaseData(2, I2).SetName("IdentityMatrix size 2 generation");
            yield return new TestCaseData(4, I4).SetName("IdentityMatrix size 4 generation");
        }
    }
}
