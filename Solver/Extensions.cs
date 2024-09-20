namespace Solver;

static class Extensions
{
    public static List<int> Clone(this List<int> source)
        => source.Select(item => item).ToList();
    
    public static List<List<int>> Clone(this List<List<int>> source)
        => source.Select(item => item.Clone()).ToList();
}