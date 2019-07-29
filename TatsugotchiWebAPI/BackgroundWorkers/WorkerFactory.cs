using Microsoft.Extensions.DependencyInjection;
using System;
using TatsugotchiWebAPI.Model.Interfaces;

namespace TatsugotchiWebAPI.BackgroundWorkers {
    public class WorkerFactory {
        private readonly ServiceProvider _sp;
        private readonly IAnimalRepository _animalRepo;
        private readonly IEggRepository _eggRepo;
        private readonly IItemRepository _itemRepo;
        private readonly IMarketRepository _marketRepo;

        public WorkerFactory(ServiceProvider sp) {
            _sp = sp;
            _sp.CreateScope();

            _animalRepo = _sp.GetService<IAnimalRepository>();
            _eggRepo = _sp.GetService<IEggRepository>();
            _itemRepo = _sp.GetService<IItemRepository>();
            _marketRepo = _sp.GetService<IMarketRepository>();
        }

        public IWorker MakeWorker(string worker) {

            IWorker res;

            switch (worker) {
                case "AnimalWorker":
                    res = new AnimalWorker(_animalRepo);
                    break;
                case "EggWorker":
                    res = new EggWorker(_eggRepo, _animalRepo);
                    break;
                case "MarketWorker":
                    res = new MarketWorker(_itemRepo, _marketRepo);
                    break;
                default:
                    throw new ArgumentException("Something went wrong with the scheduling");
            }

            return res;
        }
    }
}
