


class program
{

    static void PrintNumbes(String message)
    {

        for (int i = 0; i <=10; i++)
        {
            Console.WriteLine($"{message}:{i}");
            Thread.Sleep(1000);
        }
    }


    static void Main(string[] args)
    {
        Thread.CurrentThread.Name = "Main Thread";
        Task task1 = new Task(() => PrintNumbes("Task 1"));
        task1.Start();
        Task task2 = Task.Run(delegate
        {
            PrintNumbes("Task 2");
        });

        Task task3 = new Task(new Action(() =>
        {
            PrintNumbes("Task 3");
        }));
        task3.Start();
        Console.WriteLine($"Main Thread: {Thread.CurrentThread.Name}");
        Console.ReadKey();

    }
}