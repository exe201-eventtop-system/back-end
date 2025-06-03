using Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SystemLogger
{
    public class SystemLoggerProvider: ILoggerProvider
    {
        public readonly ProductServiceDbContext _context;

        public SystemLoggerProvider(ProductServiceDbContext context) => _context = context;

        public ILogger CreateLogger(string categoryName)
        {
            return new SystemLogger(this);
        }

        public void Dispose()
        {

        }
    }
}
