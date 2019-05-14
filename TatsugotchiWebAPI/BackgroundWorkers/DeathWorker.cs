using System;
using TatsugotchiWebAPI.Model.Interfaces;

namespace TatsugotchiWebAPI.BackgroundWorkers {
    public class DeathWorker : IWorker {
        private readonly IAnimalRepository _repo;

        public DeathWorker(IAnimalRepository repo) {
            _repo = repo;
        }

        public override void PreformDatabaseActions() {
            System.Diagnostics.Debug.WriteLine("Preformed Death operation");
        }
    }
}
