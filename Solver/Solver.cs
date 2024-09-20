namespace Solver;

public class Solver
{
    public static List<int>? Solve(List<List<int>> clauses, List<int> answer)
    {
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
             .ToList()
             .ForEach(l =>
             {
                 if (!clauses.Any(c => c.Contains(-l)))
                 {
                     answer.Add(l);
                     clauses.RemoveAll(c => c.Contains(l)); // удаляем клозы которые стали истинны
                     clauses.ForEach(c => c.RemoveAll(x => x == -l)); // удаляем из клоз ложный литерал
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
         answer.Add(chosenLiteral);
         clauses.RemoveAll(c => c.Contains(chosenLiteral)); // удаляем клозы которые стали истинны
         clauses.ForEach(c => c.RemoveAll(x => x == -chosenLiteral)); // удаляем из клоз ложный литерал
         var solveResult = Solve(clauses.Clone(), answer.Clone());
         
         if (solveResult == null)
         {
             answer.Remove(chosenLiteral);
             answer.Add(-chosenLiteral);
             clauses.RemoveAll(c => c.Contains(-chosenLiteral)); // удаляем клозы которые стали истинны
             clauses.ForEach(c => c.RemoveAll(x => x == chosenLiteral)); // удаляем из клоз ложный литерал
             return Solve(clauses.Clone(), answer.Clone());
         }

         return solveResult;
    }
    
    public static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Некорректный ввод. Передайте только путь к файлу");
            return;
        }
        
        var path = args[0];
        var file = File.ReadAllLines(path);
        var clauses = new List<List<int>>();
        foreach (var line in file)
        {
            if (line.StartsWith("c") || line.StartsWith("p")) continue;
            var clause = line.Split(" ").Select(int.Parse).ToList();
            clauses.Add(clause);
        }
        
        var result = Solve(clauses, new List<int>());
        if (result == null)
        {
            Console.WriteLine("UNSAT");
        }
        else
        {
            Console.WriteLine("SAT");
            foreach (var i in result)
                Console.Write(i + " ");
        }
    }
}
