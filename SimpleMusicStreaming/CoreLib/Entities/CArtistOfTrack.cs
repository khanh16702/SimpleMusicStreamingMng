using CoreLib.DataTableToObject.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Entities
{
    public class CArtistOfTrack
    {
        [DataNames("ArtistId")]
        public int ArtistId { get; set; }

        [DataNames("TrackId")]
        public int TrackId { get; set; }
    }
}
