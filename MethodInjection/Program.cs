


using Microsoft.Extensions.DependencyInjection;
using MethodInjection.Model;

class Program
{
    static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddTransient<IDepartment, Marketing>()
            .AddTransient<Employee>() 
            .BuildServiceProvider();
        Employee emp1 = serviceProvider.GetService<Employee>();
        emp1.empId = 1;
        emp1.empName = "Jane Doe";
      emp1.AssignDepartment(serviceProvider.GetService<IDepartment>());
       
        Employee emp2 = serviceProvider.GetService<Employee>();
        emp2.empId = 2;
        emp2.empName = "Sun Bringer";
        emp2.AssignDepartment(serviceProvider.GetService<IDepartment>());
        
        Console.WriteLine(emp1);
        Console.WriteLine(new String('-', 50));
        Console.WriteLine(emp2);
        Console.ReadLine();
    }
}