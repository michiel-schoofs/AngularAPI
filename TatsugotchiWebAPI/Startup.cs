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
using TatsugotchiWebAPI.Data;
using TatsugotchiWebAPI.Data.Repository;
using TatsugotchiWebAPI.Model.Interfaces;

namespace TatsugotchiWebAPI {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,DataInitializer di) {
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
        }
    }
}
