﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Interfaces
{
    public interface IErrorHandler
    {
        void WriteToFile(Exception e);
    }
}
