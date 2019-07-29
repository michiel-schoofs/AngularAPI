using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TatsugotchiWebAPI.Data;
using TatsugotchiWebAPI.Model;
using TatsugotchiWebAPI.Model.Interfaces;

namespace TatsugotchiWebAPI.BackgroundWorkers
{
    public class MarketWorker : IWorker
    {
        private readonly IItemRepository _itemRepo;
        private readonly IMarketRepository _marketRepo;

        public MarketWorker(IItemRepository itemRepo,IMarketRepository marketRepo):base(){
            _itemRepo = itemRepo;
            _marketRepo = marketRepo;
        }

        public override void PreformDatabaseActions(){
            using (ApplicationDBContext context = new ApplicationDBContext()) {

                Market market = _marketRepo.GetMarketMultiEnv(context);

                foreach (var listing in market.Listings) {
                    listing.Quantity = 100;
                }

                context.SaveChanges();
            }
        }
    }
}
