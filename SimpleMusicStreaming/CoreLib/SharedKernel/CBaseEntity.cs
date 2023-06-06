﻿using CoreLib.DataTableToObject.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.SharedKernel
{
    public class CBaseEntity
    {
        [DataNames("Id")]
        public int Id { get; set; }
    }
}
