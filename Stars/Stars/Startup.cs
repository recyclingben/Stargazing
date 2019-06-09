using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stars.StarManagement;
using Stars.Extensions;
using Stars.SignalR.Stars;
using Microsoft.Azure.SignalR;
using Stars.Data.Database;
using Stars.Data;

namespace Stars
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddCors(o => o.AddPolicy("FullCors", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                }));

            services.AddSingleton(Configuration);

            services.AddSignalR().AddAzureSignalR(connectionString: Configuration["Azure:SignalR:StarsSignalR:ConnectionString"]);

            Console.WriteLine(Configuration["Azure:SignalR:StarsSignalR:ConnectionString"]);           

            services.AddSingleton<IStarUpdatesEmitter, StarUpdatesEmitter>();
            services.AddSingleton<IStarCountData, StarCountData>();
            services.AddSingleton<IStarsTracker, StarsTracker>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAzureSignalR(routes =>
            {
                routes.MapHub<StarUpdatesHub>("/signalr/starupdates");
            });

            app.UseCors();
            app.UseMvc();
        }
    }
}
