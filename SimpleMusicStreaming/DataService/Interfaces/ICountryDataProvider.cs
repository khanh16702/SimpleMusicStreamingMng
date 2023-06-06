using CoreLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLib.Interfaces
{
    public interface ICountryDataProvider
    {
        CResponseMessage Insert(CCountry country);
        CResponseMessage Update(CCountry country);
        CResponseMessage Delete(int id);
        CResponseMessage Search(string name);
        CResponseMessage GetDetail(int id);
    }
}
