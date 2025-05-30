

using ProperyInjection.Model;

class Program
{
    static void Main(string[] args)
    {
        Employee emp = new Employee(1, "Jane Doe")
        {
            EmployeeDept = new Engineering()
        };
        Employee emp2 = new Employee(2, "Sun Bringer")
        {
            EmployeeDept = new Marketing()
        };
        Console.WriteLine(emp);
        Console.WriteLine(new String('-', 50));
        Console.WriteLine(emp2);
        Console.ReadLine();
    }
}