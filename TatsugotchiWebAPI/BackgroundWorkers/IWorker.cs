using System;
using System.ComponentModel;

namespace TatsugotchiWebAPI.BackgroundWorkers {
    public abstract class IWorker {
        protected BackgroundWorker Bw {get;set;}
        public delegate void AnimalWorkerHandler(object source, EventArgs e);
        public event AnimalWorkerHandler AnimalWorkerComplete;

        public IWorker() {
            Bw = new BackgroundWorker();
            InitWorker();
        }

        private void InitWorker() {
            Bw.DoWork += (sender, args) => {
                System.Diagnostics.Debug.WriteLine("Worker starting action");
                PreformDatabaseActions();
            };
            Bw.RunWorkerCompleted += (sender, args) => { OnAnimalWorkerCompleted(args); };
        }

        public void RunWorker() {
           Bw.RunWorkerAsync();
        }

        public abstract void PreformDatabaseActions();

        public virtual void OnAnimalWorkerCompleted(EventArgs args) {
            System.Diagnostics.Debug.WriteLine("Worker finished action");
            AnimalWorkerComplete?.Invoke(this, args);

        }
    }
}
