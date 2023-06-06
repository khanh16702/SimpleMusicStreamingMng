using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Entities
{
    public class CResponseMessage
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public CResponseMessage() { }

        public CResponseMessage(string code, string message)
        {
            Code = code;
            Message = message;
        }   
    }
}
