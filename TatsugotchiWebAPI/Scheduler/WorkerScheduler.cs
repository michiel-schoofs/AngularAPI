using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TatsugotchiWebAPI.BackgroundWorkers;

namespace TatsugotchiWebAPI.Scheduler {
    public class WorkerScheduler {
        private readonly WorkerFactory _wf;
        private IEnumerable<CustomTimer> _timers;

        public WorkerScheduler(ServiceProvider sp,IDictionary<string,int> dic) {
            _wf = new WorkerFactory(sp);
            SetupTimers(dic);
        }

        private void SetupTimers(IDictionary<string, int> dic) {
            _timers = dic.Select(k => {
                        var worker = _wf.MakeWorker(k.Key);
                        return new CustomTimer(k.Value,worker);
                      }).ToList();
        }

        public void StartSchedule() {
            foreach(var timer in _timers) {
                timer.Start();
            }
        }

    }
}
