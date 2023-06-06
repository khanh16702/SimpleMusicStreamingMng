using CoreLib.DataTableToObject.Attributes;
using CoreLib.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Entities
{
    public class CBanner : CBaseEntity
    {
        [DataNames("Title")]
        public string Title { get; set; }

        [DataNames("Content")]
        public string Content { get; set; }
    }
}
