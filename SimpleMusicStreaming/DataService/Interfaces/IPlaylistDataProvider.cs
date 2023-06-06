using CoreLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLib.Interfaces
{
    public interface IPlaylistDataProvider
    {
        CResponseMessage Insert(CPlaylist playlist);
        CResponseMessage Update_AddTrack(int trackId, int playlistId);
        CResponseMessage Update_RemoveTrack(int trackId, int playlistId);
        CResponseMessage Delete(int id);
        CResponseMessage Search(string name);
    }
}
