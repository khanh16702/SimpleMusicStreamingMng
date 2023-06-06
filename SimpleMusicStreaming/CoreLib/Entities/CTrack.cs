using CoreLib.DataTableToObject.Attributes;
using CoreLib.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Entities
{
    public class CTrack : CBaseEntity
    {
        [DataNames("Name")]
        public string Name { get; set; }

        [DataNames("Image")]
        public string Image { get; set; }

        [DataNames("Duration")]
        public double Duration { get; set; }

        [DataNames("Stream")]
        public int Stream { get; set; }

        [DataNames("AlbumId")]
        public int AlbumId { get; set; }

        [DataNames("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [DataNames("CreatedBy")]
        public string CreatedBy { get; set; }

        [DataNames("IsActive")]
        public bool IsActive { get; set; }

        [DataNames("Media")]
        public string Media { get; set; }
    }
}
