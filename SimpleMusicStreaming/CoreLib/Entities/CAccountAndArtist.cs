using CoreLib.DataTableToObject.Attributes;
using CoreLib.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Entities
{
    public class CAccountAndArtist
    {
        [DataNames("AccountId")]
        public int AccountId { get; set; }

        [DataNames("ArtistId")]
        public int ArtistId { get; set; }
    }
}
