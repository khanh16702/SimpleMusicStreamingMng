using CoreLib.DataTableToObject.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Entities
{
    public class CGenreOfTrack
    {
        [DataNames("GenreId")]
        public int GenreId { get; set; }

        [DataNames("TrackId")]
        public int TrackId { get; set; }
    }
}
