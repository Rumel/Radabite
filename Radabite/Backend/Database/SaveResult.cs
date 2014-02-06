using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Radabite.Backend.Database
{
    public class SaveResult<T>
    {
        public bool Success { get; set; }

        public T Result { get; set; }

        public SaveResult(bool success)
        {
            Success = success;
            Result = default(T);
        }

        public SaveResult(bool success, T result)
        {
            Success = success;
            Result = result;
        }
    }
}