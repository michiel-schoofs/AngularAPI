﻿using System;
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
using TatsugotchiWebAPI.Scheduler;

namespace TatsugotchiWebAPI {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public ServiceProvider Services { get; private set; }

    // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            //Add db Context and use the connection string found in appsettings.json
            services.AddDbContext<ApplicationDBContext>();

            //scope data initializer
            services.AddScoped<DataInitializer>();

            //scope the repositories
            services.AddScoped<IAnimalRepository, AnimalRepository>()
                .AddScoped<IBadgeRepository, BadgeRepository>()
                .AddScoped<IEggRepository,EggRepository>();

            services.AddOpenApiDocument(d=> {
                d.Description = "The cutest API for the cutest animals around";
                d.Version = "Alpha";
                d.Title = "Tatsugotchi";
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

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
            MakeScheduler();
        }

        public void MakeScheduler() {
            IDictionary<string, int> values = GetTimerValues();
            WorkerScheduler js = new WorkerScheduler(Services, values);
            js.StartSchedule();
        }


        private IDictionary<string,int> GetTimerValues() {
            var x = Configuration.GetSection("Timers").GetChildren();

            IDictionary<string, int> dic = new Dictionary<string, int>();
            foreach (var obj in x) {
                try {
                    dic.Add(obj.Key, Int32.Parse(obj.Value));
                }
                catch (Exception e) {
                    System.Diagnostics.Debug.WriteLine("Configuration file timers not formated correctly");
                    continue;
                }
            }

            return dic;
        }
    }
}
