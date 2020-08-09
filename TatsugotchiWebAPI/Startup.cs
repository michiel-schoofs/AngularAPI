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
using TatsugotchiWebAPI.Scheduler;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NSwag.SwaggerGeneration.Processors.Security;
using NSwag;

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
                .AddScoped<IEggRepository, EggRepository>()
                .AddScoped<IPetOwnerRepository, PetOwnerRepository>()
                .AddScoped<IListingRepository, ListingRepository>()
                .AddScoped<IItemRepository,ItemRepository>()
                .AddScoped<IMarketRepository,MarketRepository>()
                .AddScoped<IImageRepository,ImageRepository>();

            services.AddOpenApiDocument(d=> {
                d.Description = "The cutest API for the cutest animals around";
                d.Version = "Alpha";
                d.Title = "Tatsugotchi API";
                d.DocumentName = "Tatsugotchi API";
                d.DocumentProcessors.Add(new SecurityDefinitionAppender("JWT Token", new SwaggerSecurityScheme
                {
                    Type = SwaggerSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = SwaggerSecurityApiKeyLocation.Header,
                    Description = "Copy 'Bearer' + valid JWT token into field"
                }));
                d.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT Token"));
            });

            services.AddIdentity<IdentityUser, IdentityRole>(cfg => cfg.User.RequireUniqueEmail = true).AddEntityFrameworkStores<ApplicationDBContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 10;
                options.Password.RequiredUniqueChars = 4;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            services.AddCors(options => options.AddPolicy("AllowAllOrigins", builder => builder.AllowAnyOrigin()));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            Services = services.BuildServiceProvider();

            services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme =
                JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes("dsjfksldezeteiurhroijeo123489321564sqdfeijziofjzoe")),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = true //Ensure token hasn't expired
                };
            });
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
            app.UseAuthentication();
            app.UseMvc();

            app.UseCors("AllowAllOrigins");

            app.UseSwaggerUi3();
            app.UseSwagger();

            ////custom stuff added to pipeline
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
                catch (Exception) {
                    System.Diagnostics.Debug.WriteLine("Configuration file timers not formated correctly");
                    continue;
                }
            }

            return dic;
        }
    }
}
