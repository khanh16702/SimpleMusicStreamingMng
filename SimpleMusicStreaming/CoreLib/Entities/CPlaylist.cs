using CoreLib.DataTableToObject.Attributes;
using CoreLib.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Entities
{
    public class CPlaylist : CBaseEntity
    {
        [DataNames("AccountId")]
        public int AccountId { get; set; }

        [DataNames("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [DataNames("UpdatedDate")]
        public DateTime UpdatedDate { get; set; }

        [DataNames("Name")]
        public string Name { get; set; }
    }
}
