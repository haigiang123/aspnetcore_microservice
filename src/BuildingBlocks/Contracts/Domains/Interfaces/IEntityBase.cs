using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Domains.Interfaces
{
    public class IEntityBase<T>
    {
        public T Id { get; set; }
    }
}
