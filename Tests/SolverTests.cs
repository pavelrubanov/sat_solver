namespace Tests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Solver;

[TestFixture]
public class SolverTests
{
    public static IEnumerable<TestCaseData> SATFilesSource()
    {
        string testDataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData");
        foreach (var filePath in Directory.GetFiles(testDataDirectory, "uf*"))
        {
            string fileName = Path.GetFileName(filePath);
            yield return new TestCaseData(filePath).SetName($"SATTest_{fileName}");
        }
    }

    public static IEnumerable<TestCaseData> UNSATFilesSource()
    {
        string testDataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData");
        foreach (var filePath in Directory.GetFiles(testDataDirectory, "aim*"))
        {
            string fileName = Path.GetFileName(filePath);
            yield return new TestCaseData(filePath).SetName($"UNSATTest_{fileName}");
        }
    }

    [TestCaseSource(nameof(SATFilesSource))]
    public void SATTest(string filePath)
    {
        var answer = Solver.Solve(filePath);
        Assert.That(answer[0], Is.EqualTo("SAT"), $"Failed on file {Path.GetFileName(filePath)}");
    }

    [TestCaseSource(nameof(UNSATFilesSource))]
    public void UNSATTest(string filePath)
    {
        var answer = Solver.Solve(filePath);
        Assert.That(answer[0], Is.EqualTo("UNSAT"), $"Failed on file {Path.GetFileName(filePath)}");
    }
}
