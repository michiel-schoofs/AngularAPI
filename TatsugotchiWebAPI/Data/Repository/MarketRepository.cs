using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TatsugotchiWebAPI.Model;
using TatsugotchiWebAPI.Model.Interfaces;

namespace TatsugotchiWebAPI.Data.Repository
{
    public class MarketRepository : IMarketRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly DbSet<Market> markets;

        public MarketRepository(ApplicationDBContext context){
            _context = context;
            markets = _context.Market;
        }

        public Market GetMarket(){
            return markets.Include(m => m.Listings)
                    .ThenInclude(ml => ml.Item).First();
        }
    }
}
