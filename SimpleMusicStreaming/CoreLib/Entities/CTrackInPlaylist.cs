using CoreLib.DataTableToObject.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Entities
{
    public class CTrackInPlaylist 
    {
        [DataNames("TrackId")]
        public int TrackId { get; set; }
        
        [DataNames("PlaylistId")]
        public int PlaylistId { get; set; }

    }
}
