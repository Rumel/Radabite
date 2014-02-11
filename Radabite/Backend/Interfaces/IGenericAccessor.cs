using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radabite.Backend.Database;

namespace Radabite.Backend.Interfaces
{
    public interface IGenericAccessor<T> where T : DataObject
    {
        SaveResult<T> Save(T t);

        T GetById(long id);

        IEnumerable<T> GetAll();
    }
}
