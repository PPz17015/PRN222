using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProperyInjection.Model
{
    public interface IDepartment
    {
        int deptId { get; set; }
        String deptName { get; set; }
    }
    public class Department : IDepartment
    {
        public int deptId { get; set; }
        public string deptName { get; set; }

    }
    public class Engineering : Department

    {
        public Engineering()
        {
            deptName = "Engineering";
        }

    }
    public class Marketing : Department
    {
        public Marketing()
        {
            deptName = "Marketing";
        }
    }
}
