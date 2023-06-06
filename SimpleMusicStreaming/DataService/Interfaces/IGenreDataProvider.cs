using CoreLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLib.Interfaces
{
    public interface IGenreDataProvider
    {
        CResponseMessage Insert(CGenre genre);
        CResponseMessage Delete(int id);
        CResponseMessage Search(string name);
        CResponseMessage GetDetail(int id);
    }
}
