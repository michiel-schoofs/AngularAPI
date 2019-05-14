using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Threading;
using TatsugotchiWebAPI.Model.Interfaces;

namespace TatsugotchiWebAPI.BackgroundWorkers{

    public class AnimalWorker:IWorker{
        private readonly IAnimalRepository _animalRepository;

        public AnimalWorker(IAnimalRepository repo) :base() {
            _animalRepository = repo;
        }

        public override void PreformDatabaseActions() {
            System.Diagnostics.Debug.WriteLine("Preformed Animal operation");
        }
    }
}
