using CommonLib.Interfaces;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Implementations
{
    public class CSerilog : ISerilogProvider
    {
        private readonly ILogger _logger;
        public ILogger Logger { get { return this._logger; } }
        public CSerilog(IConfiguration configuration, ILogger logger)
        {
            if (logger == null)
            {
                if (configuration != null)
                {
                    var serilog = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();
                    this._logger = serilog;
                }
            }
            else
            {
                this._logger = logger;
            }

        }
    }
}
