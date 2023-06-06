using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Config
{
    public class SPRoute
    {
        // Account
        public const string sp_Account_Insert = "sp_Account_Insert";
        public const string sp_Account_Delete = "sp_Account_Remove";
        public const string sp_Account_Update = "sp_Account_Update";
        public const string sp_Account_Search = "sp_Account_Search";

        // Album
        public const string sp_Album_Insert = "sp_Album_Insert";
        public const string sp_Album_Search = "sp_Album_Search";
        public const string sp_Album_GetDetail = "sp_Album_GetDetail";

        // Artist
        public const string sp_Artist_Delete = "sp_Artist_Delete";
        public const string sp_Artist_Insert = "sp_Artist_Insert";
        public const string sp_Artist_Search = "sp_Artist_Search";
        public const string sp_Artist_Update = "sp_Artist_Update";
        public const string sp_Artist_GetDetail = "sp_Artist_GetDetail";

        // Country
        public const string sp_Countries_Delete = "sp_Countries_Delete";
        public const string sp_Countries_Insert = "sp_Countries_Insert";
        public const string sp_Countries_Update = "sp_Countries_Update";
        public const string sp_Countries_Search = "sp_Countries_Search";
        public const string sp_Countries_GetDetail = "sp_Countries_GetDetail";

        // Genre
        public const string sp_Genre_Delete = "sp_Genre_Delete";
        public const string sp_Genre_Insert = "sp_Genre_Insert";
        public const string sp_Genre_Search = "sp_Genre_Search";
        public const string sp_Genre_GetDetail = "sp_Genre_GetDetail";

        // Playlist
        public const string sp_Playlist_Insert = "sp_Playlist_Insert";
        public const string sp_Playlist_Delete = "sp_Playlist_Remove";
        public const string sp_Playlist_Update_AddTrack = "sp_Playlist_Update_AddTrack";
        public const string sp_Playlist_Update_RemoveTrack = "sp_Playlist_Update_RemoveTrack";
        
        // Role
        public const string sp_Roles_Delete = "sp_Roles_Delete";
        public const string sp_Roles_Insert = "sp_Roles_Insert";
        public const string sp_Roles_Update = "sp_Roles_Update";
        
        // Track
        public const string sp_Track_Delete = "sp_Track_Delete";
        public const string sp_Track_Insert = "sp_Track_Insert";
        public const string sp_Track_Update = "sp_Track_Update";
        public const string sp_Track_Search = "sp_Track_Search";
        public const string sp_Track_GetDetail = "sp_Track_GetDetail";

        // ArtistAndTrack
        public const string sp_ArtistOfTrack_GetArtistOfTrack = "sp_ArtistOfTrack_GetArtistOfTrack";

        // GenreAndTrack
        public const string sp_GenreOfTrack_GetGenreOfTrack = "sp_GenreOfTrack_GetGenreOfTrack";
    }
}
