using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProperyInjection.Model
{
  public class Employee     {
        public int empId { get; set; }
        public string empName { get; set; }
        public IDepartment _employeeDept { get; set; }
        public Employee(int id, string name)
        {
            empId = id;
            empName = name;
        }


        public IDepartment EmployeeDept
        {
            get
            {
                if (this._employeeDept == null)
                    this._employeeDept = new Engineering();
                return this._employeeDept;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Null.");
                _employeeDept = value;
            }
        }

        public override string ToString()
        {
           return $"Employee ID: {empId}, Name: {empName}, Department: {EmployeeDept.deptName}";
        }
    }
}
