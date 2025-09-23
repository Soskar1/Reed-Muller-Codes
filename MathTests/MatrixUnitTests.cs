namespace CodingTheory.Math.Tests;

[TestFixture]
internal class MatrixUnitTests
{
    [Test]
    public void MatrixInitialization_ValidParameters_MatrixCreated()
    {
        Matrix matrix = new Matrix(new int[,] { { 1, 2 }, { 3, 4 } });
        Assert.That(matrix[0, 0], Is.EqualTo(1));
        Assert.That(matrix[0, 1], Is.EqualTo(2));
        Assert.That(matrix[1, 0], Is.EqualTo(3));
        Assert.That(matrix[1, 1], Is.EqualTo(4));
    }

    [Test]
    public void MatrixInitialization_InvalidParameters_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => new Matrix(new int[,] { }));
    }

    [Test]
    public void MatrixInitialization_PassRowsAndColumns_MatrixCreated()
    {
        Matrix matrix = new Matrix(2, 3);
        Assert.That(matrix.Rows, Is.EqualTo(2));
        Assert.That(matrix.Columns, Is.EqualTo(3));

        foreach (int value in matrix)
            Assert.That(value, Is.EqualTo(0));
    }

    [Test]
    public void MatrixIndexing_ValidIndex_ReturnsCorrectValue()
    {
        Matrix matrix = new Matrix(new int[,] {
            { 1, 2 },
            { 3, 4 }
        });

        Assert.That(matrix[0, 0], Is.EqualTo(1));
        Assert.That(matrix[0, 1], Is.EqualTo(2));
        Assert.That(matrix[1, 0], Is.EqualTo(3));
        Assert.That(matrix[1, 1], Is.EqualTo(4));
    }

    [Test]
    public void MatrixIndexing_SetValue_UpdatesValue()
    {
        Matrix matrix = new Matrix(new int[,] {
            { 1, 2 },
            { 3, 4 }
        });
        matrix[0, 1] = 5;

        Assert.That(matrix[0, 0], Is.EqualTo(1));
        Assert.That(matrix[0, 1], Is.EqualTo(5));
        Assert.That(matrix[1, 0], Is.EqualTo(3));
        Assert.That(matrix[1, 1], Is.EqualTo(4));
    }

    [Test]
    public void MatrixDimensions_ReturnsCorrectDimensions()
    {
        Matrix matrix = new Matrix(new int[,] { { 1, 2, 3 }, { 4, 5, 6 } });
        Assert.That(matrix.Rows, Is.EqualTo(2));
        Assert.That(matrix.Columns, Is.EqualTo(3));
    }

    [Test]
    public void MatrixMultiplication_MultiplyMatrixByVector_ReturnsCorrectResult()
    {
        Matrix matrix = new Matrix(new int[,] {
            { 1, 2 },
            { 3, 4 }
        });

        Vector vector = new Vector(new int[] { 5, 6 });
        Vector result = matrix * vector;
        Assert.That(result[0], Is.EqualTo(17)); // 1*5 + 2*6
        Assert.That(result[1], Is.EqualTo(39)); // 3*5 + 4*6

        result = vector * matrix;
        Assert.That(result[0], Is.EqualTo(23)); // 5 + 6 * 3
        Assert.That(result[1], Is.EqualTo(34)); // 5 * 2 + 6 * 4
    }

    [Test]
    public void MatrixMultiplication_MultiplyIncompatibleSizes_ThrowsException()
    {
        Matrix matrix = new Matrix(new int[,] {
            { 1, 2 },
            { 3, 4 }
        });

        Vector vector = new Vector(new int[] { 5, 6, 7 });
        Assert.Throws<ArgumentException>(() => { var result = matrix * vector; });
        Assert.Throws<ArgumentException>(() => { var result = vector * matrix; });
    }

    [Test]
    public void MatrixMultiplication_MultiplyMatrixByMatrix_ReturnsMatrix()
    {
        Matrix m1 = new Matrix(new int[,] {
            { 1, 2 },
            { 3, 4 }
        });

        Matrix m2 = new Matrix(new int[,] {
            { 3, 2 },
            { 1, 5 }
        });

        Matrix result = m1 * m2;

        Assert.That(result.Rows, Is.EqualTo(2));
        Assert.That(result.Columns, Is.EqualTo(2));
        Assert.That(result[0, 0], Is.EqualTo(5));  // 1*3 + 2*1
        Assert.That(result[0, 1], Is.EqualTo(12)); // 1*2 + 2*5
        Assert.That(result[1, 0], Is.EqualTo(13)); // 3*3 + 4*1
        Assert.That(result[1, 1], Is.EqualTo(26)); // 3*2 + 4*5

        result = m2 * m1;
        Assert.That(result.Rows, Is.EqualTo(2));
        Assert.That(result.Columns, Is.EqualTo(2));
        Assert.That(result[0, 0], Is.EqualTo(9)); // 3*1 + 2*3
        Assert.That(result[0, 1], Is.EqualTo(14)); // 3*2 + 2*4
        Assert.That(result[1, 0], Is.EqualTo(16));  // 1*1 + 5*3
        Assert.That(result[1, 1], Is.EqualTo(22)); // 1*2 + 5*4
    }

    [Test]
    public void MatrixMultiplication_MultiplyIncompatibleMatrices_ThrowsException()
    {
        Matrix m1 = new Matrix(new int[,] {
            { 1, 2, 3 },
            { 3, 4, 5 }
        });
        Matrix m2 = new Matrix(new int[,] {
            { 3, 2, 1 },
            { 1, 5, 6 }
        });
        Assert.Throws<ArgumentException>(() => { var result = m1 * m2; });

        m1 = new Matrix(new int[,]
        {
            { 1 }
        });
        Assert.Throws<ArgumentException>(() => { var result = m1 * m2; });
    }

    [Test]
    public void MatrixGetRow_ReturnsCorrectVector()
    {
        Matrix matrix = new Matrix(new int[,] {
            { 1, 2, 3 },
            { 4, 5, 6 }
        });

        Vector row0 = matrix.GetRow(0);
        Assert.That(row0.Length, Is.EqualTo(3));
        Assert.That(row0[0], Is.EqualTo(1));
        Assert.That(row0[1], Is.EqualTo(2));
        Assert.That(row0[2], Is.EqualTo(3));

        Vector row1 = matrix.GetRow(1);
        Assert.That(row1.Length, Is.EqualTo(3));
        Assert.That(row1[0], Is.EqualTo(4));
        Assert.That(row1[1], Is.EqualTo(5));
        Assert.That(row1[2], Is.EqualTo(6));
    }

    [Test]
    public void MatrixGetColumn_ReturnsCorrectVector()
    {
        Matrix matrix = new Matrix(new int[,] {
            { 1, 2, 3 },
            { 4, 5, 6 }
        });

        Vector column0 = matrix.GetColumn(0);
        Assert.That(column0.Length, Is.EqualTo(matrix.Rows));
        Assert.That(column0[0], Is.EqualTo(1));
        Assert.That(column0[1], Is.EqualTo(4));

        Vector column1 = matrix.GetColumn(1);
        Assert.That(column1.Length, Is.EqualTo(matrix.Rows));
        Assert.That(column1[0], Is.EqualTo(2));
        Assert.That(column1[1], Is.EqualTo(5));

        Vector column2 = matrix.GetColumn(2);
        Assert.That(column2.Length, Is.EqualTo(matrix.Rows));
        Assert.That(column2[0], Is.EqualTo(3));
        Assert.That(column2[1], Is.EqualTo(6));
    }

    [Test]
    public void MatrixForEach_IteratesOverAllElementsInRowMajorOrder()
    {
        Matrix matrix = new Matrix(new int[,] {
            { 1, 2, 3 },
            { 4, 5, 6 }
        });

        List<int> elements = new List<int>();
        foreach (int value in matrix)
            elements.Add(value);

        Assert.That(elements.Count, Is.EqualTo(6));
        Assert.That(elements[0], Is.EqualTo(1));
        Assert.That(elements[1], Is.EqualTo(2));
        Assert.That(elements[2], Is.EqualTo(3));
        Assert.That(elements[3], Is.EqualTo(4));
        Assert.That(elements[4], Is.EqualTo(5));
        Assert.That(elements[5], Is.EqualTo(6));
    }

    [Test]
    public void MatrixToMod2_ReturnsMatrixMod2()
    {
        Matrix matrix = new Matrix(new int[,] {
            { 1, 2, 3 },
            { 4, 5, 6 }
        });

        MatrixMod2 result = matrix.ToMod2();

        Assert.That(result.Rows, Is.EqualTo(2));
        Assert.That(result.Columns, Is.EqualTo(3));

        Assert.That(result[0, 0], Is.EqualTo(1));
        Assert.That(result[0, 1], Is.EqualTo(0));
        Assert.That(result[0, 2], Is.EqualTo(1));
        Assert.That(result[1, 0], Is.EqualTo(0));
        Assert.That(result[1, 1], Is.EqualTo(1));
        Assert.That(result[1, 2], Is.EqualTo(0));
    }

    [Test]
    public void MatrixEquality_Equals_ReturnsTrueForIdenticalMatrices()
    {
        Matrix m1 = new Matrix(new int[,] {
            { 1, 2 },
            { 3, 4 }
        });
        Matrix m2 = new Matrix(new int[,] {
            { 1, 2 },
            { 3, 4 }
        });
        Assert.That(m1.Equals(m2), Is.True);
        Assert.That(m1.GetHashCode(), Is.EqualTo(m2.GetHashCode()));
    }

    [Test]
    public void MatrixEquality_Equals_ReturnsFalseForDifferentMatrices()
    {
        Matrix m1 = new Matrix(new int[,] {
            { 1, 2 },
            { 3, 4 }
        });
        Matrix m2 = new Matrix(new int[,] {
            { 1, 2 },
            { 3, 5 }
        });
        Matrix m3 = new Matrix(new int[,] {
            { 1, 2, 3 },
            { 4, 5, 6 }
        });

        Assert.That(m1.Equals(m2), Is.False);
        Assert.That(m1.GetHashCode(), Is.Not.EqualTo(m2.GetHashCode()));
        Assert.That(m1.Equals(m3), Is.False);
        Assert.That(m1.GetHashCode(), Is.Not.EqualTo(m3.GetHashCode()));
        Assert.That(m2.Equals(m3), Is.False);
        Assert.That(m2.GetHashCode(), Is.Not.EqualTo(m3.GetHashCode()));
    }

    [Test]
    public void MatrixEquality_EqualsNull_ReturnsFalse()
    {
        Matrix m1 = new Matrix(new int[,] {
            { 1, 2 },
            { 3, 4 }
        });
        Assert.That(m1.Equals(null), Is.False);
    }

    [Test]
    public void MatrixEquality_EqualsDifferentType_ReturnsFalse()
    {
        Matrix m1 = new Matrix(new int[,] {
            { 1, 2 },
            { 3, 4 }
        });
        Assert.That(m1.Equals("not a matrix"), Is.False);
    }

    [Test]
    public void MatrixEquality_SameReference_ReturnsTrue()
    {
        Matrix m1 = new Matrix(new int[,] {
            { 1, 2 },
            { 3, 4 }
        });
        Matrix m2 = m1;
        Assert.That(m1.Equals(m2), Is.True);
        Assert.That(m1.GetHashCode(), Is.EqualTo(m2.GetHashCode()));
    }

    [Test]
    public void MatrixEquality_EqualityOperator_ReturnsTrue()
    {
        Matrix m1 = new Matrix(new int[,] {
            { 1, 2 },
            { 3, 4 }
        });
        Matrix m2 = new Matrix(new int[,] {
            { 1, 2 },
            { 3, 4 }
        });
        Assert.That(m1 == m2);
    }

    [Test]
    public void MatrixEquality_InequalityOperator_ReturnsFalse()
    {
        Matrix m1 = new Matrix(new int[,] {
            { 1, 2 },
            { 3, 4 }
        });
        Matrix m2 = new Matrix(new int[,] {
            { 1, 2 },
            { 3, 5 }
        });
        Assert.That(m1 != m2);
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.KroneckerProductTestCases))]
    public void MatrixKroneckerProduct_ReturnsCorrectMatrix(Matrix m1, Matrix m2, Matrix expected)
    {
        Matrix result = m1.KroneckerProduct(m2);
        Assert.That(result, Is.EqualTo(expected), result.ToString());
    }
}
