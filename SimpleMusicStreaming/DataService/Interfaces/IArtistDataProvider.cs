using CoreLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLib.Interfaces
{
    public interface IArtistDataProvider
    {
        CResponseMessage Insert(CArtist artist);
        CResponseMessage Delete(int id);
        CResponseMessage Search(string name);
        CResponseMessage Update(CArtist artist);
        CResponseMessage GetDetail(int id);
    }
}
