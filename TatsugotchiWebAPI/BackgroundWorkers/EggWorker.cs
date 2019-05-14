using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TatsugotchiWebAPI.Model.Interfaces;

namespace TatsugotchiWebAPI.BackgroundWorkers {
    public class EggWorker : IWorker {
        private IEggRepository _repo;

        public EggWorker(IEggRepository repo) {
            _repo = repo;
        }

        public override void PreformDatabaseActions() {
            System.Diagnostics.Debug.WriteLine("Preformed Egg operation");
        }
    }
}
