using CodingTheory.Math;
using System.Collections;

namespace CodingTheory.ReedMuller.Tests;

internal class TestCases
{
    public static IEnumerable M3EncodedMessaged
    {
        get
        {
            yield return new TestCaseData(
                new Vector(new int[] { 1, 0, 1, 0, 1, 0, 1, 1 }),
                new Vector(new int[] { 1, 1, 0, 0})).SetName("10101011 -> 1100");
            
            yield return new TestCaseData(
                new Vector(new int[] { 1, 0, 0, 0, 1, 1, 1, 1 }),
                new Vector(new int[] { 0, 0, 0, 1 })).SetName("10001111 -> 0001");
        }
    }
}
