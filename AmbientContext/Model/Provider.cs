using MethodInjection.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientContext.Model
{
    public class Provider
    {
        public abstract class DepartmentProvider
        {

            private static DepartmentProvider current;
            public static DepartmentProvider Current
            {
                get
                {
                    if (current == null)
                    { current = new DefaultDepartmentProvider(); }

                    return current;
                }
                set
                {
                    current = value ?? new DefaultDepartmentProvider();
                }

            }
            public virtual Department Department { get; }
            public class DefaultDepartmentProvider : DepartmentProvider
            {
                public override Department Department => new Engineering();
            }
            public class MarketingDepartmentProvider : DepartmentProvider
            {
                public override Department Department => new Marketing();
            }
        }
    }

}