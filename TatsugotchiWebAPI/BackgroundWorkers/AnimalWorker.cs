using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Threading;
using TatsugotchiWebAPI.Data;
using TatsugotchiWebAPI.Model.Interfaces;

namespace TatsugotchiWebAPI.BackgroundWorkers{

    public class AnimalWorker:IWorker{
        private readonly IAnimalRepository _animalRepository;

        public AnimalWorker(IAnimalRepository repo) :base() {
            _animalRepository = repo;
        }

        public override void PreformDatabaseActions() {
            using (ApplicationDBContext context = new ApplicationDBContext()) { 
                var animals = _animalRepository.GetNotDeceasedAnimals(context);

                foreach(var animal in animals) {
                    animal.IncreaseHungerAndBoredom();
                }

                context.SaveChanges();
            }
            System.Diagnostics.Debug.WriteLine("Preformed Animal operation");
        }
    }
}
