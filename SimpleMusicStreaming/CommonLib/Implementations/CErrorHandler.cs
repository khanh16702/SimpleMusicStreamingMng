using CommonLib.Interfaces;
using CoreLib.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Implementations
{
    public class CErrorHandler : IErrorHandler
    {
        private readonly ILogger _logger;
        public ILogger Logger { get { return this._logger; } }
        public CErrorHandler(ISerilogProvider serilogProvider)
        {
            this._logger = serilogProvider.Logger;
        }
        public void WriteToFile(Exception e)
        {
            string template = "\r\n-----Message-----\r\n{0}\r\n-----Source ---\r\n{1}\r\n-----StackTrace ---\r\n{2}\r\n-----TargetSite ---\r\n{3}";
            this._logger.Error(template, e.Message, e.Source, e.StackTrace, e.TargetSite);
        }
    }
}
