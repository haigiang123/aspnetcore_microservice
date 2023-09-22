using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace Ordering.Application.Common.Exceptions
{
    public class ValidationException : Exception
    {

        public IDictionary<string, string[]> Errors { get; }

        public ValidationException() : base("One or more validation failures have occurred")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            Errors = failures
                        .GroupBy(x => x.PropertyName, e => e.ErrorMessage)
                        .ToDictionary(x => x.Key, x => x.ToArray());
        }
    }
}
