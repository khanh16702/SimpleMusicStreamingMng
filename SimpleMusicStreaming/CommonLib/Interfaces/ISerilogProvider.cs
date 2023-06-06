using CoreLib.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Interfaces
{
    public interface ISerilogProvider : ILogProvider
    {
        ILogger Logger { get; }
    }
}
