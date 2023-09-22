using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.SeedWork
{
    public class ApiSuccessResult<T> : ApiResult<T>
    {
        public ApiSuccessResult(T Data) : base(true, Data, "Success") 
        { }

        public ApiSuccessResult(T Data, string message) : base(true, Data, message)
        { }
    }
}
