using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;

class program
{

    private static bool isPrime(int n)
    {
        if (n <= 1) return false;
        for (int i = 2; i <= Math.Sqrt(n); i++)
        {
            if (n % i == 0) return false;
        }
        return true;
    }
    private static IList<int> GetPriceList(IList<int> list)=> list.Where(isPrime).ToList();

    private static IList<int> GetPriceListParallel(IList<int> list)    {
       var primeNumbers = new ConcurrentBag<int>(); 
        Parallel.ForEach(list, number =>
        {
            if (isPrime(number))
            {
                primeNumbers.Add(number);
            }
        });
        return primeNumbers.ToList();
    }
    static void Main()
    {
        var limit= 2_000_000;
        var list= Enumerable.Range(1, limit).ToList();
        var watch = Stopwatch.StartNew();

        var primeNumberfromforeach = GetPriceList(list);
        watch.Stop();
        var watchforParallel = Stopwatch.StartNew();
        var primeNumberfromParallel = GetPriceListParallel(list);
        watchforParallel.Stop();
        Console.WriteLine($"Sequential: Total items are {primeNumberfromforeach.Count} and time taken is {watch.ElapsedMilliseconds} ms");
        Console.WriteLine($"Parallel: Total items are {primeNumberfromParallel.Count} and time taken is {watchforParallel.ElapsedMilliseconds} ms");
        Console.WriteLine("Press anykey to exit!");
        Console.ReadKey();


    }


}