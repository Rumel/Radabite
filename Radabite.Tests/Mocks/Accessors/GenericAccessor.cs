using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radabite.Backend.Database;
using Radabite.Backend.Interfaces;

namespace Radabite.Tests.Mocks.Accessors
{
    public class GenericAccessor<T> : IGenericAccessor<T> where T : DataObject
    {
        private int _count = 1;
        private List<T> itemsList;

        public GenericAccessor()
        {
            itemsList = new List<T>();
        }
    
        public SaveResult<T> Save(T t)
        {
            t.Id = _count++;
            itemsList.Add(t);
            return new SaveResult<T>(true, t);
        }

        public T GetById(long id)
        {
            return itemsList.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<T> GetAll()
        {
            return itemsList;
        }
    }
}
