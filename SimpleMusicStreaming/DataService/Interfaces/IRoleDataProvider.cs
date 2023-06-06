using CoreLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLib.Interfaces
{
    public interface IRoleDataProvider
    {
        CResponseMessage Insert(CRole role);
        CResponseMessage Update(CRole role);
        CResponseMessage Delete(int id);
        CResponseMessage Search(string name);
    }
}
