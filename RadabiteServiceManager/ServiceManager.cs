using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace RadabiteServiceManager
{
    public class ServiceManager
    {
        private static IKernel _kernel;
        public static IKernel Kernel
        {
            get
            {
                if (_kernel == null)
                {
                    _kernel = new StandardKernel();
                }
                return _kernel;
            }
        }
    }
}
