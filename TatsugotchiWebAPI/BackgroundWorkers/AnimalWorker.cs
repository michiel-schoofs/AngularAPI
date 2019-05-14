using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Threading;
using TatsugotchiWebAPI.Model.Interfaces;

namespace TatsugotchiWebAPI.BackgroundWorkers{

    public class AnimalWorker:IWorker{
        private readonly IAnimalRepository _animalRepository;

        public AnimalWorker(ServiceProvider sp):base() {
            sp.CreateScope();
            _animalRepository = sp.GetService<IAnimalRepository>();

        }


        public override void PreformDatabaseActions() {
            throw new NotImplementedException();
        }
    }
}
