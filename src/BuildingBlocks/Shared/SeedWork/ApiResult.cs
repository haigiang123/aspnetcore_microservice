using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.SeedWork
{
    public class ApiResult<T>
    {
        public bool IsSucceeded { get; set; }
        public string Message { get; set; }
        public T Data { get; }

        public ApiResult() { }

        public ApiResult(bool isSucceeded, string message = "")
        {
            IsSucceeded = isSucceeded;
            Message = message;
        }

        public ApiResult(bool isSucceeded, T data, string message = "")
        {
            IsSucceeded = isSucceeded;
            Message = message;
            Data = data;
        }
    }
}
