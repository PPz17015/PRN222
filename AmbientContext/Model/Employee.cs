using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodInjection.Model
{
    public class Employee
    {
        public int empId { get; set; }
        public string empName { get; set; }
        public IDepartment _employeeDept { get; set; }
        public Employee()
        {

        }
        public Employee(int id, string name)
        {
            empId = id;
            empName = name;
        }
        public void AssignDepartment(IDepartment department)
        {
            _employeeDept = department ?? throw new ArgumentNullException(nameof(department), "Department cannot be null.");
        }


        public override string ToString()
        {
            return $"Employee ID: {empId}, Name: {empName}, Department: {_employeeDept?.deptName ?? "Not Assigned"}";
        }
    }
}
