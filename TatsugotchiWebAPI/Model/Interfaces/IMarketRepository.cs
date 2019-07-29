using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TatsugotchiWebAPI.Data;

namespace TatsugotchiWebAPI.Model.Interfaces
{
    public interface IMarketRepository{
        Market GetMarketMultiEnv(ApplicationDBContext context);
        Market GetMarket();
    }
}
