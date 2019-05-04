using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Timers;
using TatsugotchiWebAPI.Data;
using TatsugotchiWebAPI.Data.Repository;
using TatsugotchiWebAPI.Model.Interfaces;
using TatsugotchiWebAPI.BackgroundWorkers;

namespace TatsugotchiWebAPI {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private int _timerTime;
        private Timer _animalTimer;
        public ServiceProvider Services { get; private set; }

    // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            //Add db Context and use the connection string found in appsettings.json
            services.AddDbContext<ApplicationDBContext>(options =>
              options.UseSqlServer(
                  Configuration.GetConnectionString("DefaultConnection")
                  )
             );

            //scope data initializer
            services.AddScoped<DataInitializer>();

            //scope the repositories
            services.AddScoped<IAnimalRepository, AnimalRepository>();
            services.AddScoped<IBadgeRepository, BadgeRepository>();

            services.AddOpenApiDocument();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            _timerTime = Configuration.GetValue<int>("TimerEventPeriod");
            Services = services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,DataInitializer di,ApplicationDBContext context) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSwaggerUi3();
            app.UseSwagger();

            di.Seed();
            MakeTimerThread();
        }

        //Initializes timer, creates diffrent thread
        public void MakeTimerThread() {
            _animalTimer = new Timer(_timerTime) {
                AutoReset = false,
                Enabled = true
            };

            //Creates worker and make it subscribe
            AnimalWorker aw = new AnimalWorker(Services);
            aw.InitWorker();
            aw.AnimalWorkerComplete += OnWorkerCompleted;

            _animalTimer.Elapsed += (source,args) => OnTimedEvent(source,args,aw);
        }

        //Delegate the worker to do stuff
        public void OnTimedEvent(Object source,ElapsedEventArgs a,AnimalWorker aw) {
            System.Diagnostics.Debug.WriteLine("Timed event raised");
            aw.RunWorker();
            _animalTimer.Stop();
        }

        //Restart timer if worker completed
        public void OnWorkerCompleted(object source, EventArgs e) {
            System.Diagnostics.Debug.WriteLine("Timer starting");
            _animalTimer.Start();
        }

    }
}
