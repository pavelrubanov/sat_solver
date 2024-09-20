namespace Tests;

using Solver;
public class Tests
{
    public List<string> SATFilesPaths { get; set; }
    public List<string> UNSATFilesPaths { get; set; }
    [SetUp]
    public void Setup()
    {
        string testDataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData");
        SATFilesPaths = Directory.GetFiles(testDataDirectory, "uf*").ToList();
        UNSATFilesPaths = Directory.GetFiles(testDataDirectory, "aim*").ToList();
    }

    [Test]
    public void SATTests()
    {
        foreach (var answer in SATFilesPaths.Select(path => Solver.Solve(path)))
        {
            Assert.That(answer[0], Is.EqualTo("SAT"));
        }
    }

    [Test]
    public void UNSATTests()
    {
        foreach (var answer in UNSATFilesPaths.Select(path => Solver.Solve(path)))
        {
            Assert.That(answer[0], Is.EqualTo("UNSAT"));
        }
    }
}