using System.Runtime.InteropServices;

class Program
{

    public static async Task<int> method1()
    {
        int count = 0;
        await Task.Run(() =>
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Method 1. ");
                count++;
            }
        });
        return count;
    }

    public static void method2()
    {
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine("Method 2. ");
        }
    }
    public static void method3(int count)
    {
        Console.WriteLine("Method 3 is called. ");
        Console.WriteLine($"Count is {count}");
    }
    public static async Task callMethod()
    {
        method2();
        var count= await method1();
        method3(count);
    }

    static async Task Main(string[] args)
    {
        await callMethod();
        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }
}