namespace Solver;

public class Solver
{
    private static List<int>? Solve(List<List<int>> clauses, List<int> answer, int trueLiteral = 0)
    {
        if (trueLiteral != 0)
        {
            answer.Add(trueLiteral);
            clauses.RemoveAll(c => c.Contains(trueLiteral)); // удаляем клозы которые стали истинны
            clauses.ForEach(c => c.RemoveAll(x => x == -trueLiteral)); // удаляем из клоз ложный литерал
        }
        
        if (clauses.Any(c => c.Count == 0))
        {
            return null;
        }

        if (clauses.Count == 0)
        {
            return answer;
        }
        
        // Правило единственной клаузулы
        clauses
            .Where(c => c.Count == 1)
            .Select(c => c[0])
            .ToList() // одиночные литералы
            .ForEach(l =>
            {
                answer.Add(l);
                clauses.RemoveAll(c => c.Contains(l)); // удаляем клозы которые стали истинны
                clauses.ForEach(c => c.RemoveAll(x => x == -l)); // удаляем из клоз ложный литерал
            });

         // Исключение «чистых» переменных
         clauses
             .SelectMany(c => c)
             .Distinct()
             .ToList()
             .ForEach(l =>
             {
                 if (!clauses.Any(c => c.Contains(-l)))
                 {
                     answer.Add(l);
                     clauses.RemoveAll(c => c.Contains(l)); // удаляем клозы которые стали истинны
                 }
             });
         
         if (clauses.Any(c => c.Count == 0))
         {
             return null;
         }
         
         if (clauses.Count == 0)
         {
             return answer;
         }
         
         var chosenLiteral = clauses.First().First();
         return Solve(clauses.Clone(), answer.Clone(), chosenLiteral) 
                ?? Solve(clauses.Clone(), answer.Clone(), -chosenLiteral);
    }

    public static List<string> Solve(string filePath)
    {
        var file = File.ReadAllLines(filePath);
        var clauses = new List<List<int>>();
        foreach (var line in file)
        {
            if (line.StartsWith("c") || line.StartsWith("p")) continue;
            var clause = line
                .Split()
                .Where(c => !string.IsNullOrEmpty(c) && c != "0")
                .Select(int.Parse)
                .ToList();
            
            clauses.Add(clause);
        }
        
        var answer = Solve(clauses, new List<int>());
        if (answer == null)
        {
            return new List<string>(){"UNSAT"};
        }
        
        var result = new List<string>(){"SAT"};
        result.AddRange(answer.Select(i => i.ToString()));
        return result;
    }
    
    public static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Некорректный ввод. Передайте только путь к файлу");
            return;
        }
        
        var path = args[0];
        var result = Solve(path);
        Console.WriteLine(string.Join(" ", result));
    }
}
