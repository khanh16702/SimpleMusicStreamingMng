using CoreLib.Entities;

namespace SimpleMusicStreaming.Areas.Admin.Models
{
    public class CTrackViewModel : CTrack
    {
        public string ArtistName { get; set; }
        public string AlbumName { get; set; }
        public string GenreName { get; set; }
    }
}
