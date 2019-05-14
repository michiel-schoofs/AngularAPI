using System;
using System.Timers;
using TatsugotchiWebAPI.BackgroundWorkers;

namespace TatsugotchiWebAPI.Scheduler {
    public class CustomTimer {
        private readonly Timer _timer;
        private readonly IWorker _worker;

        public CustomTimer(int milliseconds, IWorker worker) {
            _timer = new Timer() {
                AutoReset = false,
                Interval = milliseconds,
                Enabled = false
            };

            _worker = worker;
            SetupTimer();
        }

        private void SetupTimer() {
            _timer.Elapsed += DoWork;
            _worker.AnimalWorkerComplete += Reset;

        }

        private void DoWork(Object source, ElapsedEventArgs a) {
            System.Diagnostics.Debug.WriteLine("Timed event elapsed");
            _timer.Stop();
            _worker.RunWorker();
        }

        private void Reset(Object source, EventArgs a) {
            System.Diagnostics.Debug.WriteLine("Timed event completed,restarting timer");
            _timer.Start();
        }

        public void Start() {
            _timer.Start();
            System.Diagnostics.Debug.WriteLine("Timer started");
        }

    }
}
