using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radabite.Backend.Database
{
    public class DataObject
    {
        [Key]
        public long Id { get; set; }
    }
}
