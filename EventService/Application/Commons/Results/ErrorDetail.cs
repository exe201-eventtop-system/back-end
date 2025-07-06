using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons.Results
{
    /// <summary>
    ///     The detailed record of an error, this can be handy when there are multiple validation check.
    /// </summary>
    /// <param name="Summary">The summary of this error</param>
    /// <param name="Detail">The detail of the error</param>
    /// <param name="ErrorValue">The value that caused the error</param>
    /// <param name="ErrorType">The type of the error</param>
    public sealed record ErrorDetail(string Summary, string Detail, object? ErrorValue, string ErrorType)
    {
        public object GetValue()
        {
            return ErrorValue;
        }

        public T GetValue<T>()
        {
            return (T) ErrorValue;
        }

        public override string ToString()
        {
            return Summary;
        }
    }
}
