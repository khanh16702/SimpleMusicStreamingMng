using CoreLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLib.Interfaces
{
    public interface IAccountDataProvider
    {
        CResponseMessage Insert(CAccount account);
        CResponseMessage Update(CAccount account);
        CResponseMessage Delete(int id);
        CResponseMessage Search(string name);

    }
}
