using CoreLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Interfaces
{
    public interface IValidateApi
    {
        bool ValidateGetTabRatiosView(string lstStockCode, ref CResponseMessage CR);
        bool ValidateFinancial(string StockCode, string Language, string Type, ref CResponseMessage CR);
    }
}
