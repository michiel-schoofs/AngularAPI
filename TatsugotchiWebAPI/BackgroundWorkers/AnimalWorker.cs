using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Threading;
using TatsugotchiWebAPI.Model.Interfaces;

namespace TatsugotchiWebAPI.BackgroundWorkers {

    public class AnimalWorker {
        private BackgroundWorker bw;
        private IAnimalRepository _animalRepository;

        public delegate void AnimalWorkerHandler(object source, EventArgs e);
        public event AnimalWorkerHandler AnimalWorkerComplete;

        public AnimalWorker(ServiceProvider sp) {
            sp.CreateScope();
            _animalRepository = sp.GetService<IAnimalRepository>();
            bw = new BackgroundWorker();
        }

        public void InitWorker() {
            bw.DoWork += (sender, args) => {
                System.Diagnostics.Debug.WriteLine("Worker starting action");
                PreformDatabaseActions();
            };
            bw.RunWorkerCompleted += (sender, args) => { OnAnimalWorkerCompleted(args); };
        }

        public void RunWorker() {
            bw.RunWorkerAsync();
        }

        private void PreformDatabaseActions() {

        }

        protected virtual void OnAnimalWorkerCompleted(EventArgs args) {
            System.Diagnostics.Debug.WriteLine("Worker finished action");
            AnimalWorkerComplete?.Invoke(this, args);
        }
    }
}
