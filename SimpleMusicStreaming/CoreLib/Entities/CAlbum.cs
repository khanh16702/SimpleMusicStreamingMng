using CoreLib.DataTableToObject.Attributes;
using CoreLib.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Entities
{
    public class CAlbum : CBaseEntity
    {
        [DataNames("Name")]
        public string Name { get; set; }

        [DataNames("Image")]
        public string Image { get; set; }

        [DataNames("ArtistId")]
        public int ArtistId { get; set; }

        [DataNames("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [DataNames("IsActive")]
        public bool IsActive { get; set; }
    }
}
