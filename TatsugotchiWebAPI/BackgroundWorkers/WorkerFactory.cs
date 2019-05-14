using Microsoft.Extensions.DependencyInjection;
using System;
using TatsugotchiWebAPI.Model.Interfaces;

namespace TatsugotchiWebAPI.BackgroundWorkers {
    public class WorkerFactory {
        private readonly ServiceProvider _sp;

        public WorkerFactory(ServiceProvider sp) {
            _sp = sp;
        }

        public IWorker MakeWorker(string worker) {
            _sp.CreateScope();
            IWorker res;

            switch (worker) {
                case "AnimalWorker":
                    res = new AnimalWorker(_sp.GetService<IAnimalRepository>());
                    break;
                case "EggWorker":
                    res = new EggWorker(_sp.GetService<IEggRepository>());
                    break;
                case "DeathWorker":
                    res = new DeathWorker(_sp.GetService<IAnimalRepository>());
                    break;
                default:
                    throw new ArgumentException("Something went wrong with the scheduling");
            }

            return res;
        }
    }
}
