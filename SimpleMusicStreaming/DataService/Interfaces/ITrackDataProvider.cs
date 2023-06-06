using CoreLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLib.Interfaces
{
    public interface ITrackDataProvider
    {
        CResponseMessage Insert(CTrack track, string artistsId, string genresId);
        CResponseMessage Update(CTrack track);
        CResponseMessage Delete(int id);
        CResponseMessage Search(string name);
        CResponseMessage GetDetail(int id);
    }
}
