using CodingTheory.ReedMullerCodesBenchmark;

float errorProbability;
int experimentCount;
int maxM;

// First argument - error probability
if (!float.TryParse(args[0], out errorProbability))
{
    Console.WriteLine("First argument must be a float number - error probability");
    Console.Read();
    return;
}

// Second argument - number of experiments
if (!int.TryParse(args[1], out experimentCount))
{
    Console.WriteLine("Second argument must be an integer number - number of experiments");
    Console.Read();
    return;
}

// Third argument - max m parameter
if (!int.TryParse(args[2], out maxM))
{
    Console.WriteLine("Third argument must be an integer number - max M parameter");
    Console.Read();
    return;
}

Random random = new Random();
for (int m = 2; m < maxM; ++m)
{
    Benchmark benchmark = new Benchmark(m, errorProbability, random);
    ExperimentResult result = benchmark.Experiment(experimentCount);
    Console.WriteLine($"m = {m}, efficiency = {result.Efficiency}, average error count = {result.AverageErrorCount}");
}