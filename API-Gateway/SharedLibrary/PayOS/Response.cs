using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.PayOS
{
    public record Response(
    int error,
    String message,
    object? data
);
}
