

class Program { 
public static void Main()
{
        var range=Enumerable.Range(1, 1000_000);
        var result = range.Where(i =>i%3==0).ToList();

        Console.WriteLine($"Sequential: Total items are {result.Count}");
         result = range.AsParallel()
            .Where(i => i % 3 == 0)
            .ToList();
        Console.WriteLine($"Parallel: Total items are {result.Count}");
        result=(from i in range.AsParallel()
                where i % 3 == 0
                select i).ToList();
        Console.WriteLine($"Parallel LINQ: Total items are {result.Count}");
        Console.ReadKey();

    }

}