

using MethodInjection.Model;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Authentication.ExtendedProtection;
using static AmbientContext.Model.Provider;
using static AmbientContext.Model.Provider.DepartmentProvider;

class Program
{
    static void Main(String[] args)
    {
        var serviceProvider = new ServiceCollection()
           .AddTransient<Employee>()
          .AddTransient<MarketingDepartmentProvider>()
          .AddTransient<DefaultDepartmentProvider>()
          .BuildServiceProvider();

        DepartmentProvider.Current = serviceProvider.GetService<MarketingDepartmentProvider>();
        Employee emp1 = serviceProvider.GetService<Employee>();
        emp1.empId= 1;
        emp1.empName = "Jane Doe";
        emp1._employeeDept = DepartmentProvider.Current.Department;
        Employee emp2= serviceProvider.GetService<Employee>();
        emp2.empId = 2;
        emp2.empName = "Sun Bringer";
        emp2._employeeDept = DepartmentProvider.Current.Department;
        Console.WriteLine(emp1);
        Console.WriteLine(new String('-', 50));
        Console.WriteLine(emp2);
        Console.ReadKey();

    }
}